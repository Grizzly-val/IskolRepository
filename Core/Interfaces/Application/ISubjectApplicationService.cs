using System.Windows.Forms;

namespace IskolRepository.Core.Interfaces.Application;

/// <summary>
/// Application service for subject use cases.
/// </summary>
public interface ISubjectApplicationService
{
    /// <summary>
    /// Creates a new subject in a semester.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown if creation fails</exception>
    void CreateSubject(string semesterPath, string subjectName);

    /// <summary>
    /// Gets all subjects for a semester.
    /// </summary>
    IEnumerable<string> GetSubjectsForSemester(string semesterPath);

    /// <summary>
    /// Loads subjects for a semester and populates the UI panel.
    /// </summary>
    void LoadSubjectsUI(string semesterPath, Panel subjectCardsPanel, Func<string, Control> createSubjectCard, Action onEmpty);
}
