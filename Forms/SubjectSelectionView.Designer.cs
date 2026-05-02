namespace IskolRepository.Forms;

partial class SubjectSelectionView
{
    private System.ComponentModel.IContainer components = null;
    private FlowLayoutPanel subjectCardsPanel = null!;
    private Panel subjectHeaderPanel = null!;
    private Button changeSemesterButton = null!;
    private Button addSubjectButton = null!;
    private Label semesterPathValueLabel = null!;
    private Label semesterPathLabel = null!;
    private Label semesterNameValueLabel = null!;
    private Label semesterNameLabel = null!;

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
        subjectHeaderPanel = new Panel();
        changeSemesterButton = new Button();
        addSubjectButton = new Button();
        semesterPathValueLabel = new Label();
        semesterPathLabel = new Label();
        semesterNameValueLabel = new Label();
        semesterNameLabel = new Label();
        subjectCardsPanel = new FlowLayoutPanel();
        subjectHeaderPanel.SuspendLayout();
        SuspendLayout();
        // 
        // subjectHeaderPanel
        // 
        subjectHeaderPanel.Controls.Add(changeSemesterButton);
        subjectHeaderPanel.Controls.Add(addSubjectButton);
        subjectHeaderPanel.Controls.Add(semesterPathValueLabel);
        subjectHeaderPanel.Controls.Add(semesterPathLabel);
        subjectHeaderPanel.Controls.Add(semesterNameValueLabel);
        subjectHeaderPanel.Controls.Add(semesterNameLabel);
        subjectHeaderPanel.Dock = DockStyle.Top;
        subjectHeaderPanel.Location = new Point(0, 0);
        subjectHeaderPanel.Name = "subjectHeaderPanel";
        subjectHeaderPanel.Size = new Size(966, 120);
        subjectHeaderPanel.TabIndex = 0;
        // 
        // changeSemesterButton
        // 
        changeSemesterButton.Location = new Point(156, 14);
        changeSemesterButton.Name = "changeSemesterButton";
        changeSemesterButton.Size = new Size(138, 32);
        changeSemesterButton.TabIndex = 1;
        changeSemesterButton.Text = "Change Semester";
        changeSemesterButton.UseVisualStyleBackColor = true;
        changeSemesterButton.Click += changeSemesterButton_Click;
        // 
        // addSubjectButton
        // 
        addSubjectButton.Location = new Point(0, 14);
        addSubjectButton.Name = "addSubjectButton";
        addSubjectButton.Size = new Size(138, 32);
        addSubjectButton.TabIndex = 0;
        addSubjectButton.Text = "Add Subject";
        addSubjectButton.UseVisualStyleBackColor = true;
        addSubjectButton.Click += addSubjectButton_Click;
        // 
        // semesterPathValueLabel
        // 
        semesterPathValueLabel.Location = new Point(0, 88);
        semesterPathValueLabel.Name = "semesterPathValueLabel";
        semesterPathValueLabel.Size = new Size(930, 20);
        semesterPathValueLabel.TabIndex = 5;
        semesterPathValueLabel.Text = "-";
        // 
        // semesterPathLabel
        // 
        semesterPathLabel.Location = new Point(0, 68);
        semesterPathLabel.Name = "semesterPathLabel";
        semesterPathLabel.Size = new Size(140, 20);
        semesterPathLabel.TabIndex = 4;
        semesterPathLabel.Text = "Semester Path";
        // 
        // semesterNameValueLabel
        // 
        semesterNameValueLabel.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
        semesterNameValueLabel.Location = new Point(0, 44);
        semesterNameValueLabel.Name = "semesterNameValueLabel";
        semesterNameValueLabel.Size = new Size(320, 24);
        semesterNameValueLabel.TabIndex = 3;
        semesterNameValueLabel.Text = "-";
        // 
        // semesterNameLabel
        // 
        semesterNameLabel.Location = new Point(0, 24);
        semesterNameLabel.Name = "semesterNameLabel";
        semesterNameLabel.Size = new Size(140, 20);
        semesterNameLabel.TabIndex = 2;
        semesterNameLabel.Text = "Active Semester";
        // 
        // subjectCardsPanel
        // 
        subjectCardsPanel.AutoScroll = true;
        subjectCardsPanel.Dock = DockStyle.Fill;
        subjectCardsPanel.Location = new Point(0, 120);
        subjectCardsPanel.Name = "subjectCardsPanel";
        subjectCardsPanel.Padding = new Padding(0, 8, 0, 0);
        subjectCardsPanel.Size = new Size(966, 640);
        subjectCardsPanel.TabIndex = 1;
        // 
        // SubjectSelectionView
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        Controls.Add(subjectCardsPanel);
        Controls.Add(subjectHeaderPanel);
        Dock = DockStyle.Fill;
        Name = "SubjectSelectionView";
        Size = new Size(966, 760);
        subjectHeaderPanel.ResumeLayout(false);
        ResumeLayout(false);
    }
}
