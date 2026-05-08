using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace IskolRepository.Forms;

public partial class StartupView : UserControl
{
    public StartupView()
    {
        InitializeComponent();
        CreateLogoMark();
        ApplyStyle();

        this.Resize += (s, e) => CenterAllControls();
        this.Load += (s, e) =>
        {
            RoundButton(openSemesterButton, 20);
            RoundButton(newSemesterButton, 20);
            CenterAllControls();
        };
    }

    private void CenterAllControls()
    {
        int centerX = this.Width / 2;
        
        // Calculate total height of all content
        int logoHeight = 48;
        int logoToNameGap = 16;
        int nameToTaglineGap = 8;
        int taglineToButtonGap = 24;
        int buttonHeight = 50;
        
        int totalContentHeight = logoHeight + logoToNameGap + appNameLabel.Height + 
                                nameToTaglineGap + taglineLabel.Height + taglineToButtonGap + buttonHeight;
        
        // Start from vertical center minus half the content height
        int startY = (this.Height - totalContentHeight) / 2;

        // Logo mark centered horizontally
        logoMarkPictureBox.Left = centerX - logoMarkPictureBox.Width / 2;
        logoMarkPictureBox.Top = startY;

        // App name centered horizontally, below logo with some spacing
        appNameLabel.Left = centerX - appNameLabel.Width / 2;
        appNameLabel.Top = logoMarkPictureBox.Bottom + logoToNameGap;

        // Tagline centered horizontally, below app name
        taglineLabel.Left = centerX - taglineLabel.Width / 2;
        taglineLabel.Top = appNameLabel.Bottom + nameToTaglineGap;

        // Button group: 24px gap between logo/tagline block and buttons
        int buttonGroupTop = taglineLabel.Bottom + taglineToButtonGap;
        
        // Calculate horizontal positions for button group (centered)
        int totalButtonWidth = openSemesterButton.Width + orLabel.Width + newSemesterButton.Width;
        int buttonGroupLeft = centerX - totalButtonWidth / 2;

        openSemesterButton.Left = buttonGroupLeft;
        openSemesterButton.Top = buttonGroupTop;

        orLabel.Left = openSemesterButton.Right;
        orLabel.Top = buttonGroupTop;

        newSemesterButton.Left = orLabel.Right;
        newSemesterButton.Top = buttonGroupTop;
    }

    private void CreateLogoMark()
    {
        // Create a 48x48 white outline folder icon
        Bitmap logoImage = new Bitmap(48, 48);
        using (Graphics g = Graphics.FromImage(logoImage))
        {
            g.Clear(Color.Transparent);
            g.SmoothingMode = SmoothingMode.AntiAlias;

            // Draw folder outline
            Pen whitePen = new Pen(Color.White, 2.5f);

            // Folder tab (top-left rectangle)
            g.DrawRectangle(whitePen, 6, 6, 18, 10);

            // Main folder body
            g.DrawRectangle(whitePen, 6, 14, 36, 24);

            // Folder flap line
            g.DrawLine(whitePen, 24, 14, 24, 24);
        }

        logoMarkPictureBox.Image = logoImage;
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
        var path = new GraphicsPath();

        path.AddArc(0, 0, radius, radius, 180, 90);
        path.AddArc(button.Width - radius, 0, radius, radius, 270, 90);
        path.AddArc(button.Width - radius, button.Height - radius, radius, radius, 0, 90);
        path.AddArc(0, button.Height - radius, radius, radius, 90, 90);
        path.CloseFigure();

        button.Region = new Region(path);
    }
}