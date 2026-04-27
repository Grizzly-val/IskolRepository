using System.Windows.Forms;

namespace IskolRepository.Core.Interfaces;

public interface IFileService
{
    void LoadFiles(string repositoryRootPath, string browsePath, ListView filesListView, string semesterMarkerFileName);

    void CreateFolder(string parentPath, string name, string folderType);

    string CreateFile(string repositoryPath, string fileName, string extension);

    void OpenFile(string filePath, Action<string>? onFileExited = null);
}
