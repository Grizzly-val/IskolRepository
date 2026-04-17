using System.Windows.Forms;

namespace IskolRepository.Core;

/// <summary>
/// Manages TreeView operations including node loading, lazy loading, and tree navigation.
/// </summary>
public static class TreeViewManager
{
    public static void LoadSemesterTree(string semesterPath, TreeView repositoryTreeView, string semesterMarkerFileName)
    {
        if (string.IsNullOrWhiteSpace(semesterPath) || !Directory.Exists(semesterPath))
        {
            repositoryTreeView.Nodes.Clear();
            return;
        }

        repositoryTreeView.BeginUpdate();
        repositoryTreeView.Nodes.Clear();

        var rootNode = new TreeNode(Path.GetFileName(semesterPath))
        {
            Tag = new NodeData(semesterPath, NodeType.Semester)
        };

        repositoryTreeView.Nodes.Add(rootNode);
        rootNode.Expand();

        LoadChildNodes(rootNode, semesterMarkerFileName);

        repositoryTreeView.EndUpdate();

        // Apply validation coloring
        foreach (TreeNode root in repositoryTreeView.Nodes)
        {
            ValidationHelper.ApplyNodeValidationColors(root);
        }
    }

    public static void LoadSubjectTree(string? currentSubjectPath, string? selectPath, TreeView repositoryTreeView, string semesterMarkerFileName)
    {
        if (string.IsNullOrWhiteSpace(currentSubjectPath) || !Directory.Exists(currentSubjectPath))
        {
            repositoryTreeView.Nodes.Clear();
            return;
        }

        repositoryTreeView.BeginUpdate();
        repositoryTreeView.Nodes.Clear();

        var rootNode = new TreeNode(Path.GetFileName(currentSubjectPath))
        {
            Tag = new NodeData(currentSubjectPath, NodeType.Subject)
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
            ValidationHelper.ApplyNodeValidationColors(root);
        }
    }

    public static void LoadChildNodes(TreeNode parentNode, string semesterMarkerFileName)
    {
        if (parentNode?.Tag is not NodeData parentData)
        {
            return;
        }

        // Prevent duplicate loading
        if (parentNode.Nodes.Count > 0)
        {
            return;
        }

        try
        {
            var parentPath = parentData.Path;
            if (!Directory.Exists(parentPath))
            {
                return;
            }

            var childNodeType = GetChildNodeType(parentData.NodeType);

            foreach (var directory in Directory.GetDirectories(parentPath)
                         .OrderBy(path => Path.GetFileName(path), StringComparer.OrdinalIgnoreCase))
            {
                if (string.Equals(Path.GetFileName(directory), VersionHelper.HistoryFolderName, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                var childNode = new TreeNode(Path.GetFileName(directory))
                {
                    Tag = new NodeData(directory, childNodeType)
                };

                parentNode.Nodes.Add(childNode);
            }

            foreach (var filePath in Directory.GetFiles(parentPath)
                         .OrderBy(path => Path.GetFileName(path), StringComparer.OrdinalIgnoreCase))
            {
                if (ValidationHelper.IsSystemManagedFile(filePath, semesterMarkerFileName))
                {
                    continue;
                }

                parentNode.Nodes.Add(new TreeNode(Path.GetFileName(filePath))
                {
                    Tag = new NodeData(filePath, NodeType.File)
                });
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"Unable to load the selected item.\n\n{ex.Message}",
                "Load Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    public static TreeNode? FindNodeByPath(TreeNodeCollection nodes, string path)
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

    public static void EnsureParentChainExpanded(TreeNode node)
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
}
