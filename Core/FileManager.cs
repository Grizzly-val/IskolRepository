using System.Windows.Forms;

namespace IskolRepository.Core;

/// <summary>
/// Manages file operations including creation, loading, and opening files.
/// </summary>
public static class FileManager
{
    public static void LoadFiles(string repositoryPath, ListView filesListView, string semesterMarkerFileName)
    {
        filesListView.Items.Clear();

        try
        {
            foreach (var filePath in Directory.GetFiles(repositoryPath)
                         .OrderBy(path => Path.GetFileName(path), StringComparer.OrdinalIgnoreCase))
            {
                if (ValidationHelper.IsSystemManagedFile(filePath, semesterMarkerFileName))
                {
                    continue;
                }

                var item = new ListViewItem(Path.GetFileNameWithoutExtension(filePath));
                item.SubItems.Add(Path.GetExtension(filePath));
                item.Tag = filePath;
                filesListView.Items.Add(item);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"Unable to load files from:\n{repositoryPath}\n\n{ex.Message}",
                "Load Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    public static void CreateFolder(string parentPath, string name, string folderType)
    {
        if (!FileSystemHelper.IsValidName(name))
        {
            MessageBox.Show(
                $"Please enter a valid {folderType} name.",
                "Invalid Name",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            return;
        }

        var fullPath = Path.Combine(parentPath, name);
        if (Directory.Exists(fullPath))
        {
            MessageBox.Show(
                $"A {folderType} with that name already exists in the selected location.",
                "Duplicate Folder",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            return;
        }

        try
        {
            FileSystemHelper.CreateDirectory(fullPath);
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"Unable to create the {folderType}.\n\n{ex.Message}",
                "Create Folder Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    public static void OpenFile(string filePath)
    {
        if (!File.Exists(filePath))
        {
            MessageBox.Show(
                "The selected file could not be found.",
                "File Missing",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            return;
        }

        try
        {
            var process = System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(filePath)
            {
                UseShellExecute = true
            });

            if (process is not null)
            {
                process.EnableRaisingEvents = true;
                process.Exited += (_, _) =>
                {
                    // Note: Caller should handle reopening file editor and prompting for version save
                };
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"Unable to open the selected file.\n\n{ex.Message}",
                "Open File Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }
}
