using IskolRepository.Utilities;

namespace IskolRepository.Forms;

public partial class FileTypeDialog : Form
{
    private string? selectedExtension;

    public FileTypeDialog()
    {
        InitializeComponent();
        ThemeManager.ThemeChanged += OnThemeChanged;
        ThemeManager.ApplyTheme(this);
        ThemeManager.SetupButton(okButton);
        ThemeManager.SetupButton(cancelButton);
        fileTypeComboBox.SelectedIndex = 0;
    }

    private void OnThemeChanged(object? sender, EventArgs e)
    {
        ThemeManager.ApplyTheme(this);
    }

    public static string? ShowCreateDialog(IWin32Window owner)
    {
        using var dialog = new FileTypeDialog();
        return dialog.ShowDialog(owner) == DialogResult.OK ? dialog.selectedExtension : null;
    }

    private void okButton_Click(object? sender, EventArgs e)
    {
        selectedExtension = fileTypeComboBox.SelectedItem?.ToString() switch
        {
            "Text File (.txt)" => ".txt",
            "Word Document (.docx)" => ".docx",
            "PowerPoint Presentation (.pptx)" => ".pptx",
            "Excel Spreadsheet (.xlsx)" => ".xlsx",
            "Publisher Document (.pub)" => ".pub",
            "OneNote Notebook (.one)" => ".one",
            "Access Database (.accdb)" => ".accdb",
            _ => null
        };

        if (string.IsNullOrWhiteSpace(selectedExtension))
        {
            MessageBox.Show(
                "Please choose a valid file type.",
                "Invalid File Type",
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
