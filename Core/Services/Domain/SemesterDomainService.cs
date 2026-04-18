using IskolRepository.Core.Interfaces.Domain;
using IskolRepository.Core.Interfaces.Infrastructure;

namespace IskolRepository.Core.Services.Domain;

/// <summary>
/// Implementation of ISemesterDomainService.
/// </summary>
public class SemesterDomainService : ISemesterDomainService
{
    private const string SemesterMarkerFileName = ".semester.json";
    private readonly IFileSystemHelper _fileSystemHelper;
    private readonly IPathProvider _pathProvider;

    public SemesterDomainService(
        IFileSystemHelper fileSystemHelper,
        IPathProvider pathProvider)
    {
        _fileSystemHelper = fileSystemHelper ?? throw new ArgumentNullException(nameof(fileSystemHelper));
        _pathProvider = pathProvider ?? throw new ArgumentNullException(nameof(pathProvider));
    }

    public bool IsSemesterFolder(string path)
    {
        if (string.IsNullOrWhiteSpace(path) || !_fileSystemHelper.DirectoryExists(path))
            return false;

        var markerPath = _pathProvider.CombinePaths(path, SemesterMarkerFileName);
        return _fileSystemHelper.FileExists(markerPath);
    }

    public void CreateSemesterMarker(string semesterPath)
    {
        try
        {
            var markerPath = _pathProvider.CombinePaths(semesterPath, SemesterMarkerFileName);
            _fileSystemHelper.WriteAllText(markerPath, "{}");

            var fileInfo = new System.IO.FileInfo(markerPath);
            fileInfo.Attributes |= System.IO.FileAttributes.Hidden;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Unable to create semester marker file.", ex);
        }
    }

    public bool ValidateAndCreateSemester(string parentPath, string semesterName, out string? error)
    {
        error = null;

        if (string.IsNullOrWhiteSpace(semesterName))
        {
            error = "Semester name cannot be empty.";
            return false;
        }

        var targetPath = _pathProvider.CombinePaths(parentPath, semesterName.Trim());

        if (_fileSystemHelper.DirectoryExists(targetPath) && 
            !_fileSystemHelper.IsDirectoryEmpty(targetPath))
        {
            error = "The target semester folder must be empty before use.";
            return false;
        }

        try
        {
            _fileSystemHelper.CreateDirectory(targetPath);
            CreateSemesterMarker(targetPath);
            return true;
        }
        catch (Exception ex)
        {
            error = $"Unable to create the semester folder: {ex.Message}";
            return false;
        }
    }
}
