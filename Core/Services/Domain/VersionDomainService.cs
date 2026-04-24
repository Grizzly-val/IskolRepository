using System.Windows.Forms;
using IskolRepository.Core.Interfaces.Domain;
using IskolRepository.Forms;
using IskolRepository.Models;

namespace IskolRepository.Core.Services.Domain;

/// <summary>
/// Implementation of IVersionDomainService.
/// </summary>
public class VersionDomainService : IVersionDomainService
{
    private static readonly string[] SupportedVersionExtensions = [".txt", ".docx"];
    private readonly System.Text.Json.JsonSerializerOptions _jsonOptions;

    public VersionDomainService(System.Text.Json.JsonSerializerOptions jsonOptions)
    {
        _jsonOptions = jsonOptions ?? throw new ArgumentNullException(nameof(jsonOptions));
    }
    public void LoadVersionHistory(string? filePath, ListBox versionsListBox, Label captionLabel, Label? noVersionsMessageLabel = null)
    {
        versionsListBox.Items.Clear();

        if (captionLabel != null)
        {
            captionLabel.Text = filePath != null
                ? $"Version History - {Path.GetFileName(filePath)}"
                : "Version History";
        }

        if (string.IsNullOrWhiteSpace(filePath) || !File.Exists(filePath))
        {
            if (noVersionsMessageLabel != null)
                noVersionsMessageLabel.Visible = false;
            return;
        }

        try
        {
            var historyFolder = VersionHelper.GetHistoryFolderPath(filePath);
            var logEntries = VersionHelper.ReadVersionLog(filePath, _jsonOptions);
            var extension = Path.GetExtension(filePath);

            foreach (var version in logEntries.OrderByDescending(v => v.Version))
            {
                var snapshotPath = Path.Combine(historyFolder, $"v{version.Version}{extension}");
                if (File.Exists(snapshotPath))
                {
                    versionsListBox.Items.Add(new FileVersion(version.Version, version.Timestamp, version.Comment, snapshotPath));
                }
            }

            // Show/hide the no versions message based on whether items exist
            if (noVersionsMessageLabel != null)
            {
                noVersionsMessageLabel.Visible = versionsListBox.Items.Count == 0;
            }
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Unable to load file history.", ex);
        }
    }

    public void PromptAndSaveVersion(string filePath, string? selectedFilePath, ListBox versionsListBox)
    {
        if (!File.Exists(filePath))
            return;

        var comment = PromptDialog.ShowDialog(
            $"Enter a version comment for {Path.GetFileName(filePath)}:\n\nSelect Cancel to skip saving a snapshot.",
            "Save Version");

        if (string.IsNullOrWhiteSpace(comment))
            return;

        try
        {
            VersionHelper.SaveVersion(filePath, comment.Trim(), _jsonOptions);

            if (string.Equals(selectedFilePath, filePath, StringComparison.OrdinalIgnoreCase))
            {
                LoadVersionHistory(filePath, versionsListBox, null!);
            }
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Unable to save the file version.", ex);
        }
    }

    public bool IsSupportedVersionFileType(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath))
            return false;

        var extension = Path.GetExtension(filePath);
        return SupportedVersionExtensions.Contains(extension, StringComparer.OrdinalIgnoreCase);
    }

    public bool CanSaveVersion(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath) || !File.Exists(filePath))
            return false;

        if (!IsSupportedVersionFileType(filePath))
            return false;

        var historyFolder = VersionHelper.GetHistoryFolderPath(filePath);
        var logEntries = VersionHelper.ReadVersionLog(filePath, _jsonOptions);
        if (logEntries.Count == 0)
            return true;

        var latestVersion = logEntries.MaxBy(v => v.Version);
        if (latestVersion is null)
            return true;

        var extension = Path.GetExtension(filePath);
        var latestSnapshotPath = Path.Combine(historyFolder, $"v{latestVersion.Version}{extension}");
        if (!File.Exists(latestSnapshotPath))
            return true;

        var currentWriteTime = File.GetLastWriteTime(filePath);
        var latestSnapshotWriteTime = File.GetLastWriteTime(latestSnapshotPath);
        return currentWriteTime != latestSnapshotWriteTime;
    }

    public void RevertToVersion(string filePath, FileVersion selectedVersion)
    {
        if (string.IsNullOrWhiteSpace(filePath))
            throw new ArgumentException("File path cannot be empty.", nameof(filePath));

        if (selectedVersion is null)
            throw new ArgumentNullException(nameof(selectedVersion));

        try
        {
            VersionHelper.RevertToVersion(filePath, selectedVersion, selectedVersion.SnapshotPath, _jsonOptions);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Unable to revert the selected file version.", ex);
        }
    }
}

