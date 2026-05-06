namespace IskolRepository.Forms;

partial class FileTypeDialog
{
    private System.ComponentModel.IContainer components = null;
    private Label fileTypeLabel = null!;
    private ComboBox fileTypeComboBox = null!;
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
        fileTypeLabel = new Label();
        fileTypeComboBox = new ComboBox();
        okButton = new Button();
        cancelButton = new Button();
        SuspendLayout();
        // 
        // fileTypeLabel
        // 
        fileTypeLabel.ForeColor = Color.FromArgb(40, 55, 70);
        fileTypeLabel.Location = new Point(12, 12);
        fileTypeLabel.Name = "fileTypeLabel";
        fileTypeLabel.Size = new Size(260, 20);
        fileTypeLabel.TabIndex = 0;
        fileTypeLabel.Text = "Choose a file type";
        // 
        // fileTypeComboBox
        // 
        fileTypeComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
        fileTypeComboBox.FormattingEnabled = true;
        fileTypeComboBox.Items.AddRange(new object[] { "Text File (.txt)", "Word Document (.docx)" });
        fileTypeComboBox.Location = new Point(12, 35);
        fileTypeComboBox.Name = "fileTypeComboBox";
        fileTypeComboBox.Size = new Size(260, 23);
        fileTypeComboBox.TabIndex = 1;
        // 
        // okButton
        // 
        okButton.BackColor = Color.FromArgb(43, 87, 158);
        okButton.FlatAppearance.BorderSize = 0;
        okButton.FlatAppearance.MouseOverBackColor = Color.FromArgb(57, 97, 163);
        okButton.FlatStyle = FlatStyle.Flat;
        okButton.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        okButton.ForeColor = Color.White;
        okButton.Location = new Point(116, 78);
        okButton.Name = "okButton";
        okButton.Size = new Size(75, 30);
        okButton.TabIndex = 2;
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
        cancelButton.Location = new Point(197, 78);
        cancelButton.Name = "cancelButton";
        cancelButton.Size = new Size(75, 30);
        cancelButton.TabIndex = 3;
        cancelButton.Text = "Cancel";
        cancelButton.UseVisualStyleBackColor = false;
        cancelButton.Click += cancelButton_Click;
        // 
        // FileTypeDialog
        // 
        AcceptButton = okButton;
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(240, 245, 250);
        CancelButton = cancelButton;
        ClientSize = new Size(284, 121);
        Controls.Add(cancelButton);
        Controls.Add(okButton);
        Controls.Add(fileTypeComboBox);
        Controls.Add(fileTypeLabel);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "FileTypeDialog";
        ShowInTaskbar = false;
        StartPosition = FormStartPosition.CenterParent;
        Text = "File Type";
        ResumeLayout(false);
    }
}
