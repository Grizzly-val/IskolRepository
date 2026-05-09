using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

using IskolRepository.Utilities;

namespace IskolRepository.Forms;

public partial class StartupView : UserControl
{
    public StartupView()
    {
        InitializeComponent();
        ApplyStyle();

        Load += (s, e) =>
        {
            RoundButton(openSemesterButton, 20);
            RoundButton(newSemesterButton, 20);
            UpdateLogoAppearance();
        };

        openSemesterButton.Resize += (s, e) => RoundButton(openSemesterButton, 20);
        newSemesterButton.Resize += (s, e) => RoundButton(newSemesterButton, 20);
        logoMarkPictureBox.Resize += (s, e) => UpdateLogoAppearance();

        AnimationHelper.AnimateTextHover(openSemesterButton, 2f, 70);
        AnimationHelper.AnimateTextHover(newSemesterButton, 2f, 70);

    }

    public void UpdateTheme()
    {
        appNameLabel.ForeColor = ThemeManager.TextColor;
        taglineLabel.ForeColor = ThemeManager.MutedTextColor;
        orLabel.ForeColor = ThemeManager.MutedTextColor;

        // Force colors for labels if they were overridden
        appNameLabel.BackColor = Color.Transparent;
        taglineLabel.BackColor = Color.Transparent;
        orLabel.BackColor = Color.Transparent;

        ThemeManager.ApplyTheme(buttonLayout);
        UpdateLogoAppearance();
    }

    private void UpdateLogoAppearance()
    {
        logoMarkPictureBox.BackColor = ThemeManager.LogoBackColor;
        RoundControl(logoMarkPictureBox, 32);
    }

    private void RoundControl(Control control, int radius)
    {
        if (control.Width <= 0 || control.Height <= 0) return;

        using GraphicsPath path = new();
        path.AddArc(0, 0, radius, radius, 180, 90);
        path.AddArc(control.Width - radius, 0, radius, radius, 270, 90);
        path.AddArc(control.Width - radius, control.Height - radius, radius, radius, 0, 90);
        path.AddArc(0, control.Height - radius, radius, radius, 90, 90);
        path.CloseFigure();

        control.Region = new Region(path);
    }

    public event EventHandler? OpenSemesterRequested;
    public event EventHandler? NewSemesterRequested;

    private void openSemesterButton_Click(object? sender, EventArgs e)
        => OpenSemesterRequested?.Invoke(this, EventArgs.Empty);

    private void newSemesterButton_Click(object? sender, EventArgs e)
        => NewSemesterRequested?.Invoke(this, EventArgs.Empty);

    private void ApplyStyle()
    {
        DoubleBuffered = true;

        AddHoverEffect(openSemesterButton);
        AddHoverEffect(newSemesterButton);
    }

    private void AddHoverEffect(Button btn)
    {
        Color defaultColor = Color.FromArgb(24, 47, 83);
        Color hoverColor = Color.FromArgb(75, 143, 218);

        btn.MouseEnter += (s, e) => btn.BackColor = hoverColor;
        btn.MouseLeave += (s, e) => btn.BackColor = defaultColor;
    }

    private void RoundButton(Button button, int radius)
    {
        using GraphicsPath path = new();

        path.AddArc(0, 0, radius, radius, 180, 90);
        path.AddArc(button.Width - radius, 0, radius, radius, 270, 90);
        path.AddArc(button.Width - radius, button.Height - radius, radius, radius, 0, 90);
        path.AddArc(0, button.Height - radius, radius, radius, 90, 90);
        path.CloseFigure();

        button.Region = new Region(path);
    }
}
