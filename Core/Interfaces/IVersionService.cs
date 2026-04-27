using System.Windows.Forms;
using IskolRepository.Models;

namespace IskolRepository.Core.Interfaces;

public interface IVersionService
{
    void LoadVersionHistory(string? filePath, ListBox versionsListBox, Label captionLabel, Label? noVersionsMessageLabel);

    void SaveVersion(string filePath, string comment);

    bool IsSupportedVersionFileType(string filePath);

    bool CanSaveVersion(string filePath);

    void RevertToVersion(string filePath, FileVersion selectedVersion);
}
