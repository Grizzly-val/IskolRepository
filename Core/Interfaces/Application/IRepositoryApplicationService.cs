using IskolRepository.Models;

namespace IskolRepository.Core.Interfaces.Application;

/// <summary>
/// Application service for repository use cases.
/// </summary>
public interface IRepositoryApplicationService
{
    /// <summary>
    /// Creates a new repository in a subject.
    /// </summary>
    /// <returns>Repository path if successful</returns>
    /// <exception cref="InvalidOperationException">Thrown if creation fails</exception>
    string CreateRepository(string subjectPath, string repositoryName, DateTime deadline);

    /// <summary>
    /// Updates repository metadata.
    /// </summary>
    void UpdateRepositoryMetadata(string repositoryPath, DateTime deadline, string status);

    /// <summary>
    /// Ensures repository metadata exists and returns it.
    /// </summary>
    RepoMetadata EnsureMetadata(string repositoryPath);

    /// <summary>
    /// Finds the repository root from a given path.
    /// </summary>
    string? FindRepositoryRoot(string startPath);
}
