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
        ApplyStyle();

        this.Resize += (s, e) =>
        {
            overlayPanel.Left = (this.Width - overlayPanel.Width) / 2;
            overlayPanel.Top = this.Height / 2;

            appNameLabel.Left = (this.Width - appNameLabel.Width) / 2;
            appNameLabel.Top = overlayPanel.Top - appNameLabel.Height - 10;

            welcomeLabel.Left = (this.Width - welcomeLabel.Width) / 2;
            welcomeLabel.Top = appNameLabel.Top - welcomeLabel.Height - 5;

            orLabel.Left = newSemesterButton.Right
                + (openSemesterButton.Left - newSemesterButton.Right - orLabel.Width) / 2;
            orLabel.Top = newSemesterButton.Top
                + (newSemesterButton.Height - orLabel.Height) / 2;
        };

        this.Load += (s, e) =>
        {
            RoundButton(openSemesterButton, 30);
            RoundButton(newSemesterButton, 30);
            RoundPanel(overlayPanel, 40);
        };
    }

    private void RoundPanel(Panel panel, int radius)
    {
        var path = new GraphicsPath();

        path.AddArc(0, 0, radius, radius, 180, 90);
        path.AddArc(panel.Width - radius, 0, radius, radius, 270, 90);
        path.AddArc(panel.Width - radius, panel.Height - radius, radius, radius, 0, 90);
        path.AddArc(0, panel.Height - radius, radius, radius, 90, 90);
        path.CloseFigure();

        panel.Region = new Region(path);
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
        btn.MouseEnter += (s, e) =>
            btn.BackColor = Color.FromArgb(200, 57, 97, 163);

        btn.MouseLeave += (s, e) =>
            btn.BackColor = Color.FromArgb(160, 43, 87, 158);
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