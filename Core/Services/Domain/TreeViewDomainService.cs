using System.Windows.Forms;
using IskolRepository.Core.Interfaces.Domain;
using IskolRepository.Core.Interfaces.Infrastructure;

namespace IskolRepository.Core.Services.Domain;

/// <summary>
/// Implementation of ITreeViewDomainService.
/// </summary>
public class TreeViewDomainService : ITreeViewDomainService
{
    private readonly IconProvider _iconProvider = new();
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
            ImageKey = _iconProvider.GetIconKey(semesterPath),
            SelectedImageKey = _iconProvider.GetIconKey(semesterPath)
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

        // 1. CAPTURE STATE before clearing
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

        // 2. RESTORE STATE top-down so lazy-loading populates the nodes
        RestoreExpandedPaths(repositoryTreeView, expandedPaths, semesterMarkerFileName);

        // 3. SELECT NEW FILE (Ensure the parents of the newly created file are loaded)
        if (!string.IsNullOrWhiteSpace(selectPath))
        {
            // We might need to load nodes down to the selectPath if it wasn't previously expanded
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
                    ImageKey = _iconProvider.GetIconKey(directory),
                    SelectedImageKey = _iconProvider.GetIconKey(directory)
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
        // Find the deepest existing node that is part of the target path
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





    public List<string> GetExpandedPaths(TreeNodeCollection nodes)
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

    public void RestoreExpandedPaths(TreeView treeView, List<string> expandedPaths, string semesterMarkerFileName)
    {
        // CRITICAL: Sort by path length so we expand parents before children.
        // This ensures lazy-loaded child nodes are generated before we try to find them.
        var sortedPaths = expandedPaths.OrderBy(p => p.Length).ToList();

        foreach (var path in sortedPaths)
        {
            var node = FindNodeByPath(treeView.Nodes, path);
            if (node != null)
            {
                // Force load children so the next level down can be found
                LoadChildNodes(node, semesterMarkerFileName);
                node.Expand();
            }
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
    /// Determines if a file node is valid.
    /// A file is invalid if its parent is a Subject node (file directly under subject, outside any repository).
    /// </summary>
    private static bool IsValidFileNode(TreeNode parentNode, NodeData parentData)
    {
        // Files are valid if parent is not a Subject (files must be inside repositories or their subdirectories)
        return parentData.NodeType != NodeType.Subject;
    }
}
