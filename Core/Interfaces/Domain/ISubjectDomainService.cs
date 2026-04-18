using System.Windows.Forms;

namespace IskolRepository.Core.Interfaces.Domain;

/// <summary>
/// Domain service for subject operations.
/// </summary>
public interface ISubjectDomainService
{
    /// <summary>
    /// Creates a new subject folder.
    /// </summary>
    /// <returns>True if successful, error message in out parameter</returns>
    bool CreateSubject(string semesterPath, string subjectName, out string? error);

    /// <summary>
    /// Gets all subjects for a semester.
    /// </summary>
    IEnumerable<string> GetSubjectsForSemester(string semesterPath);

    /// <summary>
    /// Loads subjects as cards in a panel.
    /// </summary>
    void LoadSubjectsPanel(string? semesterPath, FlowLayoutPanel panel, Func<string, Control> cardFactory, Action? onEmpty = null);

    /// <summary>
    /// Loads subjects UI in a generic panel (application service wrapper).
    /// </summary>
    void LoadSubjectsUI(string semesterPath, Panel subjectCardsPanel, Func<string, Control> createSubjectCard, Action onEmpty);
}
