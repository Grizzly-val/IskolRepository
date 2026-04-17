using System.Diagnostics;
using System.Text.Json;
using IskolRepository.Core;
using IskolRepository.Models;

namespace IskolRepository.Forms;

public partial class MainForm : Form
{
    private const string MetadataFileName = "metadata.json";
    private static readonly string[] ValidStatuses = ["in-progress", "completed", "late"];
    private readonly JsonSerializerOptions jsonOptions = new() { WriteIndented = true };
    private string? currentSemesterPath;
    private string? currentSubjectPath;
    private string? selectedRepositoryPath;
    private string? selectedFilePath;

    public MainForm()
    {
        InitializeComponent();
        statusComboBox.SelectedIndex = 0;
        ShowStartupView();
    }

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

        ActivateSemester(dialog.SelectedPath);
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

        if (!FileSystemHelper.IsValidName(semesterName))
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

            FileSystemHelper.CreateDirectory(targetPath);
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

        CreateFolder(currentSemesterPath, subjectName.Trim(), "subject");
        LoadSubjects();
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
        LoadSubjects();
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
        if (!FileSystemHelper.IsValidName(repositoryName))
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
            FileSystemHelper.CreateDirectory(repositoryPath);
            SaveMetadata(repositoryPath, new RepoMetadata
            {
                Deadline = repositoryInput.Deadline.Date,
                DateAdded = DateTime.Today,
                Status = "in-progress"
            });

            LoadSubjectTree(repositoryPath);
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

        if (!FileSystemHelper.IsValidName(fileName))
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

        CreateRepositoryFile(selectedRepositoryPath, fileName.Trim(), extension);
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
        if (!IsValidStatus(status))
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
            var metadata = EnsureMetadata(selectedRepositoryPath);
            metadata.Deadline = deadlineDateTimePicker.Value.Date;
            metadata.Status = status!;
            SaveMetadata(selectedRepositoryPath, metadata);
            LoadRepositoryMetadata(selectedRepositoryPath);

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

        switch (nodeData.NodeType)
        {
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
            case NodeType.File:
                var repositoryPath = Path.GetDirectoryName(nodeData.Path);
                if (string.IsNullOrWhiteSpace(repositoryPath))
                {
                    ResetWorkspaceSelection();
                    return;
                }

                SelectRepository(repositoryPath);
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

        if (versionsListBox.SelectedItem is not VersionListItem selectedVersion)
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
            VersionHelper.RevertToVersion(selectedFilePath, selectedVersion.Version, selectedVersion.SnapshotPath, jsonOptions);

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
        LoadSubjects();
        ShowSubjectView();
    }

