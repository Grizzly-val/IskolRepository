namespace IskolRepository.Core.Interfaces.Application;

/// <summary>
/// Application service for semester use cases.
/// Orchestrates domain services for high-level semester operations.
/// </summary>
public interface ISemesterApplicationService
{
    /// <summary>
    /// Opens an existing semester.
    /// </summary>
    /// <returns>Semester path if successful, null otherwise</returns>
    string? OpenSemester(string selectedPath);

    /// <summary>
    /// Creates and activates a new semester.
    /// </summary>
    /// <returns>Semester path if successful</returns>
    /// <exception cref="InvalidOperationException">Thrown if creation fails</exception>
    string CreateAndActivateSemester(string parentPath, string semesterName);

    /// <summary>
    /// Creates a semester marker file in the given path.
    /// </summary>
    void CreateSemesterMarker(string semesterPath);
}
