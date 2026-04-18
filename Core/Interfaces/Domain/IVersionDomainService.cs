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
    void LoadVersionHistory(string? filePath, ListBox versionsListBox, Label captionLabel);

    /// <summary>
    /// Prompts user and saves a new version.
    /// </summary>
    void PromptAndSaveVersion(string filePath, string? selectedFilePath, ListBox versionsListBox);

    /// <summary>
    /// Reverts a file to a previous version.
    /// </summary>
    void RevertToVersion(string filePath, FileVersion selectedVersion);
}
