using System.Windows.Forms;
using IskolRepository.Core.Interfaces;
using IskolRepository.Core.Interfaces.Infrastructure;

namespace IskolRepository.Core.Services;

public class SubjectService : ISubjectService
{
    private readonly IFileSystemHelper _fileSystemHelper;
    private readonly IPathProvider _pathProvider;

    public SubjectService(IFileSystemHelper fileSystemHelper, IPathProvider pathProvider)
    {
        _fileSystemHelper = fileSystemHelper ?? throw new ArgumentNullException(nameof(fileSystemHelper));
        _pathProvider = pathProvider ?? throw new ArgumentNullException(nameof(pathProvider));
    }

    public void CreateSubject(string semesterPath, string subjectName)
    {
        if (string.IsNullOrWhiteSpace(semesterPath))
            throw new ArgumentException("Semester path cannot be empty.", nameof(semesterPath));

        if (string.IsNullOrWhiteSpace(subjectName))
            throw new ArgumentException("Subject name cannot be empty.", nameof(subjectName));

        var subjectPath = _pathProvider.CombinePaths(semesterPath, subjectName.Trim());

        if (_fileSystemHelper.DirectoryExists(subjectPath))
            throw new InvalidOperationException("A subject with that name already exists.");

        try
        {
            _fileSystemHelper.CreateDirectory(subjectPath);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Unable to create the subject.", ex);
        }
    }

    public IEnumerable<string> GetSubjectsForSemester(string semesterPath)
    {
        if (string.IsNullOrWhiteSpace(semesterPath))
            throw new ArgumentException("Semester path cannot be empty.", nameof(semesterPath));

        if (!_fileSystemHelper.DirectoryExists(semesterPath))
            return Enumerable.Empty<string>();

        try
        {
            return _fileSystemHelper.EnumerateDirectories(semesterPath)
                .OrderBy(d => _pathProvider.GetFileName(d), StringComparer.OrdinalIgnoreCase);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Unable to load subjects.", ex);
        }
    }

    public void LoadSubjectsUI(string semesterPath, Panel subjectCardsPanel, Func<string, Control> createSubjectCard, Action onEmpty)
    {
        if (string.IsNullOrWhiteSpace(semesterPath))
            throw new ArgumentException("Semester path cannot be empty.", nameof(semesterPath));

        if (subjectCardsPanel is null)
            throw new ArgumentNullException(nameof(subjectCardsPanel));

        if (createSubjectCard is null)
            throw new ArgumentNullException(nameof(createSubjectCard));

        if (onEmpty is null)
            throw new ArgumentNullException(nameof(onEmpty));

        subjectCardsPanel.SuspendLayout();
        subjectCardsPanel.Controls.Clear();

        try
        {
            if (!_fileSystemHelper.DirectoryExists(semesterPath))
            {
                onEmpty();
                return;
            }

            var subjects = GetSubjectsForSemester(semesterPath).ToList();
            if (subjects.Count == 0)
            {
                onEmpty();
                return;
            }

            foreach (var subjectPath in subjects)
            {
                subjectCardsPanel.Controls.Add(createSubjectCard(subjectPath));
            }
        }
        catch (Exception ex) when (ex is not ArgumentException and not InvalidOperationException)
        {
            throw new InvalidOperationException("Unable to load subjects UI.", ex);
        }
        finally
        {
            subjectCardsPanel.ResumeLayout();
        }
    }
}
