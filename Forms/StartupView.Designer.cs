namespace IskolRepository.Forms;

partial class StartupView
{
    private System.ComponentModel.IContainer components = null;

    private ShadowLabel welcomeLabel = null!;
    private ShadowLabel appNameLabel = null!;
    private Label orLabel = null!;
    private Button openSemesterButton = null!;
    private Button newSemesterButton = null!;
    private Panel overlayPanel = null!;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
            components.Dispose();

        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        welcomeLabel = new ShadowLabel();
        appNameLabel = new ShadowLabel();
        orLabel = new Label();
        openSemesterButton = new Button();
        newSemesterButton = new Button();
        overlayPanel = new Panel();

        SuspendLayout();

        // StartupView
        this.Name = "StartupView";
        this.BackgroundImage = Image.FromFile("startup_bg.png");
        this.BackgroundImageLayout = ImageLayout.Stretch;

        // ===== TITLE (TOP) =====
        welcomeLabel.Text = "Welcome to";
        welcomeLabel.Font = new Font("Segoe UI", 36F, FontStyle.Regular);
        welcomeLabel.ForeColor = Color.FromArgb(245, 245, 240);
        welcomeLabel.TextAlign = ContentAlignment.MiddleCenter;
        welcomeLabel.BackColor = Color.Transparent;
        welcomeLabel.AutoSize = true;
        welcomeLabel.Location = new Point(120, 40);

        appNameLabel.Text = "IskolRepo";
        appNameLabel.Font = new Font("Segoe UI", 42F, FontStyle.Bold);
        appNameLabel.ForeColor = Color.FromArgb(245, 245, 240);
        appNameLabel.TextAlign = ContentAlignment.MiddleCenter;
        appNameLabel.BackColor = Color.Transparent;
        appNameLabel.AutoSize = true;
        appNameLabel.Location = new Point(120, 90);

        // ===== PANEL (ROUNDED BOX CONTAINER) =====
        overlayPanel.Size = new Size(700, 200);
        overlayPanel.BackColor = Color.FromArgb(40, 0, 0, 0);

        // Open Button
        openSemesterButton.Text = "Open Semester";
        openSemesterButton.Size = new Size(220, 70);
        openSemesterButton.Location = new Point(80, 60);
        openSemesterButton.FlatStyle = FlatStyle.Flat;
        openSemesterButton.FlatAppearance.BorderSize = 0;
        openSemesterButton.BackColor = Color.FromArgb(160, 43, 87, 158);
        openSemesterButton.ForeColor = Color.White;
        openSemesterButton.Font = new Font("Segoe UI", 14, FontStyle.Bold);
        openSemesterButton.Click += openSemesterButton_Click;

        // OR Label
        orLabel.Text = "OR";
        orLabel.Font = new Font("Segoe UI", 14, FontStyle.Bold);
        orLabel.ForeColor = Color.White;
        orLabel.BackColor = Color.Transparent;
        orLabel.TextAlign = ContentAlignment.MiddleCenter;
        orLabel.Size = new Size(60, 40);
        orLabel.Location = new Point(310, 75);

        // New Button
        newSemesterButton.Text = "New Semester";
        newSemesterButton.Size = new Size(220, 70);
        newSemesterButton.Location = new Point(400, 60);
        newSemesterButton.FlatStyle = FlatStyle.Flat;
        newSemesterButton.FlatAppearance.BorderSize = 0;
        newSemesterButton.BackColor = Color.FromArgb(160, 43, 87, 158);
        newSemesterButton.ForeColor = Color.White;
        newSemesterButton.Font = new Font("Segoe UI", 14, FontStyle.Bold);
        newSemesterButton.Click += newSemesterButton_Click;

        // Add controls to panel
        overlayPanel.Controls.Add(openSemesterButton);
        overlayPanel.Controls.Add(orLabel);
        overlayPanel.Controls.Add(newSemesterButton);

        // Add controls to view
        Controls.Add(overlayPanel);
        Controls.Add(welcomeLabel);
        Controls.Add(appNameLabel);

        ResumeLayout(false);
    }
}