    private void LoadSubjects()
    {
        subjectCardsPanel.SuspendLayout();
        subjectCardsPanel.Controls.Clear();

        if (string.IsNullOrWhiteSpace(currentSemesterPath) || !Directory.Exists(currentSemesterPath))
        {
            subjectCardsPanel.ResumeLayout();
            return;
        }

        try
        {
            var subjects = Directory.GetDirectories(currentSemesterPath)
                .OrderBy(path => Path.GetFileName(path), StringComparer.OrdinalIgnoreCase)
                .ToList();

            if (subjects.Count == 0)
            {
                subjectCardsPanel.Controls.Add(CreateEmptyStateLabel(
                    "No subjects yet. Create one to start organizing repositories."));
            }
            else
            {
                foreach (var subjectPath in subjects)
                {
                    subjectCardsPanel.Controls.Add(CreateSubjectCard(subjectPath));
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"Unable to load subjects.\n\n{ex.Message}",
                "Load Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
        finally
        {
            subjectCardsPanel.ResumeLayout();
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

    private void LoadSubjectTree(string? selectPath = null)
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

        LoadSubjectNodes(currentSubjectPath, rootNode);
        repositoryTreeView.Nodes.Add(rootNode);
        rootNode.Expand();

        var nodeToSelect = !string.IsNullOrWhiteSpace(selectPath)
            ? FindNodeByPath(repositoryTreeView.Nodes, selectPath)
            : rootNode;

        if (nodeToSelect is not null)
        {
            repositoryTreeView.SelectedNode = nodeToSelect;
            EnsureParentChainExpanded(nodeToSelect);
        }

        repositoryTreeView.EndUpdate();
    }

    private void LoadSubjectNodes(string parentPath, TreeNode parentNode)
    {
        try
        {
            foreach (var directory in Directory.GetDirectories(parentPath)
                         .OrderBy(path => Path.GetFileName(path), StringComparer.OrdinalIgnoreCase))
            {
                if (string.Equals(Path.GetFileName(directory), VersionHelper.HistoryFolderName, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                var childNode = new TreeNode(Path.GetFileName(directory))
                {
                    Tag = new NodeData(directory, NodeType.Repository)
                };

                parentNode.Nodes.Add(childNode);
                LoadSubjectNodes(directory, childNode);
            }

            foreach (var filePath in Directory.GetFiles(parentPath)
                         .OrderBy(path => Path.GetFileName(path), StringComparer.OrdinalIgnoreCase))
            {
                if (IsSystemManagedFile(filePath))
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
                $"Unable to load the selected subject.\n\n{ex.Message}",
                "Load Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    private void LoadFiles(string repositoryPath)
    {
        filesListView.Items.Clear();

        try
        {
            foreach (var filePath in Directory.GetFiles(repositoryPath)
                         .OrderBy(path => Path.GetFileName(path), StringComparer.OrdinalIgnoreCase))
            {
                if (IsSystemManagedFile(filePath))
                {
                    continue;
                }

                var item = new ListViewItem(Path.GetFileNameWithoutExtension(filePath));
                item.SubItems.Add(Path.GetExtension(filePath));
                item.Tag = filePath;
                filesListView.Items.Add(item);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"Unable to load files from:\n{repositoryPath}\n\n{ex.Message}",
                "Load Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    private void SelectRepository(string repositoryPath)
    {
        selectedRepositoryPath = repositoryPath;
        selectedFilePath = null;
        LoadFiles(repositoryPath);
        LoadRepositoryMetadata(repositoryPath);
        UpdateRepositoryUiState(repositoryPath);
        UpdateHistoryUiState(null);
    }

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
            var process = Process.Start(new ProcessStartInfo(filePath)
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

    private void CreateFolder(string parentPath, string name, string folderType)
    {
        if (!FileSystemHelper.IsValidName(name))
        {
            MessageBox.Show(
                $"Please enter a valid {folderType} name.",
                "Invalid Name",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            return;
        }

        var fullPath = Path.Combine(parentPath, name);
        if (Directory.Exists(fullPath))
        {
            MessageBox.Show(
                $"A {folderType} with that name already exists in the selected location.",
                "Duplicate Folder",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            return;
        }

        try
        {
            FileSystemHelper.CreateDirectory(fullPath);
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"Unable to create the {folderType}.\n\n{ex.Message}",
                "Create Folder Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    private void CreateRepositoryFile(string repositoryPath, string fileName, string extension)
    {
        var filePath = Path.Combine(repositoryPath, fileName + extension);
        if (File.Exists(filePath))
        {
            MessageBox.Show(
                "A file with that name already exists in the selected repository.",
                "Duplicate File",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            return;
        }

        try
        {
            var createdPath = FileSystemHelper.CreateRepositoryFile(repositoryPath, fileName, extension);
            if (string.IsNullOrWhiteSpace(createdPath))
            {
                MessageBox.Show(
                    "Please choose a valid file type.",
                    "Invalid File Type",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            LoadFiles(repositoryPath);
            LoadSubjectTree(filePath);
            SelectFileInList(filePath);
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"Unable to create the file.\n\n{ex.Message}",
                "Create File Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    private void LoadRepositoryMetadata(string repositoryPath)
    {
        try
        {
            var metadata = EnsureMetadata(repositoryPath);
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

    private RepoMetadata EnsureMetadata(string repositoryPath)
    {
        var metadataPath = Path.Combine(repositoryPath, MetadataFileName);
        if (!File.Exists(metadataPath))
        {
            var metadata = new RepoMetadata
            {
                Deadline = DateTime.Today,
                DateAdded = DateTime.Today,
                Status = "in-progress"
            };

            SaveMetadata(repositoryPath, metadata);
            return metadata;
        }

        var json = File.ReadAllText(metadataPath);
        var metadataFromFile = JsonSerializer.Deserialize<RepoMetadata>(json, jsonOptions);
        if (metadataFromFile is null || !IsValidStatus(metadataFromFile.Status))
        {
            throw new InvalidOperationException("The repository metadata is invalid.");
        }

        return metadataFromFile;
    }

    private void SaveMetadata(string repositoryPath, RepoMetadata metadata)
    {
        if (!IsValidStatus(metadata.Status))
        {
            throw new InvalidOperationException("Metadata status is invalid.");
        }

        var metadataPath = Path.Combine(repositoryPath, MetadataFileName);
        File.WriteAllText(metadataPath, JsonSerializer.Serialize(metadata, jsonOptions));
    }

    private void LoadVersionHistory(string? filePath)
    {
        versionsListBox.Items.Clear();

        if (string.IsNullOrWhiteSpace(filePath) || !File.Exists(filePath))
        {
            UpdateHistoryUiState(null);
            return;
        }

        try
        {
            var historyFolder = VersionHelper.GetHistoryFolderPath(filePath);
            var logEntries = VersionHelper.ReadVersionLog(filePath, jsonOptions);
            var extension = Path.GetExtension(filePath);

            foreach (var version in logEntries.OrderByDescending(v => v.Version))
            {
                var snapshotPath = Path.Combine(historyFolder, $"v{version.Version}{extension}");
                if (File.Exists(snapshotPath))
                {
                    versionsListBox.Items.Add(new VersionListItem(snapshotPath, version));
                }
            }

            UpdateHistoryUiState(filePath);
        }
        catch (Exception ex)
        {
            UpdateHistoryUiState(null);
            MessageBox.Show(
                $"Unable to load file history.\n\n{ex.Message}",
                "History Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    private void PromptAndSaveVersion(string filePath)
    {
        if (!File.Exists(filePath))
        {
            return;
        }

        var comment = PromptDialog.ShowDialog(
            $"Enter a version comment for {Path.GetFileName(filePath)}:\n\nSelect Cancel to skip saving a snapshot.",
            "Save Version");

        if (string.IsNullOrWhiteSpace(comment))
        {
            return;
        }

        try
        {
            VersionHelper.SaveVersion(filePath, comment.Trim(), jsonOptions);

            if (string.Equals(selectedFilePath, filePath, StringComparison.OrdinalIgnoreCase))
            {
                LoadVersionHistory(filePath);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"Unable to save the file version.\n\n{ex.Message}",
                "Version Save Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

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

    private static FolderBrowserDialog CreateFolderBrowserDialog(string description)
    {
        return new FolderBrowserDialog
        {
            Description = description,
            UseDescriptionForTitle = true,
            ShowNewFolderButton = true
        };
    }

    private static void EnsureParentChainExpanded(TreeNode node)
    {
        var current = node.Parent;
        while (current is not null)
        {
            current.Expand();
            current = current.Parent;
        }
    }

    private static TreeNode? FindNodeByPath(TreeNodeCollection nodes, string path)
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

    private static bool IsValidStatus(string? status)
    {
        return !string.IsNullOrWhiteSpace(status)
            && ValidStatuses.Contains(status, StringComparer.OrdinalIgnoreCase);
    }

    private static bool IsSystemManagedFile(string filePath)
    {
        return string.Equals(Path.GetFileName(filePath), MetadataFileName, StringComparison.OrdinalIgnoreCase);
    }

    private enum NodeType
    {
        Subject,
        Repository,
        File
    }

    private sealed class NodeData
    {
        public NodeData(string path, NodeType nodeType)
        {
            Path = path;
            NodeType = nodeType;
        }

        public string Path { get; }

        public NodeType NodeType { get; }
    }

    private sealed class VersionListItem
    {
        public VersionListItem(string snapshotPath, FileVersion version)
        {
            SnapshotPath = snapshotPath;
            Version = version;
        }

        public string SnapshotPath { get; }

        public FileVersion Version { get; }

        public override string ToString()
        {
            return $"v{Version.Version} - {Version.Timestamp:yyyy-MM-dd HH:mm} - {Version.Comment}";
        }
    }
}
