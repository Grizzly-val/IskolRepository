using IskolRepository.Core.Interfaces.Application;
using IskolRepository.Core.Interfaces.Domain;
using IskolRepository.Core.Interfaces.Infrastructure;

namespace IskolRepository.Core.Services.Application;

/// <summary>
/// Application service for semester use cases.
/// Orchestrates domain services for high-level semester operations.
/// </summary>
public class SemesterApplicationService : ISemesterApplicationService
{
    private readonly ISemesterDomainService _semesterDomainService;
    private readonly ITreeViewDomainService _treeViewDomainService;
    private readonly IFileSystemHelper? _fileSystemHelper;

    public SemesterApplicationService(
        ISemesterDomainService semesterDomainService,
        ITreeViewDomainService treeViewDomainService,
        IFileSystemHelper? fileSystemHelper = null)
    {
        _semesterDomainService = semesterDomainService ?? throw new ArgumentNullException(nameof(semesterDomainService));
        _treeViewDomainService = treeViewDomainService ?? throw new ArgumentNullException(nameof(treeViewDomainService));
        _fileSystemHelper = fileSystemHelper;
    }

    public string? OpenSemester(string selectedPath)
    {
        if (string.IsNullOrWhiteSpace(selectedPath))
            return null;

        if (!_semesterDomainService.IsSemesterFolder(selectedPath))
            return null;

        return selectedPath;
    }

    public string CreateAndActivateSemester(string parentPath, string semesterName)
    {
        if (!_semesterDomainService.ValidateAndCreateSemester(parentPath, semesterName, out string? error))
        {
            throw new InvalidOperationException(error ?? "Unable to create semester.");
        }

        var semesterPath = Path.Combine(parentPath, semesterName.Trim());
        return semesterPath;
    }

    public void CreateSemesterMarker(string semesterPath)
    {
        if (string.IsNullOrWhiteSpace(semesterPath))
            throw new ArgumentNullException(nameof(semesterPath));

        _semesterDomainService.CreateSemesterMarker(semesterPath);
    }
}
