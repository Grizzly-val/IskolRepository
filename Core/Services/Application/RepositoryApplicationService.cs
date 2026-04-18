using IskolRepository.Core.Interfaces.Application;
using IskolRepository.Core.Interfaces.Domain;
using IskolRepository.Models;

namespace IskolRepository.Core.Services.Application;

/// <summary>
/// Application service for repository use cases.
/// </summary>
public class RepositoryApplicationService : IRepositoryApplicationService
{
    private readonly IRepositoryDomainService _repositoryDomainService;

    public RepositoryApplicationService(IRepositoryDomainService repositoryDomainService)
    {
        _repositoryDomainService = repositoryDomainService ?? throw new ArgumentNullException(nameof(repositoryDomainService));
    }

    public string CreateRepository(string subjectPath, string repositoryName, DateTime deadline)
    {
        if (string.IsNullOrWhiteSpace(subjectPath))
            throw new ArgumentException("Subject path cannot be empty.", nameof(subjectPath));

        if (string.IsNullOrWhiteSpace(repositoryName))
            throw new ArgumentException("Repository name cannot be empty.", nameof(repositoryName));

        try
        {
            return _repositoryDomainService.CreateRepository(subjectPath, repositoryName, deadline);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Unable to create repository: {ex.Message}", ex);
        }
    }

    public void UpdateRepositoryMetadata(string repositoryPath, DateTime deadline, string status)
    {
        if (string.IsNullOrWhiteSpace(repositoryPath))
            throw new ArgumentException("Repository path cannot be empty.", nameof(repositoryPath));

        if (string.IsNullOrWhiteSpace(status))
            throw new ArgumentException("Status cannot be empty.", nameof(status));

        try
        {
            _repositoryDomainService.UpdateRepositoryMetadata(repositoryPath, deadline, status);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Unable to update repository metadata: {ex.Message}", ex);
        }
    }

    public RepoMetadata EnsureMetadata(string repositoryPath)
    {
        if (string.IsNullOrWhiteSpace(repositoryPath))
            throw new ArgumentException("Repository path cannot be empty.", nameof(repositoryPath));

        return _repositoryDomainService.EnsureMetadata(repositoryPath);
    }

    public string? FindRepositoryRoot(string startPath)
    {
        if (string.IsNullOrWhiteSpace(startPath))
            return null;

        return _repositoryDomainService.FindRepositoryRoot(startPath);
    }
}
