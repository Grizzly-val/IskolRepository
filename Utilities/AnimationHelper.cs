using Timer = System.Windows.Forms.Timer;

namespace IskolRepository.Utilities;

public static class AnimationHelper
{
    public static void AnimateHover(Control control, int growth, int duration)
    {
        _ = new HoverAnimationController(control, growth, duration);
    }

    public static void AnimateTextHover(Control control, float fontGrowth, int duration)
    {
        _ = new TextHoverAnimationController(control, fontGrowth, duration);
    }

    private class TextHoverAnimationController
    {
        private readonly Control _control;
        private readonly float _fontGrowth;
        private readonly int _duration;
        private readonly Timer _timer;
        private readonly Font _originalFont;
        private double _progress;
        private bool _isGrowing;

        public TextHoverAnimationController(Control control, float fontGrowth, int duration)
        {
            _control = control;
            _fontGrowth = fontGrowth;
            _duration = duration;
            _originalFont = control.Font;
            _progress = 0.0;
            _isGrowing = false;

            _timer = new Timer { Interval = 15 };
            _timer.Tick += OnTick;

            _control.MouseEnter += OnMouseEnter;
            _control.MouseLeave += OnMouseLeave;
            _control.Disposed += OnDisposed;
        }

        private void OnMouseEnter(object? sender, EventArgs e)
        {
            _isGrowing = true;
            _timer.Start();
        }

        private void OnMouseLeave(object? sender, EventArgs e)
        {
            _isGrowing = false;
            _timer.Start();
        }

        private void OnTick(object? sender, EventArgs e)
        {
            var step = (double)_timer.Interval / _duration;

            if (_isGrowing)
            {
                _progress = Math.Min(1.0, _progress + step);
            }
            else
            {
                _progress = Math.Max(0.0, _progress - step);
            }

            var easedProgress = 1 - Math.Pow(1 - _progress, 3); // Ease-out cubic

            var currentGrowth = (float)(_fontGrowth * easedProgress);
            var newSize = _originalFont.Size + currentGrowth;

            // To prevent creating new Font objects constantly if the size hasn't changed
            if (Math.Abs(_control.Font.Size - newSize) > 0.01f)
            {
                _control.Font = new Font(_originalFont.FontFamily, newSize, _originalFont.Style);
            }

            if ((_isGrowing && _progress >= 1.0) || (!_isGrowing && _progress <= 0.0))
            {
                _timer.Stop();
            }
        }

        private void OnDisposed(object? sender, EventArgs e)
        {
            _timer.Stop();
            _timer.Dispose();
        }
    }

    private class HoverAnimationController
    {
        private readonly Control _control;
        private readonly int _growth;
        private readonly int _duration;
        private readonly Timer _timer;
        private readonly Size _originalSize;
        private readonly Point _originalLocation;
        private double _progress;
        private bool _isGrowing;

        public HoverAnimationController(Control control, int growth, int duration)
        {
            _control = control;
            _growth = growth;
            _duration = duration;
            _originalSize = control.Size;
            _originalLocation = control.Location;
            _progress = 0.0;
            _isGrowing = false;

            // Enable Double Buffering to reduce flicker
            typeof(Control).GetProperty("DoubleBuffered", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.SetValue(_control, true, null);

            _timer = new Timer { Interval = 15 };
            _timer.Tick += OnTick;

            _control.MouseEnter += OnMouseEnter;
            _control.MouseLeave += OnMouseLeave;
            _control.Disposed += OnDisposed;
        }

        private void OnMouseEnter(object? sender, EventArgs e)
        {
            _isGrowing = true;
            _timer.Start();
        }

        private void OnMouseLeave(object? sender, EventArgs e)
        {
            _isGrowing = false;
            _timer.Start();
        }

        private void OnTick(object? sender, EventArgs e)
        {
            var step = (double)_timer.Interval / _duration;

            if (_isGrowing)
            {
                _progress = Math.Min(1.0, _progress + step);
            }
            else
            {
                _progress = Math.Max(0.0, _progress - step);
            }

            var easedProgress = 1 - Math.Pow(1 - _progress, 3); // Ease-out cubic

            var currentGrowth = (int)(_growth * easedProgress);
            var newSize = new Size(_originalSize.Width + currentGrowth, _originalSize.Height + currentGrowth);
            var newLocation = new Point(_originalLocation.X - currentGrowth / 2, _originalLocation.Y - currentGrowth / 2);

            if (_control.Size != newSize)
            {
                _control.Size = newSize;
            }
            if (_control.Location != newLocation)
            {
                _control.Location = newLocation;
            }

            if ((_isGrowing && _progress >= 1.0) || (!_isGrowing && _progress <= 0.0))
            {
                _timer.Stop();
            }
        }

        private void OnDisposed(object? sender, EventArgs e)
        {
            _timer.Stop();
            _timer.Dispose();
        }
    }

    public static async void AnimateControl(Control control, int duration, int delay)
    {
        await Task.Delay(delay);

        // Enable smoother rendering (important for WinForms)
        typeof(Control)
            .GetProperty("DoubleBuffered", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            ?.SetValue(control, true, null);

        var originalLocation = control.Location;
        var originalSize = control.Size;

        var startLocation = new Point(originalLocation.X, originalLocation.Y + 20);
        float startScale = 0.97f;

        // Precompute center (prevents jitter)
        var center = new Point(
            originalLocation.X + originalSize.Width / 2,
            originalLocation.Y + originalSize.Height / 2
        );

        control.SuspendLayout();

        control.Location = startLocation;
        control.Size = new Size(
            (int)(originalSize.Width * startScale),
            (int)(originalSize.Height * startScale)
        );

        control.Visible = true;

        control.ResumeLayout();

        var timer = new Timer { Interval = 16 }; // ~60 FPS
        var startTime = Environment.TickCount;

        timer.Tick += (s, e) =>
        {
            float elapsed = Environment.TickCount - startTime;
            float t = Math.Min(elapsed / duration, 1f);

            // Ease-out cubic
            float ease = 1 - (float)Math.Pow(1 - t, 3);

            // Position
            int newY = (int)(startLocation.Y + (originalLocation.Y - startLocation.Y) * ease);

            // Scale
            float scale = startScale + (1f - startScale) * ease;
            int newWidth = (int)(originalSize.Width * scale);
            int newHeight = (int)(originalSize.Height * scale);

            // Re-center based on fixed center point
            int newX = center.X - newWidth / 2;

            control.SuspendLayout();

            control.Size = new Size(newWidth, newHeight);
            control.Location = new Point(newX, newY);

            control.ResumeLayout();

            if (t >= 1f)
            {
                control.SuspendLayout();

                control.Size = originalSize;
                control.Location = originalLocation;

                control.ResumeLayout();

                timer.Stop();
                timer.Dispose();
            }
        };

        timer.Start();
    }
}