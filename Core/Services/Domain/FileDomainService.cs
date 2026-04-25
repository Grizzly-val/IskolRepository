using System.Windows.Forms;
using IskolRepository.Core.Interfaces.Domain;
using IskolRepository.Core.Interfaces.Infrastructure;

namespace IskolRepository.Core.Services.Domain;

/// <summary>
/// Implementation of IFileDomainService.
/// </summary>
public class FileDomainService : IFileDomainService
{
    private readonly IconProvider _iconProvider = new();
    private readonly IFileSystemHelper _fileSystemHelper;
    private readonly IPathProvider _pathProvider;
    private readonly IValidationHelper _validationHelper;

    public FileDomainService(
        IFileSystemHelper fileSystemHelper,
        IPathProvider pathProvider,
        IValidationHelper validationHelper)
    {
        _fileSystemHelper = fileSystemHelper ?? throw new ArgumentNullException(nameof(fileSystemHelper));
        _pathProvider = pathProvider ?? throw new ArgumentNullException(nameof(pathProvider));
        _validationHelper = validationHelper ?? throw new ArgumentNullException(nameof(validationHelper));
    }

    public void LoadFiles(string repositoryRootPath, string browsePath, ListView filesListView, string semesterMarkerFileName)
    {
        LoadFilesInternal(repositoryRootPath, browsePath, filesListView, semesterMarkerFileName);
    }

    private void LoadFilesInternal(string repositoryRootPath, string browsePath, ListView filesListView, string semesterMarkerFileName)
    {
        filesListView.Items.Clear();

        try
        {
            var normalizedRepositoryRoot = Path.GetFullPath(repositoryRootPath);
            var normalizedBrowsePath = Path.GetFullPath(browsePath);

            if (!IsPathInsideRoot(normalizedBrowsePath, normalizedRepositoryRoot))
                throw new InvalidOperationException("Browse path must stay inside the selected repository.");

            var parentPath = _pathProvider.GetDirectoryName(normalizedBrowsePath);
            if (!string.IsNullOrWhiteSpace(parentPath)
                && !string.Equals(normalizedBrowsePath, normalizedRepositoryRoot, StringComparison.OrdinalIgnoreCase)
                && IsPathInsideRoot(Path.GetFullPath(parentPath), normalizedRepositoryRoot))
            {
                var parentItem = new ListViewItem("..");
                parentItem.SubItems.Add("<Parent>");
                parentItem.ImageKey = IconProvider.FolderIconKey;
                parentItem.Tag = new RepositoryBrowseEntry(
                    RepositoryBrowseEntryKind.Parent,
                    Path.GetFullPath(parentPath),
                    "..");
                filesListView.Items.Add(parentItem);
            }

            var directories = _fileSystemHelper.EnumerateDirectories(normalizedBrowsePath)
                .OrderBy(d => _pathProvider.GetFileName(d), StringComparer.OrdinalIgnoreCase);

            foreach (var directoryPath in directories)
            {
                var directoryName = _pathProvider.GetFileName(directoryPath);
                if (string.Equals(directoryName, RepositoryDomainService.MetadataFolderName, StringComparison.OrdinalIgnoreCase)
                    || string.Equals(directoryName, VersionHelper.HistoryFolderName, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                var directoryItem = new ListViewItem(directoryName);
                directoryItem.SubItems.Add("<Folder>");
                directoryItem.ImageKey = _iconProvider.GetIconKey(directoryPath);
                directoryItem.Tag = new RepositoryBrowseEntry(
                    RepositoryBrowseEntryKind.Directory,
                    directoryPath,
                    directoryName);
                filesListView.Items.Add(directoryItem);
            }

            var files = _fileSystemHelper.EnumerateFiles(normalizedBrowsePath)
                .OrderBy(f => _pathProvider.GetFileName(f), StringComparer.OrdinalIgnoreCase);

            foreach (var filePath in files)
            {
                if (_validationHelper.IsSystemManagedFile(filePath, semesterMarkerFileName))
                    continue;

                var item = new ListViewItem(_pathProvider.GetFileNameWithoutExtension(filePath));
                item.SubItems.Add(_pathProvider.GetExtension(filePath));
                item.ImageKey = _iconProvider.GetIconKey(filePath);
                item.Tag = new RepositoryBrowseEntry(
                    RepositoryBrowseEntryKind.File,
                    filePath,
                    _pathProvider.GetFileName(filePath));
                filesListView.Items.Add(item);
            }
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Unable to load files from: {browsePath}", ex);
        }
    }

    private static bool IsPathInsideRoot(string path, string rootPath)
    {
        if (string.Equals(path, rootPath, StringComparison.OrdinalIgnoreCase))
            return true;

        var normalizedRoot = rootPath.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)
            + Path.DirectorySeparatorChar;
        var normalizedPath = path.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)
            + Path.DirectorySeparatorChar;

        return normalizedPath.StartsWith(normalizedRoot, StringComparison.OrdinalIgnoreCase);
    }

    public bool CreateFolder(string parentPath, string name, string folderType, out string? error)
    {
        error = null;

        if (string.IsNullOrWhiteSpace(name))
        {
            error = $"Please enter a valid {folderType} name.";
            return false;
        }

        var fullPath = _pathProvider.CombinePaths(parentPath, name);
        if (_fileSystemHelper.DirectoryExists(fullPath))
        {
            error = $"A {folderType} with that name already exists in the selected location.";
            return false;
        }

        try
        {
            _fileSystemHelper.CreateDirectory(fullPath);
            return true;
        }
        catch (Exception ex)
        {
            error = $"Unable to create the {folderType}: {ex.Message}";
            return false;
        }
    }

    public string? CreateRepositoryFile(string repositoryPath, string fileName, string extension, out string? error)
    {
        error = null;
        // Basic validation can still throw specific exceptions
        if (string.IsNullOrWhiteSpace(repositoryPath))
            throw new ArgumentException("Repository path cannot be empty.", nameof(repositoryPath));

        if (string.IsNullOrWhiteSpace(fileName))
            throw new ArgumentException("File name cannot be empty.", nameof(fileName));

        if (string.IsNullOrWhiteSpace(extension))
            throw new ArgumentException("Extension cannot be empty.", nameof(extension));

        return _fileSystemHelper.CreateRepositoryFile(repositoryPath, fileName, extension);
    }


    public void OpenFile(string filePath, Action<string> onFileExited)
    {
        if (!_fileSystemHelper.FileExists(filePath))
            throw new FileNotFoundException("The selected file could not be found.", filePath);

        try
        {
            var process = System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(filePath)
            {
                UseShellExecute = true
            });

            if (process is not null)
            {
                process.EnableRaisingEvents = true;
                process.Exited += (_, _) => onFileExited(filePath);
            }
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Unable to open the selected file.", ex);
        }
    }
}
