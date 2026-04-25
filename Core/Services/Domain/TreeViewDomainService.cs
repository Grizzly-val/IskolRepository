using System.Windows.Forms;
using IskolRepository.Core.Interfaces.Domain;
using IskolRepository.Core.Interfaces.Infrastructure;

namespace IskolRepository.Core.Services.Domain;

/// <summary>
/// Implementation of ITreeViewDomainService.
/// </summary>
public class TreeViewDomainService : ITreeViewDomainService
{
    private readonly IFileSystemHelper _fileSystemHelper;
    private readonly IPathProvider _pathProvider;
    private readonly IValidationHelper _validationHelper;

    public TreeViewDomainService(
        IFileSystemHelper fileSystemHelper,
        IPathProvider pathProvider,
        IValidationHelper validationHelper)
    {
        _fileSystemHelper = fileSystemHelper ?? throw new ArgumentNullException(nameof(fileSystemHelper));
        _pathProvider = pathProvider ?? throw new ArgumentNullException(nameof(pathProvider));
        _validationHelper = validationHelper ?? throw new ArgumentNullException(nameof(validationHelper));
    }

    public void LoadSemesterTree(string semesterPath, TreeView repositoryTreeView, string semesterMarkerFileName)
    {
        if (string.IsNullOrWhiteSpace(semesterPath) || !_fileSystemHelper.DirectoryExists(semesterPath))
        {
            repositoryTreeView.Nodes.Clear();
            return;
        }

        repositoryTreeView.BeginUpdate();
        repositoryTreeView.Nodes.Clear();

        var rootNode = new TreeNode(_pathProvider.GetFileName(semesterPath))
        {
            Tag = new NodeData(semesterPath, NodeType.Semester),
            ImageKey = GetIconKey(NodeType.Semester),
            SelectedImageKey = GetIconKey(NodeType.Semester)
        };

        repositoryTreeView.Nodes.Add(rootNode);
        rootNode.Expand();
        LoadChildNodes(rootNode, semesterMarkerFileName);

        repositoryTreeView.EndUpdate();

        // Apply validation coloring
        foreach (TreeNode root in repositoryTreeView.Nodes)
        {
            _validationHelper.ApplyNodeValidationColors(root);
        }
    }

    public void LoadSubjectTree(string? currentSubjectPath, string? selectPath, TreeView repositoryTreeView, string semesterMarkerFileName)
    {
        if (string.IsNullOrWhiteSpace(currentSubjectPath) || !_fileSystemHelper.DirectoryExists(currentSubjectPath))
        {
            repositoryTreeView.Nodes.Clear();
            return;
        }

        repositoryTreeView.BeginUpdate();
        repositoryTreeView.Nodes.Clear();

        var rootNode = new TreeNode(_pathProvider.GetFileName(currentSubjectPath))
        {
            Tag = new NodeData(currentSubjectPath, NodeType.Subject),
            ImageKey = GetIconKey(NodeType.Subject),
            SelectedImageKey = GetIconKey(NodeType.Subject)
        };

        repositoryTreeView.Nodes.Add(rootNode);
        rootNode.Expand();
        LoadChildNodes(rootNode, semesterMarkerFileName);

        var nodeToSelect = !string.IsNullOrWhiteSpace(selectPath)
            ? FindNodeByPath(repositoryTreeView.Nodes, selectPath)
            : rootNode;

        if (nodeToSelect is not null)
        {
            repositoryTreeView.SelectedNode = nodeToSelect;
            EnsureParentChainExpanded(nodeToSelect);
        }

        repositoryTreeView.EndUpdate();

        // Apply validation coloring
        foreach (TreeNode root in repositoryTreeView.Nodes)
        {
            _validationHelper.ApplyNodeValidationColors(root);
        }
    }

