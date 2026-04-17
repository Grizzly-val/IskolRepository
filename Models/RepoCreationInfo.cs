namespace IskolRepository.Models;

public sealed class RepoCreationInfo
{
    public string RepositoryName { get; init; } = string.Empty;

    public DateTime Deadline { get; init; }
}
