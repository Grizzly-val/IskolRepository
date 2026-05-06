using System.Windows.Forms;
using IskolRepository.Core.Interfaces;
using IskolRepository.Core.Interfaces.Infrastructure;

namespace IskolRepository.Core.Services;

public class FileService : IFileService
{
    private readonly IconProvider _iconProvider = new();
    private readonly IFileSystemHelper _fileSystemHelper;
    private readonly IPathProvider _pathProvider;
    private readonly IValidationHelper _validationHelper;
    private readonly IFileIdentityManager _identityManager;
    private readonly IRepositoryService _repositoryService;

    public FileService(
        IFileSystemHelper fileSystemHelper,
        IPathProvider pathProvider,
        IValidationHelper validationHelper,
        IFileIdentityManager identityManager,
        IRepositoryService repositoryService)
    {
        _fileSystemHelper = fileSystemHelper ?? throw new ArgumentNullException(nameof(fileSystemHelper));
        _pathProvider = pathProvider ?? throw new ArgumentNullException(nameof(pathProvider));
        _validationHelper = validationHelper ?? throw new ArgumentNullException(nameof(validationHelper));
        _identityManager = identityManager ?? throw new ArgumentNullException(nameof(identityManager));
        _repositoryService = repositoryService ?? throw new ArgumentNullException(nameof(repositoryService));
    }
    
    public void LoadFiles(string repositoryRootPath, string browsePath, ListView filesListView, string semesterMarkerFileName)
    {
        if (string.IsNullOrWhiteSpace(repositoryRootPath))
            throw new ArgumentException("Repository root path cannot be empty.", nameof(repositoryRootPath));

        if (string.IsNullOrWhiteSpace(browsePath))
            throw new ArgumentException("Browse path cannot be empty.", nameof(browsePath));

        if (filesListView is null)
            throw new ArgumentNullException(nameof(filesListView));

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
                if (string.Equals(directoryName, RepositoryService.MetadataFolderName, StringComparison.OrdinalIgnoreCase)
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
        catch (InvalidOperationException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Unable to load files from: {browsePath}", ex);
        }
    }

    public void CreateFolder(string parentPath, string name, string folderType)
    {
        if (string.IsNullOrWhiteSpace(parentPath))
            throw new ArgumentException("Parent path cannot be empty.", nameof(parentPath));

        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException($"{folderType} name cannot be empty.", nameof(name));

        var fullPath = _pathProvider.CombinePaths(parentPath, name.Trim());
        if (_fileSystemHelper.DirectoryExists(fullPath))
            throw new InvalidOperationException($"A {folderType} with that name already exists in the selected location.");

        try
        {
            _fileSystemHelper.CreateDirectory(fullPath);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Unable to create the {folderType}.", ex);
        }
    }

    public string CreateFile(string repositoryPath, string fileName, string extension)
    {
        if (string.IsNullOrWhiteSpace(repositoryPath))
            throw new ArgumentException("Repository path cannot be empty.", nameof(repositoryPath));

        if (string.IsNullOrWhiteSpace(fileName))
            throw new ArgumentException("File name cannot be empty.", nameof(fileName));

        if (string.IsNullOrWhiteSpace(extension))
            throw new ArgumentException("Extension cannot be empty.", nameof(extension));

        var result = _fileSystemHelper.CreateRepositoryFile(repositoryPath, fileName.Trim(), extension);
        if (string.IsNullOrWhiteSpace(result))
            throw new InvalidOperationException("Unable to create file.");

        // Register the new file in the identity manifest
        try
        {
            var fileId = _identityManager.RegisterFile(repositoryPath, result);
        }
        catch (Exception ex)
        {
            // Log the error but don't fail the file creation
            // In production, consider a logging mechanism
            System.Diagnostics.Debug.WriteLine($"Failed to register file identity: {ex.Message}");
        }

        return result;
    }

    public void OpenFile(string filePath, Action<string>? onFileExited = null)
    {
        if (string.IsNullOrWhiteSpace(filePath))
            throw new ArgumentException("File path cannot be empty.", nameof(filePath));

        if (!_fileSystemHelper.FileExists(filePath))
            throw new InvalidOperationException("The selected file could not be found.");

        try
        {
            var process = System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(filePath)
            {
                UseShellExecute = true
            });

            if (process is not null && onFileExited is not null)
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
}
