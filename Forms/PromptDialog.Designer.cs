namespace IskolRepository.Forms;

partial class PromptDialog
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;
    private Label promptLabel = null!;
    private TextBox inputTextBox = null!;
    private Button okButton = null!;
    private Button cancelButton = null!;

    /// <summary>
    ///  Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources are used; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        promptLabel = new Label();
        inputTextBox = new TextBox();
        okButton = new Button();
        cancelButton = new Button();
        SuspendLayout();
        // 
        // promptLabel
        // 
        promptLabel.ForeColor = Color.FromArgb(40, 55, 70);
        promptLabel.Location = new Point(12, 12);
        promptLabel.Name = "promptLabel";
        promptLabel.Size = new Size(360, 36);
        promptLabel.TabIndex = 0;
        promptLabel.Text = "Prompt";
        // 
        // inputTextBox
        // 
        inputTextBox.Location = new Point(12, 51);
        inputTextBox.Name = "inputTextBox";
        inputTextBox.Size = new Size(360, 23);
        inputTextBox.TabIndex = 1;
        // 
        // okButton
        // 
        okButton.BackColor = Color.FromArgb(43, 87, 158);
        okButton.FlatAppearance.BorderSize = 0;
        okButton.FlatAppearance.MouseOverBackColor = Color.FromArgb(57, 97, 163);
        okButton.FlatStyle = FlatStyle.Flat;
        okButton.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        okButton.ForeColor = Color.White;
        okButton.Location = new Point(216, 92);
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
        cancelButton.Location = new Point(297, 92);
        cancelButton.Name = "cancelButton";
        cancelButton.Size = new Size(75, 30);
        cancelButton.TabIndex = 3;
        cancelButton.Text = "Cancel";
        cancelButton.UseVisualStyleBackColor = false;
        cancelButton.Click += cancelButton_Click;
        // 
        // PromptDialog
        // 
        AcceptButton = okButton;
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(240, 245, 250);
        CancelButton = cancelButton;
        ClientSize = new Size(384, 132);
        Controls.Add(cancelButton);
        Controls.Add(okButton);
        Controls.Add(inputTextBox);
        Controls.Add(promptLabel);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "PromptDialog";
        ShowInTaskbar = false;
        StartPosition = FormStartPosition.CenterParent;
        Text = "Input";
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion
}
