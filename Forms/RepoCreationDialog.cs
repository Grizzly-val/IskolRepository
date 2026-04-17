using IskolRepository.Models;

namespace IskolRepository.Forms;

public partial class RepoCreationDialog : Form
{
    public RepoCreationDialog()
    {
        InitializeComponent();
        deadlineDateTimePicker.Value = DateTime.Today;
    }

    private RepoCreationInfo? result;

    public static RepoCreationInfo? ShowCreateDialog(IWin32Window owner)
    {
        using var dialog = new RepoCreationDialog();
        return dialog.ShowDialog(owner) == DialogResult.OK ? dialog.result : null;
    }

    private void okButton_Click(object? sender, EventArgs e)
    {
        var repositoryName = repositoryNameTextBox.Text.Trim();
        if (string.IsNullOrWhiteSpace(repositoryName))
        {
            MessageBox.Show(
                "A repository name is required.",
                "Input Required",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            return;
        }

        result = new RepoCreationInfo
        {
            RepositoryName = repositoryName,
            Deadline = deadlineDateTimePicker.Value.Date
        };

        DialogResult = DialogResult.OK;
        Close();
    }

    private void cancelButton_Click(object? sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }
}
