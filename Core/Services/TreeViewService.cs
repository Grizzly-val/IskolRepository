using System.Windows.Forms;
using IskolRepository.Core.Interfaces;
using IskolRepository.Core.Interfaces.Infrastructure;

namespace IskolRepository.Core.Services;

public class TreeViewService : ITreeViewService
{
    private readonly IconProvider _iconProvider = new();
    private readonly IFileSystemHelper _fileSystemHelper;
    private readonly IPathProvider _pathProvider;
    private readonly IValidationHelper _validationHelper;
    private readonly IRepositoryService _repositoryService;

    public TreeViewService(
        IFileSystemHelper fileSystemHelper,
        IPathProvider pathProvider,
        IValidationHelper validationHelper,
        IRepositoryService repositoryService)
    {
        _fileSystemHelper = fileSystemHelper ?? throw new ArgumentNullException(nameof(fileSystemHelper));
        _pathProvider = pathProvider ?? throw new ArgumentNullException(nameof(pathProvider));
        _validationHelper = validationHelper ?? throw new ArgumentNullException(nameof(validationHelper));
        _repositoryService = repositoryService ?? throw new ArgumentNullException(nameof(repositoryService));
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
            ImageKey = _iconProvider.GetIconKey(semesterPath),
            SelectedImageKey = _iconProvider.GetIconKey(semesterPath)
        };

        repositoryTreeView.Nodes.Add(rootNode);
        rootNode.Expand();
        LoadChildNodes(rootNode, semesterMarkerFileName);

        repositoryTreeView.EndUpdate();

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
        var expandedPaths = GetExpandedPaths(repositoryTreeView.Nodes);
        repositoryTreeView.Nodes.Clear();

        var rootNode = new TreeNode(_pathProvider.GetFileName(currentSubjectPath))
        {
            Tag = new NodeData(currentSubjectPath, NodeType.Subject),
            ImageKey = _iconProvider.GetIconKey(currentSubjectPath),
            SelectedImageKey = _iconProvider.GetIconKey(currentSubjectPath)
        };

        repositoryTreeView.Nodes.Add(rootNode);
        rootNode.Expand();
        LoadChildNodes(rootNode, semesterMarkerFileName);

        RestoreExpandedPaths(repositoryTreeView, expandedPaths, semesterMarkerFileName);

        if (!string.IsNullOrWhiteSpace(selectPath))
        {
            ForceLoadPath(repositoryTreeView.Nodes, selectPath, semesterMarkerFileName);
        }

        var nodeToSelect = !string.IsNullOrWhiteSpace(selectPath)
            ? FindNodeByPath(repositoryTreeView.Nodes, selectPath)
            : rootNode;

        if (nodeToSelect is not null)
        {
            repositoryTreeView.SelectedNode = nodeToSelect;
            EnsureParentChainExpanded(nodeToSelect);
        }

        repositoryTreeView.EndUpdate();

        foreach (TreeNode root in repositoryTreeView.Nodes)
        {
            _validationHelper.ApplyNodeValidationColors(root);
        }
    }

    public void LoadChildNodes(TreeNode parentNode, string semesterMarkerFileName)
    {
        if (parentNode?.Tag is not NodeData parentData)
            return;

        if (parentNode.Nodes.Count > 0)
            return;

        try
        {
            var parentPath = parentData.Path;
            if (!_fileSystemHelper.DirectoryExists(parentPath))
                return;

            var childNodeType = GetChildNodeType(parentData.NodeType);

            foreach (var directory in _fileSystemHelper.EnumerateDirectories(parentPath)
                .OrderBy(d => _pathProvider.GetFileName(d), StringComparer.OrdinalIgnoreCase))
            {
                var directoryName = _pathProvider.GetFileName(directory);
                if (string.Equals(directoryName, RepositoryService.MetadataFolderName, StringComparison.OrdinalIgnoreCase)
                    || string.Equals(directoryName, VersionHelper.HistoryFolderName, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                // Determine if this repository needs a warning icon
                bool hasWarning = false;
                if (childNodeType == NodeType.Repository)
                {
                    hasWarning = HasRepositoryDeadlineWarning(directory);
                }

                var iconKey = childNodeType == NodeType.Repository 
                    ? _iconProvider.GetFolderIconKey(directory, hasWarning)
                    : _iconProvider.GetIconKey(directory);

                var childNode = new TreeNode(directoryName)
                {
                    Tag = new NodeData(directory, childNodeType),
                    ImageKey = iconKey,
                    SelectedImageKey = iconKey
                };

                parentNode.Nodes.Add(childNode);
            }

            foreach (var filePath in _fileSystemHelper.EnumerateFiles(parentPath)
                .OrderBy(f => _pathProvider.GetFileName(f), StringComparer.OrdinalIgnoreCase))
            {
                if (_validationHelper.IsSystemManagedFile(filePath, semesterMarkerFileName))
                    continue;

                var isValidFile = parentData.NodeType != NodeType.Subject;
                parentNode.Nodes.Add(new TreeNode(_pathProvider.GetFileName(filePath))
                {
                    Tag = new NodeData(filePath, NodeType.File, isValidFile),
                    ImageKey = _iconProvider.GetIconKey(filePath),
                    SelectedImageKey = _iconProvider.GetIconKey(filePath)
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

    private void ForceLoadPath(TreeNodeCollection nodes, string targetPath, string semesterMarkerFileName)
    {
        foreach (TreeNode node in nodes)
        {
            if (node.Tag is NodeData data && targetPath.StartsWith(data.Path, StringComparison.OrdinalIgnoreCase))
            {
                LoadChildNodes(node, semesterMarkerFileName);
                ForceLoadPath(node.Nodes, targetPath, semesterMarkerFileName);
                break;
            }
        }
    }

    private List<string> GetExpandedPaths(TreeNodeCollection nodes)
    {
        var paths = new List<string>();
        foreach (TreeNode node in nodes)
        {
            if (node.IsExpanded && node.Tag is NodeData data)
            {
                paths.Add(data.Path);
            }

            if (node.Nodes.Count > 0)
            {
                paths.AddRange(GetExpandedPaths(node.Nodes));
            }
        }

        return paths;
    }

    private void RestoreExpandedPaths(TreeView treeView, List<string> expandedPaths, string semesterMarkerFileName)
    {
        foreach (var path in expandedPaths.OrderBy(p => p.Length))
        {
            var node = FindNodeByPath(treeView.Nodes, path);
            if (node is null)
                continue;

            LoadChildNodes(node, semesterMarkerFileName);
            node.Expand();
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

    /// <summary>
    /// Checks if a repository node should display a warning icon based on deadline validation.
    /// Returns true if the repository is overdue, due today, or submitted late.
    /// </summary>
    private bool HasRepositoryDeadlineWarning(string repositoryPath)
    {
        try
        {
            if (!_validationHelper.IsRepositoryFolder(repositoryPath))
                return false;

            var metadata = _repositoryService.EnsureMetadata(repositoryPath);
            var today = DateTime.Today;
            var daysUntilDue = (metadata.Deadline - today).Days;

            // Show warning if overdue or due today
            if (daysUntilDue <= 0)
                return true;

            // Show warning if submitted late
            if (metadata.Status == "submitted" && metadata.Submitted.HasValue)
            {
                return metadata.Submitted.Value > metadata.Deadline;
            }

            return false;
        }
        catch
        {
            // If we can't validate, don't show a warning
            return false;
        }
    }
}
