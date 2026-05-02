using IskolRepository.Core;
using IskolRepository.Core.Interfaces;
using IskolRepository.Core.Interfaces.Infrastructure;
using IskolRepository.Models;

namespace IskolRepository.Forms;

public partial class MainForm : Form
{
    private const string SemesterMarkerFileName = ".semester.json";

    private readonly ISemesterService _semesterService;
    private readonly IRepositoryService _repositoryService;
    private readonly IFileService _fileService;
    private readonly ISubjectService _subjectService;
    private readonly ITreeViewService _treeViewService;
    private readonly IVersionService _versionService;
    private readonly IValidationHelper _validationService;
    private readonly IconProvider _iconProvider = new();


    // UI state
    private string? currentSemesterPath;
    private string? currentSubjectPath;
    private string? selectedRepositoryPath;
    private string? currentBrowsePath;
    private string? selectedFilePath;

    public MainForm(ServiceRegistry services)
    {
        ArgumentNullException.ThrowIfNull(services);

        _semesterService = services.SemesterService;
        _repositoryService = services.RepositoryService;
        _fileService = services.FileService;
        _subjectService = services.SubjectService;
        _treeViewService = services.TreeViewService;
        _versionService = services.VersionService;
        _validationService = services.ValidationService;

        InitializeComponent();
        
        _startupView.OpenSemesterRequested += openSemesterButton_Click;
        _startupView.NewSemesterRequested += newSemesterButton_Click;
        _subjectSelectionView.AddSubjectRequested += addSubjectButton_Click;
        _subjectSelectionView.ChangeSemesterRequested += changeSemesterButton_Click;
        
        InitializeFilesListViewIcons();
        InitializeTreeViewIcons();

        statusComboBox.SelectedIndex = 0;
        ShowStartupView();
        UpdateSaveVersionButtonState();
    }

    #region Event Handlers

