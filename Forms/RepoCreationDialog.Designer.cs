namespace IskolRepository.Forms;

partial class RepoCreationDialog
{
    private System.ComponentModel.IContainer components = null;
    private Label repositoryNameLabel = null!;
    private TextBox repositoryNameTextBox = null!;
    private Label deadlineLabel = null!;
    private DateTimePicker deadlineDateTimePicker = null!;
    private Button okButton = null!;
    private Button cancelButton = null!;

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
        components = new System.ComponentModel.Container();
        repositoryNameLabel = new Label();
        repositoryNameTextBox = new TextBox();
        deadlineLabel = new Label();
        deadlineDateTimePicker = new DateTimePicker();
        okButton = new Button();
        cancelButton = new Button();
        SuspendLayout();
        // 
        // repositoryNameLabel
        // 
        repositoryNameLabel.ForeColor = Color.FromArgb(40, 55, 70);
        repositoryNameLabel.Location = new Point(12, 12);
        repositoryNameLabel.Name = "repositoryNameLabel";
        repositoryNameLabel.Size = new Size(360, 20);
        repositoryNameLabel.TabIndex = 0;
        repositoryNameLabel.Text = "Repository Name";
        // 
        // repositoryNameTextBox
        // 
        repositoryNameTextBox.Location = new Point(12, 35);
        repositoryNameTextBox.Name = "repositoryNameTextBox";
        repositoryNameTextBox.Size = new Size(360, 23);
        repositoryNameTextBox.TabIndex = 1;
        // 
        // deadlineLabel
        // 
        deadlineLabel.ForeColor = Color.FromArgb(40, 55, 70);
        deadlineLabel.Location = new Point(12, 70);
        deadlineLabel.Name = "deadlineLabel";
        deadlineLabel.Size = new Size(360, 20);
        deadlineLabel.TabIndex = 2;
        deadlineLabel.Text = "Deadline";
        // 
        // deadlineDateTimePicker
        // 
        deadlineDateTimePicker.Format = DateTimePickerFormat.Short;
        deadlineDateTimePicker.Location = new Point(12, 93);
        deadlineDateTimePicker.Name = "deadlineDateTimePicker";
        deadlineDateTimePicker.Size = new Size(160, 23);
        deadlineDateTimePicker.TabIndex = 3;
        // 
        // okButton
        // 
        okButton.BackColor = Color.FromArgb(43, 87, 158);
        okButton.FlatAppearance.BorderSize = 0;
        okButton.FlatAppearance.MouseOverBackColor = Color.FromArgb(57, 97, 163);
        okButton.FlatStyle = FlatStyle.Flat;
        okButton.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        okButton.ForeColor = Color.White;
        okButton.Location = new Point(216, 136);
        okButton.Name = "okButton";
        okButton.Size = new Size(75, 30);
        okButton.TabIndex = 4;
        okButton.Text = "OK";
        okButton.UseVisualStyleBackColor = false;
        okButton.Click += okButton_Click;
        // 
        // cancelButton
        // 
        cancelButton.BackColor = Color.FromArgb(43, 87, 158);
        cancelButton.FlatAppearance.BorderSize = 0;
        cancelButton.FlatAppearance.MouseOverBackColor = Color.FromArgb(57, 97, 163);
        cancelButton.FlatStyle = FlatStyle.Flat;
        cancelButton.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        cancelButton.ForeColor = Color.White;
        cancelButton.Location = new Point(297, 136);
        cancelButton.Name = "cancelButton";
        cancelButton.Size = new Size(75, 30);
        cancelButton.TabIndex = 5;
        cancelButton.Text = "Cancel";
        cancelButton.UseVisualStyleBackColor = false;
        cancelButton.Click += cancelButton_Click;
        // 
        // RepoCreationDialog
        // 
        AcceptButton = okButton;
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(240, 245, 250);
        CancelButton = cancelButton;
        ClientSize = new Size(384, 181);
        Controls.Add(cancelButton);
        Controls.Add(okButton);
        Controls.Add(deadlineDateTimePicker);
        Controls.Add(deadlineLabel);
        Controls.Add(repositoryNameTextBox);
        Controls.Add(repositoryNameLabel);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "RepoCreationDialog";
        ShowInTaskbar = false;
        StartPosition = FormStartPosition.CenterParent;
        Text = "Create Repository";
        ResumeLayout(false);
        PerformLayout();
    }
}
