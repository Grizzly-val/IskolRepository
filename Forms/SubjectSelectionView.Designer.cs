namespace IskolRepository.Forms;

partial class SubjectSelectionView
{
    private System.ComponentModel.IContainer components = null;
    private FlowLayoutPanel subjectCardsPanel = null!;
    private Panel subjectHeaderPanel = null!;
    private Button changeSemesterButton = null!;
    private Button addSubjectButton = null!;
    private Label semesterNameValueLabel = null!;
    private Label semesterNameLabel = null!;
    private Label subjectsHeaderLabel = null!;

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
        semesterNameValueLabel = new Label();
        semesterNameLabel = new Label();
        subjectsHeaderLabel = new Label();
        subjectCardsPanel = new FlowLayoutPanel();
        subjectHeaderPanel.SuspendLayout();
        SuspendLayout();
        // 
        // subjectHeaderPanel
        // 
        subjectHeaderPanel.BackColor = Color.White;
        subjectHeaderPanel.Controls.Add(changeSemesterButton);
        subjectHeaderPanel.Controls.Add(addSubjectButton);
        subjectHeaderPanel.Controls.Add(semesterNameValueLabel);
        subjectHeaderPanel.Controls.Add(semesterNameLabel);
        subjectHeaderPanel.Controls.Add(subjectsHeaderLabel);
        subjectHeaderPanel.Dock = DockStyle.Top;
        subjectHeaderPanel.Location = new Point(0, 0);
        subjectHeaderPanel.Name = "subjectHeaderPanel";
        subjectHeaderPanel.Padding = new Padding(20);
        subjectHeaderPanel.Size = new Size(1401, 140);
        subjectHeaderPanel.TabIndex = 0;
        // 
        // changeSemesterButton
        // 
        changeSemesterButton.BackColor = Color.FromArgb(43, 87, 158);
        changeSemesterButton.Cursor = Cursors.Hand;
        changeSemesterButton.FlatAppearance.BorderSize = 0;
        changeSemesterButton.FlatAppearance.MouseOverBackColor = Color.FromArgb(57, 97, 163);
        changeSemesterButton.FlatStyle = FlatStyle.Flat;
        changeSemesterButton.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        changeSemesterButton.ForeColor = Color.White;
        changeSemesterButton.Location = new Point(170, 46);
        changeSemesterButton.Name = "changeSemesterButton";
        changeSemesterButton.Size = new Size(160, 40);
        changeSemesterButton.TabIndex = 1;
        changeSemesterButton.Text = "Change Semester";
        changeSemesterButton.UseVisualStyleBackColor = false;
        changeSemesterButton.Click += changeSemesterButton_Click;
        // 
        // addSubjectButton
        // 
        addSubjectButton.BackColor = Color.FromArgb(43, 87, 158);
        addSubjectButton.Cursor = Cursors.Hand;
        addSubjectButton.FlatAppearance.BorderSize = 0;
        addSubjectButton.FlatAppearance.MouseOverBackColor = Color.FromArgb(57, 97, 163);
        addSubjectButton.FlatStyle = FlatStyle.Flat;
        addSubjectButton.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        addSubjectButton.ForeColor = Color.White;
        addSubjectButton.ImageAlign = ContentAlignment.MiddleLeft;
        addSubjectButton.Location = new Point(20, 46);
        addSubjectButton.Name = "addSubjectButton";
        addSubjectButton.Size = new Size(130, 40);
        addSubjectButton.TabIndex = 0;
        addSubjectButton.Text = "Add Subject";
        addSubjectButton.UseVisualStyleBackColor = false;
        addSubjectButton.Click += addSubjectButton_Click;
        // 
        // semesterNameValueLabel
        // 
        semesterNameValueLabel.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
        semesterNameValueLabel.ForeColor = Color.FromArgb(43, 87, 158);
        semesterNameValueLabel.Location = new Point(366, 61);
        semesterNameValueLabel.Name = "semesterNameValueLabel";
        semesterNameValueLabel.Size = new Size(400, 30);
        semesterNameValueLabel.TabIndex = 3;
        semesterNameValueLabel.Text = "-";
        // 
        // semesterNameLabel
        // 
        semesterNameLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        semesterNameLabel.ForeColor = Color.FromArgb(40, 55, 70);
        semesterNameLabel.Location = new Point(366, 46);
        semesterNameLabel.Name = "semesterNameLabel";
        semesterNameLabel.Size = new Size(140, 15);
        semesterNameLabel.TabIndex = 2;
        semesterNameLabel.Text = "Active Semester";
        // 
        // subjectsHeaderLabel
        // 
        subjectsHeaderLabel.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
        subjectsHeaderLabel.ForeColor = Color.FromArgb(40, 55, 70);
        subjectsHeaderLabel.Location = new Point(20, 104);
        subjectsHeaderLabel.Name = "subjectsHeaderLabel";
        subjectsHeaderLabel.Size = new Size(110, 33);
        subjectsHeaderLabel.TabIndex = 4;
        subjectsHeaderLabel.Text = "Subjects";
        // 
        // subjectCardsPanel
        // 
        subjectCardsPanel.AutoScroll = true;
        subjectCardsPanel.BackColor = Color.FromArgb(240, 245, 250);
        subjectCardsPanel.Dock = DockStyle.Fill;
        subjectCardsPanel.Location = new Point(0, 140);
        subjectCardsPanel.Name = "subjectCardsPanel";
        subjectCardsPanel.Padding = new Padding(20);
        subjectCardsPanel.Size = new Size(1401, 433);
        subjectCardsPanel.TabIndex = 1;
        // 
        // SubjectSelectionView
        // 
        AutoScaleDimensions = new SizeF(7F, 17F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(240, 245, 250);
        Controls.Add(subjectCardsPanel);
        Controls.Add(subjectHeaderPanel);
        Font = new Font("Segoe UI", 10F);
        Name = "SubjectSelectionView";
        Size = new Size(1401, 573);
        subjectHeaderPanel.ResumeLayout(false);
        ResumeLayout(false);
    }
}
