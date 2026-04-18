using System.Windows.Forms;
using IskolRepository.Core.Interfaces.Domain;
using IskolRepository.Core.Interfaces.Infrastructure;

namespace IskolRepository.Core.Services.Domain;

/// <summary>
/// Implementation of ISubjectDomainService.
/// </summary>
public class SubjectDomainService : ISubjectDomainService
{
    private readonly IFileSystemHelper _fileSystemHelper;
    private readonly IPathProvider _pathProvider;

    public SubjectDomainService(
        IFileSystemHelper fileSystemHelper,
        IPathProvider pathProvider)
    {
        _fileSystemHelper = fileSystemHelper ?? throw new ArgumentNullException(nameof(fileSystemHelper));
        _pathProvider = pathProvider ?? throw new ArgumentNullException(nameof(pathProvider));
    }

    public bool CreateSubject(string semesterPath, string subjectName, out string? error)
    {
        error = null;

        if (string.IsNullOrWhiteSpace(subjectName))
        {
            error = "Subject name cannot be empty.";
            return false;
        }

        var subjectPath = _pathProvider.CombinePaths(semesterPath, subjectName.Trim());

        if (_fileSystemHelper.DirectoryExists(subjectPath))
        {
            error = "A subject with that name already exists.";
            return false;
        }

        try
        {
            _fileSystemHelper.CreateDirectory(subjectPath);
            return true;
        }
        catch (Exception ex)
        {
            error = $"Unable to create the subject: {ex.Message}";
            return false;
        }
    }

    public IEnumerable<string> GetSubjectsForSemester(string semesterPath)
    {
        if (!_fileSystemHelper.DirectoryExists(semesterPath))
            return Enumerable.Empty<string>();

        try
        {
            return _fileSystemHelper.EnumerateDirectories(semesterPath)
                .OrderBy(d => _pathProvider.GetFileName(d), StringComparer.OrdinalIgnoreCase);
        }
        catch
        {
            return Enumerable.Empty<string>();
        }
    }

    public void LoadSubjectsPanel(string? semesterPath, FlowLayoutPanel panel, Func<string, Control> cardFactory, Action? onEmpty = null)
    {
        panel.SuspendLayout();
        panel.Controls.Clear();

        if (string.IsNullOrWhiteSpace(semesterPath) || !_fileSystemHelper.DirectoryExists(semesterPath))
        {
            panel.ResumeLayout();
            return;
        }

        try
        {
            var subjects = GetSubjectsForSemester(semesterPath).ToList();

            if (subjects.Count == 0)
            {
                onEmpty?.Invoke();
            }
            else
            {
                foreach (var subjectPath in subjects)
                {
                    var card = cardFactory(subjectPath);
                    panel.Controls.Add(card);
                }
            }
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Unable to load subjects.", ex);
        }
        finally
        {
            panel.ResumeLayout();
        }
    }

    public void LoadSubjectsUI(string semesterPath, Panel subjectCardsPanel, Func<string, Control> createSubjectCard, Action onEmpty)
    {
        // Cast generic Panel to FlowLayoutPanel if needed, or work with the generic Panel
        if (subjectCardsPanel == null)
            throw new ArgumentNullException(nameof(subjectCardsPanel));

        subjectCardsPanel.Controls.Clear();

        if (string.IsNullOrWhiteSpace(semesterPath) || !_fileSystemHelper.DirectoryExists(semesterPath))
        {
            onEmpty?.Invoke();
            return;
        }

        try
        {
            var subjects = GetSubjectsForSemester(semesterPath).ToList();

            if (subjects.Count == 0)
            {
                onEmpty?.Invoke();
            }
            else
            {
                foreach (var subjectPath in subjects)
                {
                    var card = createSubjectCard(subjectPath);
                    subjectCardsPanel.Controls.Add(card);
                }
            }
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Unable to load subjects UI.", ex);
        }
    }
}
