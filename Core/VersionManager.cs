using System.Text.Json;
using System.Windows.Forms;
using IskolRepository.Forms;

namespace IskolRepository.Core;

/// <summary>
/// Manages version history operations including loading and saving file versions.
/// </summary>
public static class VersionManager
{
    public static void LoadVersionHistory(string? filePath, ListBox versionsListBox, JsonSerializerOptions jsonOptions, Label? historyCaptionLabel = null)
    {
        versionsListBox.Items.Clear();

        if (string.IsNullOrWhiteSpace(filePath) || !File.Exists(filePath))
        {
            UpdateHistoryUiState(null, versionsListBox, historyCaptionLabel);
            return;
        }

        try
        {
            var historyFolder = VersionHelper.GetHistoryFolderPath(filePath);
            var logEntries = VersionHelper.ReadVersionLog(filePath, jsonOptions);
            var extension = Path.GetExtension(filePath);

            foreach (var version in logEntries.OrderByDescending(v => v.Version))
            {
                var snapshotPath = Path.Combine(historyFolder, $"v{version.Version}{extension}");
                if (File.Exists(snapshotPath))
                {
                    versionsListBox.Items.Add(new VersionListItem(snapshotPath, version));
                }
            }

            UpdateHistoryUiState(filePath, versionsListBox, historyCaptionLabel);
        }
        catch (Exception ex)
        {
            UpdateHistoryUiState(null, versionsListBox, historyCaptionLabel);
            MessageBox.Show(
                $"Unable to load file history.\n\n{ex.Message}",
                "History Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    public static void PromptAndSaveVersion(string filePath, string? selectedFilePath, JsonSerializerOptions jsonOptions, ListBox versionsListBox)
    {
        if (!File.Exists(filePath))
        {
            return;
        }

        var comment = PromptDialog.ShowDialog(
            $"Enter a version comment for {Path.GetFileName(filePath)}:\n\nSelect Cancel to skip saving a snapshot.",
            "Save Version");

        if (string.IsNullOrWhiteSpace(comment))
        {
            return;
        }

        try
        {
            VersionHelper.SaveVersion(filePath, comment.Trim(), jsonOptions);

            if (string.Equals(selectedFilePath, filePath, StringComparison.OrdinalIgnoreCase))
            {
                LoadVersionHistory(filePath, versionsListBox, jsonOptions);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"Unable to save the file version.\n\n{ex.Message}",
                "Version Save Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    private static void UpdateHistoryUiState(string? filePath, ListBox versionsListBox, Label? historyCaptionLabel)
    {
        // Note: This is UI state management, caller should update button states
        if (historyCaptionLabel != null)
        {
            historyCaptionLabel.Text = !string.IsNullOrWhiteSpace(filePath)
                ? $"Version History - {Path.GetFileName(filePath)}"
                : "Version History";
        }
    }
}
