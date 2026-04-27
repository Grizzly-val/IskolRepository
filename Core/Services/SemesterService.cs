using IskolRepository.Core.Interfaces;
using IskolRepository.Core.Interfaces.Infrastructure;

namespace IskolRepository.Core.Services;

public class SemesterService : ISemesterService
{
    private const string SemesterMarkerFileName = ".semester.json";

    private readonly IFileSystemHelper _fileSystemHelper;
    private readonly IPathProvider _pathProvider;

    public SemesterService(IFileSystemHelper fileSystemHelper, IPathProvider pathProvider)
    {
        _fileSystemHelper = fileSystemHelper ?? throw new ArgumentNullException(nameof(fileSystemHelper));
        _pathProvider = pathProvider ?? throw new ArgumentNullException(nameof(pathProvider));
    }

    public string OpenSemester(string selectedPath)
    {
        if (string.IsNullOrWhiteSpace(selectedPath))
            throw new ArgumentException("Selected path cannot be empty.", nameof(selectedPath));

        if (!IsSemesterFolder(selectedPath))
            throw new InvalidOperationException("The selected folder is not a valid semester folder.");

        return selectedPath;
    }

    public string CreateSemester(string parentPath, string semesterName)
    {
        if (string.IsNullOrWhiteSpace(parentPath))
            throw new ArgumentException("Parent path cannot be empty.", nameof(parentPath));

        if (string.IsNullOrWhiteSpace(semesterName))
            throw new ArgumentException("Semester name cannot be empty.", nameof(semesterName));

        var targetPath = _pathProvider.CombinePaths(parentPath, semesterName.Trim());

        if (_fileSystemHelper.DirectoryExists(targetPath) && !_fileSystemHelper.IsDirectoryEmpty(targetPath))
            throw new InvalidOperationException("The target semester folder must be empty before use.");

        try
        {
            _fileSystemHelper.CreateDirectory(targetPath);
            CreateSemesterMarker(targetPath);
            return targetPath;
        }
        catch (InvalidOperationException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Unable to create the semester folder.", ex);
        }
    }

    public void CreateSemesterMarker(string semesterPath)
    {
        if (string.IsNullOrWhiteSpace(semesterPath))
            throw new ArgumentException("Semester path cannot be empty.", nameof(semesterPath));

        try
        {
            var markerPath = _pathProvider.CombinePaths(semesterPath, SemesterMarkerFileName);
            _fileSystemHelper.WriteAllText(markerPath, "{}");

            var fileInfo = new FileInfo(markerPath);
            fileInfo.Attributes |= FileAttributes.Hidden;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Unable to create semester marker file.", ex);
        }
    }

    private bool IsSemesterFolder(string path)
    {
        if (string.IsNullOrWhiteSpace(path) || !_fileSystemHelper.DirectoryExists(path))
            return false;

        var markerPath = _pathProvider.CombinePaths(path, SemesterMarkerFileName);
        return _fileSystemHelper.FileExists(markerPath);
    }
}
