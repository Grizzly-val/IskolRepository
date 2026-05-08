namespace IskolRepository.Forms;

partial class StartupView
{
    private System.ComponentModel.IContainer components = null;

    private PictureBox logoMarkPictureBox = null!;
    private Label appNameLabel = null!;
    private Label taglineLabel = null!;
    private Button openSemesterButton = null!;
    private Button newSemesterButton = null!;
    private Label orLabel = null!;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
            components.Dispose();

        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        logoMarkPictureBox = new PictureBox();
        appNameLabel = new Label();
        taglineLabel = new Label();
        openSemesterButton = new Button();
        newSemesterButton = new Button();
        orLabel = new Label();

        ((System.ComponentModel.ISupportInitialize)logoMarkPictureBox).BeginInit();
        SuspendLayout();

        // StartupView
        this.Name = "StartupView";
        this.BackColor = Color.FromArgb(12, 14, 24);

        // ===== LOGO MARK (48×48px) =====
        logoMarkPictureBox.Size = new Size(48, 48);
        logoMarkPictureBox.BackColor = Color.Transparent;
        logoMarkPictureBox.SizeMode = PictureBoxSizeMode.CenterImage;

        // ===== APP NAME LABEL =====
        appNameLabel.Text = "IskolRepo";
        appNameLabel.Font = new Font("Segoe UI", 28F, FontStyle.Bold);
        appNameLabel.ForeColor = Color.White;
        appNameLabel.TextAlign = ContentAlignment.MiddleCenter;
        appNameLabel.BackColor = Color.Transparent;
        appNameLabel.AutoSize = true;

        // ===== TAGLINE LABEL =====
        taglineLabel.Text = "Your academic file organizer";
        taglineLabel.Font = new Font("Segoe UI", 14F, FontStyle.Regular);
        taglineLabel.ForeColor = Color.FromArgb(180, 190, 205);
        taglineLabel.TextAlign = ContentAlignment.MiddleCenter;
        taglineLabel.BackColor = Color.Transparent;
        taglineLabel.AutoSize = true;

        // ===== OPEN SEMESTER BUTTON =====
        openSemesterButton.Text = "Open Semester";
        openSemesterButton.Size = new Size(200, 50);
        openSemesterButton.FlatStyle = FlatStyle.Flat;
        openSemesterButton.FlatAppearance.BorderSize = 0;
        openSemesterButton.BackColor = Color.FromArgb(24, 47, 83);
        openSemesterButton.ForeColor = Color.White;
        openSemesterButton.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
        openSemesterButton.Cursor = Cursors.Hand;
        openSemesterButton.Click += openSemesterButton_Click;

        // ===== OR LABEL =====
        orLabel.Text = "OR";
        orLabel.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
        orLabel.ForeColor = Color.FromArgb(180, 190, 205);
        orLabel.BackColor = Color.Transparent;
        orLabel.TextAlign = ContentAlignment.MiddleCenter;
        orLabel.Size = new Size(40, 50);

        // ===== NEW SEMESTER BUTTON =====
        newSemesterButton.Text = "New Semester";
        newSemesterButton.Size = new Size(200, 50);
        newSemesterButton.FlatStyle = FlatStyle.Flat;
        newSemesterButton.FlatAppearance.BorderSize = 0;
        newSemesterButton.BackColor = Color.FromArgb(24, 47, 83);
        newSemesterButton.ForeColor = Color.White;
        newSemesterButton.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
        newSemesterButton.Cursor = Cursors.Hand;
        newSemesterButton.Click += newSemesterButton_Click;

        // Add controls to view
        Controls.Add(logoMarkPictureBox);
        Controls.Add(appNameLabel);
        Controls.Add(taglineLabel);
        Controls.Add(openSemesterButton);
        Controls.Add(orLabel);
        Controls.Add(newSemesterButton);

        ((System.ComponentModel.ISupportInitialize)logoMarkPictureBox).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }
}