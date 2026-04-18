namespace IskolRepository.Core.Interfaces.Domain;

/// <summary>
/// Domain service for semester operations.
/// </summary>
public interface ISemesterDomainService
{
    /// <summary>
    /// Checks if a path is a valid semester folder.
    /// </summary>
    bool IsSemesterFolder(string path);

    /// <summary>
    /// Creates a semester marker file.
    /// </summary>
    void CreateSemesterMarker(string semesterPath);

    /// <summary>
    /// Validates and creates a new semester.
    /// </summary>
    /// <returns>True if successful, error message in out parameter</returns>
    bool ValidateAndCreateSemester(string parentPath, string semesterName, out string? error);
}
