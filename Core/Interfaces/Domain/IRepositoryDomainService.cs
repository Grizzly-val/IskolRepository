using IskolRepository.Models;

namespace IskolRepository.Core.Interfaces.Domain;

/// <summary>
/// Domain service for repository operations.
/// Handles all repository-related business logic.
/// </summary>
public interface IRepositoryDomainService
{
    /// <summary>
    /// Ensures metadata exists for a repository, creating default if necessary.
    /// </summary>
    RepoMetadata EnsureMetadata(string repositoryPath);

    /// <summary>
    /// Saves metadata for a repository.
    /// </summary>
    void SaveMetadata(string repositoryPath, RepoMetadata metadata);

    /// <summary>
    /// Finds the repository root folder by walking up the directory tree.
    /// </summary>
    string? FindRepositoryRoot(string startPath);

    /// <summary>
    /// Checks if a path is a valid repository folder.
    /// </summary>
    bool IsRepositoryPath(string path);

    /// <summary>
    /// Creates a new repository with initial metadata.
    /// </summary>
    /// <returns>Path to the created repository</returns>
    string CreateRepository(string subjectPath, string repositoryName, DateTime deadline);

    /// <summary>
    /// Updates repository metadata.
    /// </summary>
    void UpdateRepositoryMetadata(string repositoryPath, DateTime deadline, string status);
}
