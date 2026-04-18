using System.Text.Json;
using IskolRepository.Core;
using IskolRepository.Core.Interfaces.Application;
using IskolRepository.Core.Interfaces.Domain;
using IskolRepository.Core.Interfaces.Infrastructure;
using IskolRepository.Models;

namespace IskolRepository.Forms;

public partial class MainForm : Form
{
    private const string SemesterMarkerFileName = ".semester.json";
    private readonly JsonSerializerOptions jsonOptions = new() { WriteIndented = true };

    // Injected services
    private readonly ISemesterApplicationService _semesterService;
    private readonly IRepositoryApplicationService _repositoryService;
    private readonly IFileApplicationService _fileApplicationService;
    private readonly IFileDomainService _fileDomainService;
    private readonly ISubjectApplicationService _subjectService;
    private readonly ITreeViewDomainService _treeViewService;
    private readonly IVersionDomainService _versionService;
    private readonly IValidationHelper _validationService;


    // UI state
    private string? currentSemesterPath;
    private string? currentSubjectPath;
    private string? selectedRepositoryPath;
    private string? selectedFilePath;

    public MainForm(ApplicationServices services)
    {
        ArgumentNullException.ThrowIfNull(services);

        _semesterService = services.SemesterService;
        _repositoryService = services.RepositoryService;
        _fileApplicationService = services.FileService;
        _fileDomainService = services.FileDomainService;
        _subjectService = services.SubjectService;
        _treeViewService = services.TreeViewService;
        _versionService = services.VersionService;
        _validationService = services.ValidationService;

        InitializeComponent();
        statusComboBox.SelectedIndex = 0;
        ShowStartupView();
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

        var openedSemesterPath = _semesterService.OpenSemester(dialog.SelectedPath);
        if (string.IsNullOrWhiteSpace(openedSemesterPath))
        {
            MessageBox.Show(
                "The selected folder is not a valid semester folder.",
                "Invalid Semester",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            return;
        }

        ActivateSemester(openedSemesterPath);
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

        var targetPath = Path.Combine(dialog.SelectedPath, semesterName.Trim());

        try
        {
            if (Directory.Exists(targetPath) && Directory.EnumerateFileSystemEntries(targetPath).Any())
            {
                MessageBox.Show(
                    "The target semester folder must be empty before use.",
                    "Semester Not Empty",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            Directory.CreateDirectory(targetPath);
            _semesterService.CreateSemesterMarker(targetPath);
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
        selectedFilePath = null;
        subjectCardsPanel.Controls.Clear();
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

        _fileDomainService.CreateRepositoryFile(selectedRepositoryPath, fileName.Trim(), extension, out string? error);
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
                selectedFilePath = null;
                filesListView.Items.Clear();
                versionsListBox.Items.Clear();
                ClearMetadataDisplay();
                UpdateRepositoryUiState(null);
                UpdateHistoryUiState(null);
                break;
            case NodeType.Repository:
                SelectRepository(nodeData.Path);
                break;
            case NodeType.SubRepository:
                selectedRepositoryPath = nodeData.Path;
                selectedFilePath = null;
                filesListView.Items.Clear();
                versionsListBox.Items.Clear();
                ClearMetadataDisplay();
                UpdateRepositoryUiState(nodeData.Path);
                UpdateHistoryUiState(null);
                break;
            case NodeType.File:
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

                SelectRepository(repoPath);
                SelectFileInList(nodeData.Path);
                selectedFilePath = nodeData.Path;
                LoadVersionHistory(nodeData.Path);
                break;
        }
    }

    private void repositoryTreeView_NodeMouseDoubleClick(object? sender, TreeNodeMouseClickEventArgs e)
    {
        if (e.Node is not null && e.Node.Tag is NodeData nodeData && nodeData.NodeType == NodeType.File)
        {
            OpenFile(nodeData.Path);
        }
    }

    private void filesListView_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (filesListView.SelectedItems.Count == 0)
        {
            selectedFilePath = null;
            UpdateHistoryUiState(null);
            return;
        }

        selectedFilePath = filesListView.SelectedItems[0].Tag as string;
        LoadVersionHistory(selectedFilePath);
    }

    private void filesListView_DoubleClick(object? sender, EventArgs e)
    {
        if (filesListView.SelectedItems.Count == 0)
        {
            return;
        }

        var filePath = filesListView.SelectedItems[0].Tag as string;
        if (string.IsNullOrWhiteSpace(filePath))
        {
            return;
        }

        OpenFile(filePath);
    }

    private void viewHistoryButton_Click(object? sender, EventArgs e)
    {
        LoadVersionHistory(selectedFilePath);
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
            _fileApplicationService.RevertFileVersion(selectedFilePath, selectedVersion);

            if (!string.IsNullOrWhiteSpace(selectedRepositoryPath))
            {
                LoadFiles(selectedRepositoryPath);
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
        selectedFilePath = null;
        semesterNameValueLabel.Text = Path.GetFileName(semesterPath);
        semesterPathValueLabel.Text = semesterPath;
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
                _subjectService.LoadSubjectsUI(
                    currentSemesterPath,
                    subjectCardsPanel,
                    CreateSubjectCard,
                    () => subjectCardsPanel.Controls.Add(CreateEmptyStateLabel("No subjects yet. Create one to start organizing repositories.")));
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
        selectedRepositoryPath = repositoryPath;
        selectedFilePath = null;
        LoadFiles(repositoryPath);
        LoadRepositoryMetadataUI(repositoryPath);
        UpdateRepositoryUiState(repositoryPath);
        UpdateHistoryUiState(null);
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
        selectedFilePath = null;
        selectedPathValueLabel.Text = "No item selected";
        filesListView.Items.Clear();
        versionsListBox.Items.Clear();
        ClearMetadataDisplay();
        UpdateRepositoryUiState(null);
        UpdateHistoryUiState(null);
    }

    private void ShowStartupView()
    {
        mainSplitContainer.Panel1Collapsed = true;
        startupPanel.Visible = true;
        subjectSelectionPanel.Visible = false;
        workspacePanel.Visible = false;
    }

    private void ShowSubjectView()
    {
        mainSplitContainer.Panel1Collapsed = true;
        startupPanel.Visible = false;
        subjectSelectionPanel.Visible = true;
        workspacePanel.Visible = false;
    }

    private void ShowWorkspaceView()
    {
        mainSplitContainer.Panel1Collapsed = false;
        startupPanel.Visible = false;
        subjectSelectionPanel.Visible = false;
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
            var process = System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(filePath)
            {
                UseShellExecute = true
            });

            if (process is not null)
            {
                process.EnableRaisingEvents = true;
                process.Exited += (_, _) =>
                {
                    if (!IsHandleCreated)
                    {
                        return;
                    }

                    BeginInvoke(new Action(() => PromptAndSaveVersion(filePath)));
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

    #endregion

    #region File Management


    private void LoadFiles(string repositoryPath)
    {
        _fileApplicationService.LoadFiles(repositoryPath, filesListView, SemesterMarkerFileName);
    }

    #endregion

    #region Version History

    private void LoadVersionHistory(string? filePath)
    {
        _versionService.LoadVersionHistory(filePath, versionsListBox, historyCaptionLabel);
        UpdateHistoryUiState(filePath);
    }

    private void PromptAndSaveVersion(string filePath)
    {
        _versionService.PromptAndSaveVersion(filePath, selectedFilePath, versionsListBox);
    }

    #endregion

    #region UI Helpers

    private void SelectFileInList(string filePath)
    {
        foreach (ListViewItem item in filesListView.Items)
        {
            if (string.Equals(item.Tag as string, filePath, StringComparison.OrdinalIgnoreCase))
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
    }

    private void UpdateHistoryUiState(string? filePath)
    {
        var hasFile = !string.IsNullOrWhiteSpace(filePath);
        viewHistoryButton.Enabled = hasFile;
        revertButton.Enabled = hasFile && versionsListBox.SelectedItem is not null;
        historyCaptionLabel.Text = hasFile
            ? $"Version History - {Path.GetFileName(filePath)}"
            : "Version History";
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
