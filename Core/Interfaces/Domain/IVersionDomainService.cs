using System.Windows.Forms;
using IskolRepository.Models;

namespace IskolRepository.Core.Interfaces.Domain;

/// <summary>
/// Domain service for version history operations.
/// </summary>
public interface IVersionDomainService
{
    /// <summary>
    /// Loads version history for a file.
    /// </summary>
    void LoadVersionHistory(string? filePath, ListBox versionsListBox, Label captionLabel, Label? noVersionsMessageLabel);

    /// <summary>
    /// Prompts user and saves a new version.
    /// </summary>
    void PromptAndSaveVersion(string filePath, string? selectedFilePath, ListBox versionsListBox);

    /// <summary>
    /// Checks if a file extension is supported for version snapshots.
    /// </summary>
    bool IsSupportedVersionFileType(string filePath);

    /// <summary>
    /// Determines whether a new version can be saved for a file.
    /// </summary>
    bool CanSaveVersion(string filePath);

    /// <summary>
    /// Reverts a file to a previous version.
    /// </summary>
    void RevertToVersion(string filePath, FileVersion selectedVersion);
}
