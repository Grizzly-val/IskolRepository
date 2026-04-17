using System.Windows.Forms;

namespace IskolRepository.Core;

/// <summary>
/// Manages semester-related operations including folder validation and initialization.
/// </summary>
public static class SemesterManager
{
    private const string SemesterMarkerFileName = ".semester.json";

    public static bool IsSemesterFolder(string path)
    {
        if (string.IsNullOrWhiteSpace(path) || !Directory.Exists(path))
        {
            return false;
        }

        var markerPath = Path.Combine(path, SemesterMarkerFileName);
        return File.Exists(markerPath);
    }

    public static void CreateSemesterMarker(string semesterPath)
    {
        try
        {
            var markerPath = Path.Combine(semesterPath, SemesterMarkerFileName);
            File.WriteAllText(markerPath, "{}");

            // Mark file as hidden
            var fileInfo = new System.IO.FileInfo(markerPath);
            fileInfo.Attributes |= System.IO.FileAttributes.Hidden;
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"Unable to create semester marker file.\n\n{ex.Message}",
                "Semester Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }
}
