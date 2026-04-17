using System.Windows.Forms;

namespace IskolRepository.Core;

/// <summary>
/// Manages subject-related operations including loading and opening subjects.
/// </summary>
public static class SubjectManager
{
    public static void LoadSubjects(string? currentSemesterPath, FlowLayoutPanel subjectCardsPanel, Func<string, Control> cardFactory, Action? onEmpty = null)
    {
        subjectCardsPanel.SuspendLayout();
        subjectCardsPanel.Controls.Clear();

        if (string.IsNullOrWhiteSpace(currentSemesterPath) || !Directory.Exists(currentSemesterPath))
        {
            subjectCardsPanel.ResumeLayout();
            return;
        }

        try
        {
            var subjects = Directory.GetDirectories(currentSemesterPath)
                .OrderBy(path => Path.GetFileName(path), StringComparer.OrdinalIgnoreCase)
                .ToList();

            if (subjects.Count == 0)
            {
                onEmpty?.Invoke();
            }
            else
            {
                foreach (var subjectPath in subjects)
                {
                    var card = cardFactory(subjectPath);
                    subjectCardsPanel.Controls.Add(card);
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"Unable to load subjects.\n\n{ex.Message}",
                "Load Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
        finally
        {
            subjectCardsPanel.ResumeLayout();
        }
    }
}
