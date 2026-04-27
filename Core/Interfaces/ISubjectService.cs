using System.Windows.Forms;

namespace IskolRepository.Core.Interfaces;

public interface ISubjectService
{
    void CreateSubject(string semesterPath, string subjectName);

    IEnumerable<string> GetSubjectsForSemester(string semesterPath);

    void LoadSubjectsUI(string semesterPath, Panel subjectCardsPanel, Func<string, Control> createSubjectCard, Action onEmpty);
}
