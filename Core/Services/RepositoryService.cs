using System.Text.Json;
using IskolRepository.Core.Interfaces;
using IskolRepository.Core.Interfaces.Infrastructure;
using IskolRepository.Models;

namespace IskolRepository.Core.Services;

public class RepositoryService : IRepositoryService
{
    public const string MetadataFolderName = ".metadata";
    public const string MetadataFileName = "metadata.json";

    private readonly IFileSystemHelper _fileSystemHelper;
    private readonly IValidationHelper _validationHelper;
    private readonly IPathProvider _pathProvider;
    private readonly JsonSerializerOptions _jsonOptions;

    public RepositoryService(
        IFileSystemHelper fileSystemHelper,
        IValidationHelper validationHelper,
        IPathProvider pathProvider,
        JsonSerializerOptions jsonOptions)
    {
        _fileSystemHelper = fileSystemHelper ?? throw new ArgumentNullException(nameof(fileSystemHelper));
        _validationHelper = validationHelper ?? throw new ArgumentNullException(nameof(validationHelper));
        _pathProvider = pathProvider ?? throw new ArgumentNullException(nameof(pathProvider));
        _jsonOptions = jsonOptions ?? throw new ArgumentNullException(nameof(jsonOptions));
    }

    public RepoMetadata EnsureMetadata(string repositoryPath)
    {
        if (string.IsNullOrWhiteSpace(repositoryPath))
            throw new ArgumentException("Repository path cannot be empty.", nameof(repositoryPath));

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

        try
        {
            var json = _fileSystemHelper.ReadAllText(metadataPath);
            var metadataFromFile = JsonSerializer.Deserialize<RepoMetadata>(json, _jsonOptions);

            if (metadataFromFile is null || !_validationHelper.IsValidStatus(metadataFromFile.Status))
                throw new InvalidOperationException("The repository metadata is invalid.");

            return metadataFromFile;
        }
        catch (InvalidOperationException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Unable to load repository metadata.", ex);
        }
    }

    public string? FindRepositoryRoot(string startPath)
    {
        if (string.IsNullOrWhiteSpace(startPath))
            return null;

        var currentPath = startPath;
        while (!string.IsNullOrWhiteSpace(currentPath))
        {
            if (_validationHelper.IsRepositoryFolder(currentPath))
                return currentPath;

            currentPath = _pathProvider.GetDirectoryName(currentPath);
        }

        return null;
    }

    public string CreateRepository(string subjectPath, string repositoryName, DateTime deadline)
    {
        if (string.IsNullOrWhiteSpace(subjectPath))
            throw new ArgumentException("Subject path cannot be empty.", nameof(subjectPath));

        if (string.IsNullOrWhiteSpace(repositoryName))
            throw new ArgumentException("Repository name cannot be empty.", nameof(repositoryName));

        var repositoryPath = _pathProvider.CombinePaths(subjectPath, repositoryName.Trim());

        if (_fileSystemHelper.DirectoryExists(repositoryPath))
            throw new InvalidOperationException("Repository already exists.");

        try
        {
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
        catch (InvalidOperationException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Unable to create repository.", ex);
        }
    }

    public void UpdateRepositoryMetadata(string repositoryPath, DateTime deadline, string status)
    {
        if (string.IsNullOrWhiteSpace(repositoryPath))
            throw new ArgumentException("Repository path cannot be empty.", nameof(repositoryPath));

        if (string.IsNullOrWhiteSpace(status))
            throw new ArgumentException("Status cannot be empty.", nameof(status));

        var metadata = EnsureMetadata(repositoryPath);
        metadata.Deadline = deadline.Date;
        metadata.Status = status;
        SaveMetadata(repositoryPath, metadata);
    }

    private void SaveMetadata(string repositoryPath, RepoMetadata metadata)
    {
        if (!_validationHelper.IsValidStatus(metadata.Status))
            throw new InvalidOperationException("Metadata status is invalid.");

        try
        {
            EnsureMetadataFolder(repositoryPath);
            var metadataPath = GetMetadataFilePath(repositoryPath);
            var json = JsonSerializer.Serialize(metadata, _jsonOptions);
            _fileSystemHelper.WriteAllText(metadataPath, json);
        }
        catch (InvalidOperationException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Unable to save repository metadata.", ex);
        }
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
