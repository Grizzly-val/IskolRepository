using System.Windows.Forms;

namespace IskolRepository.Core.Interfaces;

public interface ITreeViewService
{
    void LoadSemesterTree(string semesterPath, TreeView repositoryTreeView, string semesterMarkerFileName);

    void LoadSubjectTree(string? currentSubjectPath, string? selectPath, TreeView repositoryTreeView, string semesterMarkerFileName);

    void LoadChildNodes(TreeNode parentNode, string semesterMarkerFileName);

    TreeNode? FindNodeByPath(TreeNodeCollection nodes, string path);

    void EnsureParentChainExpanded(TreeNode node);
}
