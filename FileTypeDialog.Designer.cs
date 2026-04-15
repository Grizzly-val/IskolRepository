namespace IskolRepository;

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
        okButton.Location = new Point(116, 78);
        okButton.Name = "okButton";
        okButton.Size = new Size(75, 28);
        okButton.TabIndex = 2;
        okButton.Text = "OK";
        okButton.UseVisualStyleBackColor = true;
        okButton.Click += okButton_Click;
        // 
        // cancelButton
        // 
        cancelButton.Location = new Point(197, 78);
        cancelButton.Name = "cancelButton";
        cancelButton.Size = new Size(75, 28);
        cancelButton.TabIndex = 3;
        cancelButton.Text = "Cancel";
        cancelButton.UseVisualStyleBackColor = true;
        cancelButton.Click += cancelButton_Click;
        // 
        // FileTypeDialog
        // 
        AcceptButton = okButton;
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
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
