namespace IskolRepository.Forms;

public partial class PromptDialog : Form
{
    public PromptDialog(string prompt, string title)
    {
        InitializeComponent();
        promptLabel.Text = prompt;
        Text = title;
    }

    public string ResponseText => inputTextBox.Text.Trim();

    public static string? ShowDialog(string prompt, string title)
    {
        using var dialog = new PromptDialog(prompt, title);
        return dialog.ShowDialog() == DialogResult.OK ? dialog.ResponseText : null;
    }

    private void okButton_Click(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(inputTextBox.Text))
        {
            MessageBox.Show(
                "A value is required.",
                "Input Required",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            return;
        }

        DialogResult = DialogResult.OK;
        Close();
    }

    private void cancelButton_Click(object? sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }
}
