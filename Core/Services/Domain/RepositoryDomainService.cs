using System.Text.Json;
using IskolRepository.Core.Interfaces.Domain;
using IskolRepository.Core.Interfaces.Infrastructure;
using IskolRepository.Models;

namespace IskolRepository.Core.Services.Domain;

/// <summary>
/// Implementation of IRepositoryDomainService.
/// </summary>
public class RepositoryDomainService : IRepositoryDomainService
{
    public const string MetadataFolderName = ".metadata";
    public const string MetadataFileName = "metadata.json";

    private readonly IFileSystemHelper _fileSystemHelper;
    private readonly IValidationHelper _validationHelper;
    private readonly IPathProvider _pathProvider;
    private readonly JsonSerializerOptions _jsonOptions;
    private readonly string[] _validStatuses;

    public RepositoryDomainService(
        IFileSystemHelper fileSystemHelper,
        IValidationHelper validationHelper,
        IPathProvider pathProvider,
        JsonSerializerOptions jsonOptions,
        string[] validStatuses)
    {
        _fileSystemHelper = fileSystemHelper ?? throw new ArgumentNullException(nameof(fileSystemHelper));
        _validationHelper = validationHelper ?? throw new ArgumentNullException(nameof(validationHelper));
        _pathProvider = pathProvider ?? throw new ArgumentNullException(nameof(pathProvider));
        _jsonOptions = jsonOptions ?? throw new ArgumentNullException(nameof(jsonOptions));
        _validStatuses = validStatuses ?? throw new ArgumentNullException(nameof(validStatuses));
    }

    public RepoMetadata EnsureMetadata(string repositoryPath)
    {
        var metadataPath = GetMetadataFilePath(repositoryPath);
        EnsureMetadataFolder(repositoryPath);
        
        if (!_fileSystemHelper.FileExists(metadataPath))
        {
            var metadata = new RepoMetadata
            {
                Deadline = DateTime.Today,
                DateAdded = DateTime.Today,
                Status = "in-progress"
            };

            SaveMetadata(repositoryPath, metadata);
            return metadata;
        }

        var json = _fileSystemHelper.ReadAllText(metadataPath);
        var metadataFromFile = System.Text.Json.JsonSerializer.Deserialize<RepoMetadata>(json, _jsonOptions);
        
        if (metadataFromFile is null || 
            !_validationHelper.IsValidStatus(metadataFromFile.Status))
        {
            throw new InvalidOperationException("The repository metadata is invalid.");
        }

        return metadataFromFile;
    }

    public void SaveMetadata(string repositoryPath, RepoMetadata metadata)
    {
        if (!_validationHelper.IsValidStatus(metadata.Status))
            throw new InvalidOperationException("Metadata status is invalid.");

        EnsureMetadataFolder(repositoryPath);
        var metadataPath = GetMetadataFilePath(repositoryPath);
        var json = System.Text.Json.JsonSerializer.Serialize(metadata, _jsonOptions);
        _fileSystemHelper.WriteAllText(metadataPath, json);
    }

    public string? FindRepositoryRoot(string startPath)
    {
        var currentPath = startPath;
        while (!string.IsNullOrWhiteSpace(currentPath))
        {
            if (IsRepositoryPath(currentPath))
                return currentPath;

            currentPath = _pathProvider.GetDirectoryName(currentPath);
        }

        return null;
    }

    public bool IsRepositoryPath(string path)
    {
        return _validationHelper.IsRepositoryFolder(path);
    }

    public string CreateRepository(string subjectPath, string repositoryName, DateTime deadline)
    {
        if (string.IsNullOrWhiteSpace(repositoryName))
            throw new ArgumentException("Repository name cannot be empty.", nameof(repositoryName));

        var repositoryPath = _pathProvider.CombinePaths(subjectPath, repositoryName.Trim());

        if (_fileSystemHelper.DirectoryExists(repositoryPath))
            throw new InvalidOperationException("Repository already exists.");

        _fileSystemHelper.CreateDirectory(repositoryPath);

        var metadata = new RepoMetadata
        {
            Deadline = deadline.Date,
            DateAdded = DateTime.Today,
            Status = "in-progress"
        };

        SaveMetadata(repositoryPath, metadata);
        return repositoryPath;
    }

    public void UpdateRepositoryMetadata(string repositoryPath, DateTime deadline, string status)
    {
        var metadata = EnsureMetadata(repositoryPath);
        metadata.Deadline = deadline;
        metadata.Status = status;
        SaveMetadata(repositoryPath, metadata);
    }

    private string GetMetadataFolderPath(string repositoryPath)
    {
        return _pathProvider.CombinePaths(repositoryPath, MetadataFolderName);
    }

    private string GetMetadataFilePath(string repositoryPath)
    {
        return _pathProvider.CombinePaths(GetMetadataFolderPath(repositoryPath), MetadataFileName);
    }

    private void EnsureMetadataFolder(string repositoryPath)
    {
        var metadataFolderPath = GetMetadataFolderPath(repositoryPath);
        if (!_fileSystemHelper.DirectoryExists(metadataFolderPath))
        {
            _fileSystemHelper.CreateDirectory(metadataFolderPath);
        }

        if (Directory.Exists(metadataFolderPath))
        {
            var dirInfo = new DirectoryInfo(metadataFolderPath);
            dirInfo.Attributes |= FileAttributes.Hidden;
        }
    }
}
