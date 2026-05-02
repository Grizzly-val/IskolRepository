namespace IskolRepository.Forms;

partial class StartupView
{
    private System.ComponentModel.IContainer components = null;
    private Label startupTitleLabel = null!;
    private Label startupSubtitleLabel = null!;
    private Button openSemesterButton = null!;
    private Button newSemesterButton = null!;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        startupTitleLabel = new Label();
        startupSubtitleLabel = new Label();
        openSemesterButton = new Button();
        newSemesterButton = new Button();
        SuspendLayout();
        // 
        // startupTitleLabel
        // 
        startupTitleLabel.Font = new Font("Segoe UI", 22F, FontStyle.Bold);
        startupTitleLabel.Location = new Point(32, 32);
        startupTitleLabel.Name = "startupTitleLabel";
        startupTitleLabel.Size = new Size(420, 48);
        startupTitleLabel.TabIndex = 0;
        startupTitleLabel.Text = "IskolRepository";
        // 
        // startupSubtitleLabel
        // 
        startupSubtitleLabel.Location = new Point(40, 90);
        startupSubtitleLabel.Name = "startupSubtitleLabel";
        startupSubtitleLabel.Size = new Size(560, 48);
        startupSubtitleLabel.TabIndex = 1;
        startupSubtitleLabel.Text = "Open an existing semester folder or create a new empty semester workspace to start organizing subjects, repositories, files, and versions.";
        // 
        // openSemesterButton
        // 
        openSemesterButton.Location = new Point(40, 170);
        openSemesterButton.Name = "openSemesterButton";
        openSemesterButton.Size = new Size(170, 44);
        openSemesterButton.TabIndex = 2;
        openSemesterButton.Text = "Open Semester";
        openSemesterButton.UseVisualStyleBackColor = true;
        openSemesterButton.Click += openSemesterButton_Click;
        // 
        // newSemesterButton
        // 
        newSemesterButton.Location = new Point(226, 170);
        newSemesterButton.Name = "newSemesterButton";
        newSemesterButton.Size = new Size(170, 44);
        newSemesterButton.TabIndex = 3;
        newSemesterButton.Text = "New Semester";
        newSemesterButton.UseVisualStyleBackColor = true;
        newSemesterButton.Click += newSemesterButton_Click;
        // 
        // StartupView
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        Controls.Add(newSemesterButton);
        Controls.Add(openSemesterButton);
        Controls.Add(startupSubtitleLabel);
        Controls.Add(startupTitleLabel);
        Name = "StartupView";
        Size = new Size(1450, 573);
        ResumeLayout(false);
    }
}
