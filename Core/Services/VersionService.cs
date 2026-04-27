using System.Text.Json;
using System.Windows.Forms;
using IskolRepository.Core.Interfaces;
using IskolRepository.Models;

namespace IskolRepository.Core.Services;

public class VersionService : IVersionService
{
    private static readonly string[] SupportedVersionExtensions = [".txt", ".docx"];

    private readonly JsonSerializerOptions _jsonOptions;

    public VersionService(JsonSerializerOptions jsonOptions)
    {
        _jsonOptions = jsonOptions ?? throw new ArgumentNullException(nameof(jsonOptions));
    }

    public void LoadVersionHistory(string? filePath, ListBox versionsListBox, Label captionLabel, Label? noVersionsMessageLabel)
    {
        if (versionsListBox is null)
            throw new ArgumentNullException(nameof(versionsListBox));

        if (captionLabel is null)
            throw new ArgumentNullException(nameof(captionLabel));

        versionsListBox.Items.Clear();
        captionLabel.Text = filePath != null
            ? $"Version History - {Path.GetFileName(filePath)}"
            : "Version History";

        if (string.IsNullOrWhiteSpace(filePath) || !File.Exists(filePath))
        {
            if (noVersionsMessageLabel is not null)
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

            if (noVersionsMessageLabel is not null)
            {
                noVersionsMessageLabel.Visible = versionsListBox.Items.Count == 0;
            }
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Unable to load file history.", ex);
        }
    }

    public void SaveVersion(string filePath, string comment)
    {
        if (string.IsNullOrWhiteSpace(filePath))
            throw new ArgumentException("File path cannot be empty.", nameof(filePath));

        if (string.IsNullOrWhiteSpace(comment))
            throw new ArgumentException("Version comment cannot be empty.", nameof(comment));

        if (!File.Exists(filePath))
            throw new InvalidOperationException("The selected file could not be found.");

        try
        {
            VersionHelper.SaveVersion(filePath, comment.Trim(), _jsonOptions);
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