    public void LoadChildNodes(TreeNode parentNode, string semesterMarkerFileName)
    {
        if (parentNode?.Tag is not NodeData parentData)
            return;

        // Prevent duplicate loading
        if (parentNode.Nodes.Count > 0)
            return;

        try
        {
            var parentPath = parentData.Path;
            if (!_fileSystemHelper.DirectoryExists(parentPath))
                return;

            var childNodeType = GetChildNodeType(parentData.NodeType);

            // Load directories
            foreach (var directory in _fileSystemHelper.EnumerateDirectories(parentPath)
                .OrderBy(d => _pathProvider.GetFileName(d), StringComparer.OrdinalIgnoreCase))
            {
                var directoryName = _pathProvider.GetFileName(directory);
                if (string.Equals(directoryName, RepositoryDomainService.MetadataFolderName, StringComparison.OrdinalIgnoreCase)
                    || string.Equals(directoryName, VersionHelper.HistoryFolderName, StringComparison.OrdinalIgnoreCase))
                    continue;

                var childNode = new TreeNode(directoryName)
                {
                    Tag = new NodeData(directory, childNodeType),
                    ImageKey = GetIconKey(childNodeType),
                    SelectedImageKey = GetIconKey(childNodeType)
                };

                parentNode.Nodes.Add(childNode);
            }

            // Load files
            foreach (var filePath in _fileSystemHelper.EnumerateFiles(parentPath)
                .OrderBy(f => _pathProvider.GetFileName(f), StringComparer.OrdinalIgnoreCase))
            {
                if (_validationHelper.IsSystemManagedFile(filePath, semesterMarkerFileName))
                    continue;

                // Check if file is valid (inside a repository, not directly under subject)
                var isValidFile = IsValidFileNode(parentNode, parentData);

                parentNode.Nodes.Add(new TreeNode(_pathProvider.GetFileName(filePath))
                {
                    Tag = new NodeData(filePath, NodeType.File, isValidFile),
                    ImageKey = GetIconKey(NodeType.File),
                    SelectedImageKey = GetIconKey(NodeType.File)
                });
            }
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Unable to load the selected item.", ex);
        }
    }

    public TreeNode? FindNodeByPath(TreeNodeCollection nodes, string path)
    {
        foreach (TreeNode node in nodes)
        {
            if (node.Tag is NodeData nodeData
                && string.Equals(nodeData.Path, path, StringComparison.OrdinalIgnoreCase))
            {
                return node;
            }

            var childMatch = FindNodeByPath(node.Nodes, path);
            if (childMatch is not null)
            {
                return childMatch;
            }
        }

        return null;
    }

    public void EnsureParentChainExpanded(TreeNode node)
    {
        var current = node.Parent;
        while (current is not null)
        {
            current.Expand();
            current = current.Parent;
        }
    }

    private static NodeType GetChildNodeType(NodeType parentType)
    {
        return parentType switch
        {
            NodeType.Semester => NodeType.Subject,
            NodeType.Subject => NodeType.Repository,
            NodeType.Repository => NodeType.SubRepository,
            NodeType.SubRepository => NodeType.SubRepository,
            _ => NodeType.File
        };
    }

    private static string GetIconKey(NodeType nodeType)
    {
        return nodeType switch
        {
            NodeType.Semester => IconProvider.SemesterIconKey,
            NodeType.Subject => IconProvider.SubjectIconKey,
            NodeType.Repository => IconProvider.RepositoryIconKey,
            NodeType.SubRepository => IconProvider.SubRepositoryIconKey,
            NodeType.File => IconProvider.FileIconKey,
            _ => IconProvider.FileIconKey
        };
    }

    /// <summary>
    /// Determines if a file node is valid.
    /// A file is invalid if its parent is a Subject node (file directly under subject, outside any repository).
    /// </summary>
    private static bool IsValidFileNode(TreeNode parentNode, NodeData parentData)
    {
        // Files are valid if parent is not a Subject (files must be inside repositories or their subdirectories)
        return parentData.NodeType != NodeType.Subject;
    }
}
