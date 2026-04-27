using IskolRepository.Models;

namespace IskolRepository.Core.Interfaces;

public interface IRepositoryService
{
    string CreateRepository(string subjectPath, string repositoryName, DateTime deadline);

    void UpdateRepositoryMetadata(string repositoryPath, DateTime deadline, string status);

    RepoMetadata EnsureMetadata(string repositoryPath);

    string? FindRepositoryRoot(string startPath);
}
