using System.Windows.Forms;
using IskolRepository.Core.Interfaces.Application;
using IskolRepository.Core.Interfaces.Domain;
using IskolRepository.Models;

namespace IskolRepository.Core.Services.Application;

/// <summary>
/// Application service for file use cases.
/// </summary>
public class FileApplicationService : IFileApplicationService
{
    private readonly IFileDomainService _fileDomainService;
    private readonly IVersionDomainService _versionDomainService;

    public FileApplicationService(
        IFileDomainService fileDomainService,
        IVersionDomainService versionDomainService)
    {
        _fileDomainService = fileDomainService ?? throw new ArgumentNullException(nameof(fileDomainService));
        _versionDomainService = versionDomainService ?? throw new ArgumentNullException(nameof(versionDomainService));
    }

    public string CreateFile(string repositoryPath, string fileName, string extension)
    {


        var result = _fileDomainService.CreateRepositoryFile(repositoryPath, fileName, extension, out string? error);
        if (result == null)
            throw new InvalidOperationException(error ?? "Unable to create file.");

        return result;
    }

    public void OpenAndTrackFile(string filePath, Action<string> onFileClosed)
    {
        if (string.IsNullOrWhiteSpace(filePath))
            throw new ArgumentException("File path cannot be empty.", nameof(filePath));

        if (onFileClosed == null)
            throw new ArgumentNullException(nameof(onFileClosed));

        try
        {
            _fileDomainService.OpenFile(filePath, onFileClosed);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Unable to open file: {ex.Message}", ex);
        }
    }

    public void RevertFileVersion(string filePath, FileVersion selectedVersion)
    {
        if (string.IsNullOrWhiteSpace(filePath))
            throw new ArgumentException("File path cannot be empty.", nameof(filePath));

        if (selectedVersion == null)
            throw new ArgumentNullException(nameof(selectedVersion));

        try
        {
            _versionDomainService.RevertToVersion(filePath, selectedVersion);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Unable to revert file version: {ex.Message}", ex);
        }
    }

    public void LoadFiles(string repositoryPath, ListView filesListView, string semesterMarkerFileName)
    {
        if (string.IsNullOrWhiteSpace(repositoryPath))
            throw new ArgumentException("Repository path cannot be empty.", nameof(repositoryPath));

        if (filesListView == null)
            throw new ArgumentNullException(nameof(filesListView));

        try
        {
            _fileDomainService.LoadFiles(repositoryPath, filesListView, semesterMarkerFileName);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Unable to load files: {ex.Message}", ex);
        }
    }
}
