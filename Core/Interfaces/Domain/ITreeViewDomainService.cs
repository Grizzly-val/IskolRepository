using System.Windows.Forms;

namespace IskolRepository.Core.Interfaces.Domain;

/// <summary>
/// Domain service for TreeView operations.
/// </summary>
public interface ITreeViewDomainService
{
    /// <summary>
    /// Loads the semester tree structure.
    /// </summary>
    void LoadSemesterTree(string semesterPath, TreeView repositoryTreeView, string semesterMarkerFileName);

    /// <summary>
    /// Loads the subject tree structure.
    /// </summary>
    void LoadSubjectTree(string? currentSubjectPath, string? selectPath, TreeView repositoryTreeView, string semesterMarkerFileName);

    /// <summary>
    /// Lazily loads child nodes for a parent node.
    /// </summary>
    void LoadChildNodes(TreeNode parentNode, string semesterMarkerFileName);

    /// <summary>
    /// Finds a node by its path.
    /// </summary>
    TreeNode? FindNodeByPath(TreeNodeCollection nodes, string path);

    /// <summary>
    /// Ensures parent chain is expanded.
    /// </summary>
    void EnsureParentChainExpanded(TreeNode node);
}
