using System.Windows.Forms;
using IskolRepository.Core.Interfaces.Application;
using IskolRepository.Core.Interfaces.Domain;

namespace IskolRepository.Core.Services.Application;

/// <summary>
/// Application service for subject use cases.
/// </summary>
public class SubjectApplicationService : ISubjectApplicationService
{
    private readonly ISubjectDomainService _subjectDomainService;

    public SubjectApplicationService(ISubjectDomainService subjectDomainService)
    {
        _subjectDomainService = subjectDomainService ?? throw new ArgumentNullException(nameof(subjectDomainService));
    }

    public void CreateSubject(string semesterPath, string subjectName)
    {
        if (string.IsNullOrWhiteSpace(semesterPath))
            throw new ArgumentException("Semester path cannot be empty.", nameof(semesterPath));

        if (string.IsNullOrWhiteSpace(subjectName))
            throw new ArgumentException("Subject name cannot be empty.", nameof(subjectName));

        if (!_subjectDomainService.CreateSubject(semesterPath, subjectName, out string? error))
        {
            throw new InvalidOperationException(error ?? "Unable to create subject.");
        }
    }

    public IEnumerable<string> GetSubjectsForSemester(string semesterPath)
    {
        if (string.IsNullOrWhiteSpace(semesterPath))
            return Enumerable.Empty<string>();

        try
        {
            return _subjectDomainService.GetSubjectsForSemester(semesterPath);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Unable to get subjects: {ex.Message}", ex);
        }
    }

    public void LoadSubjectsUI(string semesterPath, Panel subjectCardsPanel, Func<string, Control> createSubjectCard, Action onEmpty)
    {
        if (string.IsNullOrWhiteSpace(semesterPath))
            throw new ArgumentException("Semester path cannot be empty.", nameof(semesterPath));

        if (subjectCardsPanel == null)
            throw new ArgumentNullException(nameof(subjectCardsPanel));

        if (createSubjectCard == null)
            throw new ArgumentNullException(nameof(createSubjectCard));

        if (onEmpty == null)
            throw new ArgumentNullException(nameof(onEmpty));

        try
        {
            _subjectDomainService.LoadSubjectsUI(semesterPath, subjectCardsPanel, createSubjectCard, onEmpty);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Unable to load subjects UI: {ex.Message}", ex);
        }
    }
}