    private void openSemesterButton_Click(object? sender, EventArgs e)
    {
        using var dialog = CreateFolderBrowserDialog("Select an existing semester folder.");
        if (dialog.ShowDialog(this) != DialogResult.OK || string.IsNullOrWhiteSpace(dialog.SelectedPath))
        {
            return;
        }

        if (!Directory.Exists(dialog.SelectedPath))
        {
            MessageBox.Show(
                "The selected semester folder does not exist.",
                "Invalid Semester",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            return;
        }

        try
        {
            var openedSemesterPath = _semesterService.OpenSemester(dialog.SelectedPath);
            ActivateSemester(openedSemesterPath);
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                ex.Message,
                "Invalid Semester",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
        }
    }

    private void newSemesterButton_Click(object? sender, EventArgs e)
    {
        using var dialog = CreateFolderBrowserDialog("Select where the new semester folder should be created.");
        if (dialog.ShowDialog(this) != DialogResult.OK || string.IsNullOrWhiteSpace(dialog.SelectedPath))
        {
            return;
        }

        var semesterName = PromptDialog.ShowDialog(
            "Enter the new semester name:",
            "New Semester");

        if (string.IsNullOrWhiteSpace(semesterName))
        {
            return;
        }

        if (!_validationService.IsValidName(semesterName))
        {
            MessageBox.Show(
                "Please enter a valid semester name.",
                "Invalid Name",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            return;
        }

        try
        {
            var targetPath = _semesterService.CreateSemester(dialog.SelectedPath, semesterName.Trim());
            ActivateSemester(targetPath);
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"Unable to create the semester folder.\n\n{ex.Message}",
                "Semester Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    private void addSubjectButton_Click(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(currentSemesterPath))
        {
            MessageBox.Show(
                "Open or create a semester first.",
                "No Semester Selected",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            return;
        }

        var subjectName = PromptDialog.ShowDialog(
            "Enter a subject name:",
            "Create Subject");

        if (string.IsNullOrWhiteSpace(subjectName))
        {
            return;
        }

        try
        {
            _subjectService.CreateSubject(currentSemesterPath, subjectName.Trim());
            LoadSubjectsUI();
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"Unable to create the subject.\n\n{ex.Message}",
                "Subject Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    private void changeSemesterButton_Click(object? sender, EventArgs e)
    {
        currentSemesterPath = null;
        currentSubjectPath = null;
        selectedRepositoryPath = null;
        currentBrowsePath = null;
        selectedFilePath = null;
        _subjectSelectionView.PopulateSubjects(_ => { });
        repositoryTreeView.Nodes.Clear();
        filesListView.Items.Clear();
        versionsListBox.Items.Clear();
        ClearMetadataDisplay();
        ShowStartupView();
    }

    private void backToSubjectsButton_Click(object? sender, EventArgs e)
    {
        currentSubjectPath = null;
        selectedRepositoryPath = null;
        currentBrowsePath = null;
        selectedFilePath = null;
        repositoryTreeView.Nodes.Clear();
        filesListView.Items.Clear();
        versionsListBox.Items.Clear();
        ClearMetadataDisplay();
        LoadSubjectsUI();
        ShowSubjectView();
    }

    private void createRepositoryButton_Click(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(currentSubjectPath))
        {
            MessageBox.Show(
                "Please open a subject first.",
                "No Subject Selected",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            return;
        }

        var repositoryInput = RepoCreationDialog.ShowCreateDialog(this);
        if (repositoryInput is null)
        {
            return;
        }

        var repositoryName = repositoryInput.RepositoryName.Trim();
        if (!_validationService.IsValidName(repositoryName))
        {
            MessageBox.Show(
                "Please enter a valid repository name.",
                "Invalid Name",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            return;
        }

        var repositoryPath = Path.Combine(currentSubjectPath, repositoryName);
        if (Directory.Exists(repositoryPath))
        {
            MessageBox.Show(
                "A repository with that name already exists in this subject.",
                "Duplicate Folder",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            return;
        }

        try
        {
            var createdPath = _repositoryService.CreateRepository(
                currentSubjectPath,
                repositoryName,
                repositoryInput.Deadline.Date);

            LoadSubjectTree(createdPath);

        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"Unable to create the repository.\n\n{ex.Message}",
                "Create Repository Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    private void createFileButton_Click(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(selectedRepositoryPath))
        {
            MessageBox.Show(
                "Please select a repository first.",
                "No Repository Selected",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            return;
        }

        var fileName = PromptDialog.ShowDialog(
            "Enter a file name:",
            "Create File");

        if (string.IsNullOrWhiteSpace(fileName))
        {
            return;
        }

        if (!_validationService.IsValidName(fileName))
        {
            MessageBox.Show(
                "Please enter a valid file name.",
                "Invalid Name",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            return;
        }

        var extension = FileTypeDialog.ShowCreateDialog(this);
        if (string.IsNullOrWhiteSpace(extension))
        {
            return;
        }

        var targetBrowsePath = currentBrowsePath ?? selectedRepositoryPath;
        try
        {
            var createdFilePath = _fileService.CreateFile(targetBrowsePath, fileName.Trim(), extension);
            DisplayRepositoryContents(selectedRepositoryPath, targetBrowsePath);
            LoadSubjectTree(createdFilePath);
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                ex.Message,
                "Create File Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
        }
    }

    private void createSubrepositoryButton_Click(object? sender, EventArgs e)
    {
        var parentFolderPath = GetSelectedSubrepositoryParentPath();
        if (string.IsNullOrWhiteSpace(parentFolderPath))
        {
            MessageBox.Show(
                "Please select a repository or subrepository first.",
                "No Folder Selected",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            return;
        }

        var subrepositoryName = PromptDialog.ShowDialog(
            "Enter a subrepository name:",
            "Create Subrepository");

        if (string.IsNullOrWhiteSpace(subrepositoryName))
        {
            return;
        }

        if (!_validationService.IsValidName(subrepositoryName))
        {
            MessageBox.Show(
                "Please enter a valid subrepository name.",
                "Invalid Name",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            return;
        }

        try
        {
            _fileService.CreateFolder(parentFolderPath, subrepositoryName.Trim(), "subrepository");
            var createdPath = Path.Combine(parentFolderPath, subrepositoryName.Trim());
            DisplayRepositoryContents(selectedRepositoryPath!, parentFolderPath);
            LoadSubjectTree(createdPath);
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                ex.Message,
                "Create Subrepository Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
        }
    }

    private void updateMetadataButton_Click(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(selectedRepositoryPath))
        {
            MessageBox.Show(
                "Please select a repository first.",
                "Invalid Selection",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            return;
        }

        var status = statusComboBox.SelectedItem?.ToString();
        if (!_validationService.IsValidStatus(status))
        {
            MessageBox.Show(
                "Please select a valid repository status.",
                "Invalid Metadata",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            return;
        }

        try
        {
            _repositoryService.UpdateRepositoryMetadata(
                selectedRepositoryPath,
                deadlineDateTimePicker.Value.Date,
                status!);

            LoadRepositoryMetadataUI(selectedRepositoryPath);

            MessageBox.Show(
                "Repository metadata updated successfully.",
                "Metadata Updated",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"Unable to update metadata.\n\n{ex.Message}",
                "Metadata Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    private void saveVersionButton_Click(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(selectedFilePath))
        {
            MessageBox.Show(
                "Please select a file first.",
                "No File Selected",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            return;
        }

        if (!_versionService.CanSaveVersion(selectedFilePath))
        {
            MessageBox.Show(
                "This file is not eligible for saving a new version yet.",
                "Version Save Not Available",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
            return;
        }

        try
        {
            var comment = PromptDialog.ShowDialog(
                $"Enter a version comment for {Path.GetFileName(selectedFilePath)}:\n\nSelect Cancel to skip saving a snapshot.",
                "Save Version");

            if (string.IsNullOrWhiteSpace(comment))
            {
                return;
            }

            _versionService.SaveVersion(selectedFilePath, comment);
            LoadVersionHistory(selectedFilePath);
            UpdateSaveVersionButtonState();
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"Unable to save a new version.\n\n{ex.Message}",
                "Save Version Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    private void repositoryTreeView_AfterSelect(object? sender, TreeViewEventArgs e)
    {
        if (e.Node?.Tag is not NodeData nodeData)
        {
            ResetWorkspaceSelection();
            return;
        }

        selectedPathValueLabel.Text = nodeData.Path;

        // Lazy load children when node is selected
        LoadChildNodes(e.Node);

        switch (nodeData.NodeType)
        {
            case NodeType.Semester:
            case NodeType.Subject:
                selectedRepositoryPath = null;
                currentBrowsePath = null;
                selectedFilePath = null;
                filesListView.Items.Clear();
                versionsListBox.Items.Clear();
                ClearMetadataDisplay();
                UpdateRepositoryUiState(null);
                UpdateHistoryUiState(null);
                ShowMessage("Please select a repository");
                break;
            case NodeType.Repository:
                SelectRepository(nodeData.Path);
                HideMessage();
                break;
            case NodeType.SubRepository:
                var repositoryRoot = _repositoryService.FindRepositoryRoot(nodeData.Path) ?? nodeData.Path;
                SelectRepository(repositoryRoot, nodeData.Path);
                HideMessage();
                break;
            case NodeType.File:
                // Check if file is valid (inside a repository)
                if (!nodeData.IsValidFile)
                {
                    selectedRepositoryPath = null;
                    selectedFilePath = null;
                    filesListView.Items.Clear();
                    versionsListBox.Items.Clear();
                    ClearMetadataDisplay();
                    UpdateRepositoryUiState(null);
                    UpdateHistoryUiState(null);
                    ShowMessage($"File \"{nodeData.FileName}\" not under a repository / unknown activity");
                    return;
                }

                var parentPath = Path.GetDirectoryName(nodeData.Path);
                if (string.IsNullOrWhiteSpace(parentPath))
                {
                    ResetWorkspaceSelection();
                    return;
                }

                // Find the repository root from ancestors
                var repoPath = _repositoryService.FindRepositoryRoot(parentPath);
                if (string.IsNullOrWhiteSpace(repoPath))
                {
                    repoPath = parentPath;
                }

                SelectRepository(repoPath, parentPath);
                SelectFileInList(nodeData.Path);
                selectedFilePath = nodeData.Path;
                LoadVersionHistory(nodeData.Path);
                HideMessage();
                break;
        }
    }

    private void repositoryTreeView_NodeMouseDoubleClick(object? sender, TreeNodeMouseClickEventArgs e)
    {
        if (e.Node?.Tag is not NodeData nodeData || nodeData.NodeType != NodeType.File)
            return;

        // Prevent opening invalid files (files outside repositories)
        if (!nodeData.IsValidFile)
            return;

        OpenFile(nodeData.Path);
    }

    private void filesListView_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (filesListView.SelectedItems.Count == 0)
        {
            selectedFilePath = null;
            UpdateHistoryUiState(null);
            UpdateCreateSubrepositoryButtonState();
            UpdateSaveVersionButtonState();
            return;
        }

        if (filesListView.SelectedItems[0].Tag is not RepositoryBrowseEntry browseEntry)
        {
            selectedFilePath = null;
            UpdateHistoryUiState(null);
            UpdateCreateSubrepositoryButtonState();
            UpdateSaveVersionButtonState();
            return;
        }

        if (browseEntry.Kind == RepositoryBrowseEntryKind.File)
        {
            selectedFilePath = browseEntry.Path;
            LoadVersionHistory(selectedFilePath);
        }
        else
        {
            selectedFilePath = null;
            versionsListBox.Items.Clear();
            UpdateHistoryUiState(null);
        }

        UpdateCreateSubrepositoryButtonState();
        UpdateSaveVersionButtonState();
    }

    private void filesListView_DoubleClick(object? sender, EventArgs e)
    {
        if (filesListView.SelectedItems.Count == 0)
        {
            return;
        }

        if (filesListView.SelectedItems[0].Tag is not RepositoryBrowseEntry browseEntry)
        {
            return;
        }

        switch (browseEntry.Kind)
        {
            case RepositoryBrowseEntryKind.File:
                OpenFile(browseEntry.Path);
                break;
            case RepositoryBrowseEntryKind.Directory:
            case RepositoryBrowseEntryKind.Parent:
                NavigateToBrowsePath(browseEntry.Path);
                break;
        }
    }

    private void versionsListBox_SelectedIndexChanged(object? sender, EventArgs e)
    {
        revertButton.Enabled = !string.IsNullOrWhiteSpace(selectedFilePath)
            && versionsListBox.SelectedItem is not null;
    }

    private void revertButton_Click(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(selectedFilePath))
        {
            MessageBox.Show(
                "Please select a file first.",
                "No File Selected",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            return;
        }

        if (versionsListBox.SelectedItem is not FileVersion selectedVersion)
        {
            MessageBox.Show(
                "Please select a version to revert to.",
                "No Version Selected",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            return;
        }

        var confirmation = MessageBox.Show(
            "This will restore the selected version and delete all newer versions. Continue?",
            "Confirm Revert",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Warning);

        if (confirmation != DialogResult.Yes)
        {
            return;
        }

        try
        {
            _versionService.RevertToVersion(selectedFilePath, selectedVersion);

            if (!string.IsNullOrWhiteSpace(selectedRepositoryPath))
            {
                DisplayRepositoryContents(selectedRepositoryPath, currentBrowsePath ?? selectedRepositoryPath);
                SelectFileInList(selectedFilePath);
            }

            LoadSubjectTree(selectedFilePath);
            LoadVersionHistory(selectedFilePath);

            MessageBox.Show(
                "File reverted successfully.",
                "Revert Complete",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"Unable to revert the selected file version.\n\n{ex.Message}",
                "Revert Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    #endregion

    #region Semester Management

    private void ActivateSemester(string semesterPath)
    {
        currentSemesterPath = semesterPath;
        currentSubjectPath = null;
        selectedRepositoryPath = null;
        currentBrowsePath = null;
        selectedFilePath = null;
        _subjectSelectionView.SemesterName = Path.GetFileName(semesterPath);
        _subjectSelectionView.SemesterPath = semesterPath;
        selectedSubjectValueLabel.Text = "No subject selected";
        repositoryTreeView.Nodes.Clear();
        filesListView.Items.Clear();
        versionsListBox.Items.Clear();
        ClearMetadataDisplay();
        _treeViewService.LoadSemesterTree(semesterPath, repositoryTreeView, SemesterMarkerFileName);
        LoadSubjectsUI();
        ShowSubjectView();
    }

    #endregion

    #region Subject Management

    private void LoadSubjectsUI()
    {
        try
        {
            if (!string.IsNullOrWhiteSpace(currentSemesterPath))
            {
                _subjectSelectionView.PopulateSubjects(panel =>
                    _subjectService.LoadSubjectsUI(
                        currentSemesterPath,
                        panel,
                        CreateSubjectCard,
                        () => panel.Controls.Add(CreateEmptyStateLabel("No subjects yet. Create one to start organizing repositories."))));
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"Unable to load subjects.\n\n{ex.Message}",
                "Load Subjects Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    private Control CreateSubjectCard(string subjectPath)
    {
        var button = new Button
        {
            AutoSize = false,
            Width = 220,
            Height = 90,
            Margin = new Padding(0, 0, 16, 16),
            TextAlign = ContentAlignment.MiddleLeft,
            Font = new Font("Segoe UI", 10F, FontStyle.Bold),
            Text = Path.GetFileName(subjectPath),
            Tag = subjectPath,
            UseVisualStyleBackColor = true
        };

        button.Click += (_, _) => OpenSubject(subjectPath);
        return button;
    }

    private static Label CreateEmptyStateLabel(string message)
    {
        return new Label
        {
            AutoSize = true,
            MaximumSize = new Size(500, 0),
            Text = message,
            Padding = new Padding(8)
        };
    }

    private void OpenSubject(string subjectPath)
    {
        currentSubjectPath = subjectPath;
        selectedSubjectValueLabel.Text = Path.GetFileName(subjectPath);
        LoadSubjectTree(subjectPath);
        ShowWorkspaceView();
    }

    #endregion

    #region Semester Tree Management

    private void LoadSubjectTree(string? selectPath = null)
    {
        _treeViewService.LoadSubjectTree(currentSubjectPath, selectPath, repositoryTreeView, SemesterMarkerFileName);
    }

    #endregion

    #region TreeView Logic & Helpers

    private void LoadChildNodes(TreeNode parentNode)
    {
        _treeViewService.LoadChildNodes(parentNode, SemesterMarkerFileName);
    }

    #endregion

    #region Repository Management

    private void SelectRepository(string repositoryPath)
    {
        SelectRepository(repositoryPath, repositoryPath);
    }

    private void SelectRepository(string repositoryRootPath, string browsePath)
    {
        DisplayRepositoryContents(repositoryRootPath, browsePath);
        LoadRepositoryMetadataUI(repositoryRootPath);
    }

    private void LoadRepositoryMetadataUI(string repositoryPath)
    {
        try
        {
            var metadata = _repositoryService.EnsureMetadata(repositoryPath);
            deadlineValueLabel.Text = metadata.Deadline.ToString("yyyy-MM-dd");
            dateAddedValueLabel.Text = metadata.DateAdded.ToString("yyyy-MM-dd");
            deadlineDateTimePicker.Value = metadata.Deadline;
            statusComboBox.SelectedItem = metadata.Status;
        }
        catch (Exception ex)
        {
            ClearMetadataDisplay();
            MessageBox.Show(
                $"Unable to load repository metadata.\n\n{ex.Message}",
                "Metadata Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    #endregion

    #region View Management

    private void ResetWorkspaceSelection()
    {
        selectedRepositoryPath = null;
        currentBrowsePath = null;
        selectedFilePath = null;
        selectedPathValueLabel.Text = "No item selected";
        filesListView.Items.Clear();
        versionsListBox.Items.Clear();
        ClearMetadataDisplay();
        UpdateRepositoryUiState(null);
        UpdateHistoryUiState(null);
        ShowMessage("Please select a repository");
    }

    private void ShowStartupView()
    {
        mainSplitContainer.Panel1Collapsed = true;
        pathHeaderPanel.Visible = false;
        toolbarHeaderPanel.Visible = false;
        _startupView.Visible = true;
        _subjectSelectionView.Visible = false;
        workspacePanel.Visible = false;
    }

    private void ShowSubjectView()
    {
        mainSplitContainer.Panel1Collapsed = true;
        pathHeaderPanel.Visible = false;
        toolbarHeaderPanel.Visible = false;
        _startupView.Visible = false;
        _subjectSelectionView.Visible = true;
        workspacePanel.Visible = false;
    }

    private void ShowWorkspaceView()
    {
        mainSplitContainer.Panel1Collapsed = false;
        pathHeaderPanel.Visible = true;
        toolbarHeaderPanel.Visible = true;
        _startupView.Visible = false;
        _subjectSelectionView.Visible = false;
        workspacePanel.Visible = true;
    }

    private void OpenFile(string filePath)
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
            _fileService.OpenFile(filePath);
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

    #endregion

    #region File Management


    private void LoadFiles(string repositoryRootPath, string browsePath)
    {
        _fileService.LoadFiles(repositoryRootPath, browsePath, filesListView, SemesterMarkerFileName);
    }

    #endregion

    #region Version History

    private void LoadVersionHistory(string? filePath)
    {
        _versionService.LoadVersionHistory(filePath, versionsListBox, historyCaptionLabel, noVersionsMessageLabel);
        UpdateHistoryUiState(filePath);
    }

    #endregion

    #region UI Helpers

    private void SelectFileInList(string filePath)
    {
        foreach (ListViewItem item in filesListView.Items)
        {
            if (item.Tag is RepositoryBrowseEntry browseEntry
                && browseEntry.Kind == RepositoryBrowseEntryKind.File
                && string.Equals(browseEntry.Path, filePath, StringComparison.OrdinalIgnoreCase))
            {
                item.Selected = true;
                item.Focused = true;
                item.EnsureVisible();
                break;
            }
        }
    }

    private void ClearMetadataDisplay()
    {
        deadlineValueLabel.Text = "-";
        dateAddedValueLabel.Text = "-";
        deadlineDateTimePicker.Value = DateTime.Today;
        statusComboBox.SelectedIndex = 0;
    }

    private void UpdateRepositoryUiState(string? repositoryPath)
    {
        var hasRepository = !string.IsNullOrWhiteSpace(repositoryPath);
        createFileButton.Enabled = hasRepository;
        deadlineDateTimePicker.Enabled = hasRepository;
        statusComboBox.Enabled = hasRepository;
        updateMetadataButton.Enabled = hasRepository;
        UpdateCreateSubrepositoryButtonState();
        UpdateSaveVersionButtonState();
    }

    private void UpdateHistoryUiState(string? filePath)
    {
        var hasFile = !string.IsNullOrWhiteSpace(filePath);
        revertButton.Enabled = hasFile && versionsListBox.SelectedItem is not null;
        historyCaptionLabel.Text = hasFile
            ? $"Version History - {Path.GetFileName(filePath)}"
            : "Version History";
        UpdateSaveVersionButtonState();
    }

    private void UpdateSaveVersionButtonState()
    {
        if (string.IsNullOrWhiteSpace(selectedFilePath))
        {
            saveVersionButton.Text = "Select a file to save a version of";
            saveVersionButton.Enabled = false;
            return;
        }

        var fileName = Path.GetFileName(selectedFilePath);
        saveVersionButton.Text = $"Save a version for {fileName}";
        saveVersionButton.Enabled = _versionService.CanSaveVersion(selectedFilePath);
    }

    private void UpdateCreateSubrepositoryButtonState()
    {
        createSubrepositoryButton.Enabled = !string.IsNullOrWhiteSpace(GetSelectedSubrepositoryParentPath());
    }

    private void DisplayRepositoryContents(string repositoryRootPath, string browsePath)
    {
        selectedRepositoryPath = repositoryRootPath;
        currentBrowsePath = browsePath;
        selectedFilePath = null;
        LoadFiles(repositoryRootPath, browsePath);
        UpdateRepositoryUiState(repositoryRootPath);
        UpdateHistoryUiState(null);
    }

    private void NavigateToBrowsePath(string targetPath)
    {
        if (string.IsNullOrWhiteSpace(selectedRepositoryPath) || string.IsNullOrWhiteSpace(targetPath))
        {
            return;
        }

        var repositoryRoot = Path.GetFullPath(selectedRepositoryPath);
        var normalizedTargetPath = Path.GetFullPath(targetPath);

        if (!IsPathInsideRepository(normalizedTargetPath, repositoryRoot))
        {
            return;
        }

        var targetNode = _treeViewService.FindNodeByPath(repositoryTreeView.Nodes, normalizedTargetPath);
        if (targetNode is not null)
        {
            repositoryTreeView.SelectedNode = targetNode;
            targetNode.EnsureVisible();
            return;
        }

        DisplayRepositoryContents(repositoryRoot, normalizedTargetPath);
        HideMessage();
    }

    private static bool IsPathInsideRepository(string path, string repositoryRoot)
    {
        if (string.Equals(path, repositoryRoot, StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }

        var normalizedRoot = repositoryRoot.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)
            + Path.DirectorySeparatorChar;
        var normalizedPath = path.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)
            + Path.DirectorySeparatorChar;

        return normalizedPath.StartsWith(normalizedRoot, StringComparison.OrdinalIgnoreCase);
    }

    private string? GetSelectedSubrepositoryParentPath()
    {
        if (repositoryTreeView.SelectedNode?.Tag is NodeData nodeData
            && (nodeData.NodeType == NodeType.Repository || nodeData.NodeType == NodeType.SubRepository))
        {
            return nodeData.Path;
        }

        if (filesListView.SelectedItems.Count > 0
            && filesListView.SelectedItems[0].Tag is RepositoryBrowseEntry browseEntry
            && browseEntry.Kind == RepositoryBrowseEntryKind.Directory)
        {
            return browseEntry.Path;
        }

        return null;
    }

    private void InitializeFilesListViewIcons()
    {
        filesListView.SmallImageList = _iconProvider.CreateImageList();
    }

    private void InitializeTreeViewIcons()
    {
        repositoryTreeView.ImageList = _iconProvider.CreateTreeViewImageList();
    }

    #endregion

    #region Message Panel Management

    /// <summary>
    /// Shows the repository message label and hides file list.
    /// </summary>
    private void ShowMessage(string message)
    {
        noRepositoryMessageLabel.Text = message;
        noRepositoryMessageLabel.Visible = true;
        filesListView.Visible = false;
    }

    /// <summary>
    /// Hides the repository message label and shows file list.
    /// </summary>
    private void HideMessage()
    {
        noRepositoryMessageLabel.Visible = false;
        filesListView.Visible = true;
    }

    #endregion

    #region Static Helpers & Validation

    private static FolderBrowserDialog CreateFolderBrowserDialog(string description)
    {
        return new FolderBrowserDialog
        {
            Description = description,
            UseDescriptionForTitle = true,
            ShowNewFolderButton = true
        };
    }

    #endregion

    #region Nested Classes - Moved to Core

    // NodeType, NodeData, and VersionListItem are now defined in Core/TreeNodeData.cs
    // They are imported via the IskolRepository.Core namespace

    #endregion

}
