namespace IskolRepository.Forms;

partial class MainForm
{
    private System.ComponentModel.IContainer components = null;
    private SplitContainer mainSplitContainer = null!;
    private TreeView repositoryTreeView = null!;
    private Panel hostPanel = null!;
    private StartupView _startupView = null!;
    private SubjectSelectionView _subjectSelectionView = null!;
    private Panel workspacePanel = null!;
    private SplitContainer contentSplitContainer = null!;
    private ListView filesListView = null!;
    private ColumnHeader fileNameColumn = null!;
    private ColumnHeader extensionColumn = null!;
    private Panel historyPanel = null!;
    private ListBox versionsListBox = null!;
    private Label historyCaptionLabel = null!;
    private Button revertButton = null!;
    private GroupBox metadataGroupBox = null!;
    private Button saveVersionButton = null!;
    private Button updateMetadataButton = null!;
    private ComboBox statusComboBox = null!;
    private Label statusLabel = null!;
    private DateTimePicker deadlineDateTimePicker = null!;
    private Label editDeadlineLabel = null!;
    private Label dateAddedValueLabel = null!;
    private Label dateAddedLabel = null!;
    private Label deadlineValueLabel = null!;
    private Label deadlineLabel = null!;
    private Panel toolbarHeaderPanel = null!;
    private Panel pathHeaderPanel = null!;
    private Button createFileButton = null!;
    private Button createSubrepositoryButton = null!;
    private Button createRepositoryButton = null!;
    private Button backToSubjectsButton = null!;
    private Label selectedPathValueLabel = null!;
    private Label selectedPathLabel = null!;
    private Label selectedSubjectValueLabel = null!;
    private Label selectedSubjectLabel = null!;
    private Label noRepositoryMessageLabel = null!;
    private Label noVersionsMessageLabel = null!;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
        mainSplitContainer = new SplitContainer();
        repositoryTreeView = new TreeView();
        hostPanel = new Panel();
        workspacePanel = new Panel();
        contentSplitContainer = new SplitContainer();
        noRepositoryMessageLabel = new Label();
        filesListView = new ListView();
        fileNameColumn = new ColumnHeader();
        extensionColumn = new ColumnHeader();
        historyPanel = new Panel();
        noVersionsMessageLabel = new Label();
        versionsListBox = new ListBox();
        historyCaptionLabel = new Label();
        revertButton = new Button();
        metadataGroupBox = new GroupBox();
        saveVersionButton = new Button();
        updateMetadataButton = new Button();
        statusComboBox = new ComboBox();
        statusLabel = new Label();
        deadlineDateTimePicker = new DateTimePicker();
        editDeadlineLabel = new Label();
        dateAddedValueLabel = new Label();
        dateAddedLabel = new Label();
        deadlineValueLabel = new Label();
        deadlineLabel = new Label();
        _subjectSelectionView = new SubjectSelectionView();
        _startupView = new StartupView();
        toolbarHeaderPanel = new Panel();
        backToSubjectsButton = new Button();
        createRepositoryButton = new Button();
        createSubrepositoryButton = new Button();
        createFileButton = new Button();
        pathHeaderPanel = new Panel();
        selectedPathLabel = new Label();
        selectedPathValueLabel = new Label();
        selectedSubjectValueLabel = new Label();
        selectedSubjectLabel = new Label();
        ((System.ComponentModel.ISupportInitialize)mainSplitContainer).BeginInit();
        mainSplitContainer.Panel1.SuspendLayout();
        mainSplitContainer.Panel2.SuspendLayout();
        mainSplitContainer.SuspendLayout();
        hostPanel.SuspendLayout();
        workspacePanel.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)contentSplitContainer).BeginInit();
        contentSplitContainer.Panel1.SuspendLayout();
        contentSplitContainer.Panel2.SuspendLayout();
        contentSplitContainer.SuspendLayout();
        historyPanel.SuspendLayout();
        metadataGroupBox.SuspendLayout();
        toolbarHeaderPanel.SuspendLayout();
        pathHeaderPanel.SuspendLayout();
        SuspendLayout();
        // 
        // mainSplitContainer
        // 
        mainSplitContainer.Dock = DockStyle.Fill;
        mainSplitContainer.Location = new Point(0, 0);
        mainSplitContainer.Name = "mainSplitContainer";
        // 
        // mainSplitContainer.Panel1
        // 
        mainSplitContainer.Panel1.Controls.Add(repositoryTreeView);
        mainSplitContainer.Panel1.Padding = new Padding(10);
        // 
        // mainSplitContainer.Panel2
        // 
        mainSplitContainer.Panel2.Controls.Add(hostPanel);
        mainSplitContainer.Panel2.Controls.Add(toolbarHeaderPanel);
        mainSplitContainer.Panel2.Controls.Add(pathHeaderPanel);
        mainSplitContainer.Panel2.Padding = new Padding(10);
        mainSplitContainer.Size = new Size(1320, 749);
        mainSplitContainer.SplitterDistance = 330;
        mainSplitContainer.TabIndex = 0;
        // 
        // repositoryTreeView
        // 
        repositoryTreeView.BackColor = Color.FromArgb(218, 215, 205);
        repositoryTreeView.Dock = DockStyle.Fill;
        repositoryTreeView.HideSelection = false;
        repositoryTreeView.Location = new Point(10, 10);
        repositoryTreeView.Name = "repositoryTreeView";
        repositoryTreeView.Size = new Size(310, 729);
        repositoryTreeView.TabIndex = 0;
        repositoryTreeView.AfterSelect += repositoryTreeView_AfterSelect;
        repositoryTreeView.NodeMouseDoubleClick += repositoryTreeView_NodeMouseDoubleClick;
        // 
        // hostPanel
        // 
        hostPanel.Controls.Add(workspacePanel);
        hostPanel.Controls.Add(_subjectSelectionView);
        hostPanel.Controls.Add(_startupView);
        hostPanel.Dock = DockStyle.Fill;
        hostPanel.Location = new Point(10, 102);
        hostPanel.Name = "hostPanel";
        hostPanel.Size = new Size(966, 637);
        hostPanel.TabIndex = 0;
        // 
        // workspacePanel
        // 
        workspacePanel.Controls.Add(contentSplitContainer);
        workspacePanel.Dock = DockStyle.Fill;
        workspacePanel.Location = new Point(0, 0);
        workspacePanel.Name = "workspacePanel";
        workspacePanel.Size = new Size(966, 637);
        workspacePanel.TabIndex = 2;
        // 
        // contentSplitContainer
        // 
        contentSplitContainer.Dock = DockStyle.Fill;
        contentSplitContainer.Location = new Point(0, 0);
        contentSplitContainer.Name = "contentSplitContainer";
        // 
        // contentSplitContainer.Panel1
        // 
        contentSplitContainer.Panel1.Controls.Add(noRepositoryMessageLabel);
        contentSplitContainer.Panel1.Controls.Add(filesListView);
        contentSplitContainer.Panel1.Padding = new Padding(0, 0, 8, 0);
        // 
        // contentSplitContainer.Panel2
        // 
        contentSplitContainer.Panel2.Controls.Add(historyPanel);
        contentSplitContainer.Panel2.Padding = new Padding(8, 0, 0, 0);
        contentSplitContainer.Size = new Size(966, 637);
        contentSplitContainer.SplitterDistance = 560;
        contentSplitContainer.TabIndex = 2;
        // 
        // noRepositoryMessageLabel
        // 
        noRepositoryMessageLabel.Dock = DockStyle.Fill;
        noRepositoryMessageLabel.Font = new Font("Segoe UI", 12F);
        noRepositoryMessageLabel.ForeColor = Color.FromArgb(58, 90, 64);
        noRepositoryMessageLabel.Location = new Point(0, 0);
        noRepositoryMessageLabel.Name = "noRepositoryMessageLabel";
        noRepositoryMessageLabel.Size = new Size(552, 637);
        noRepositoryMessageLabel.TabIndex = 1;
        noRepositoryMessageLabel.Text = "Please select an Activity Repository";
        noRepositoryMessageLabel.TextAlign = ContentAlignment.MiddleCenter;
        noRepositoryMessageLabel.Visible = false;
        // 
        // filesListView
        // 
        filesListView.BackColor = Color.FromArgb(58, 90, 64);
        filesListView.Columns.AddRange(new ColumnHeader[] { fileNameColumn, extensionColumn });
        filesListView.Dock = DockStyle.Fill;
        filesListView.FullRowSelect = true;
        filesListView.GridLines = true;
        filesListView.Location = new Point(0, 0);
        filesListView.MultiSelect = false;
        filesListView.Name = "filesListView";
        filesListView.Size = new Size(552, 637);
        filesListView.TabIndex = 0;
        filesListView.UseCompatibleStateImageBehavior = false;
        filesListView.View = View.Details;
        filesListView.SelectedIndexChanged += filesListView_SelectedIndexChanged;
        filesListView.DoubleClick += filesListView_DoubleClick;
        // 
        // fileNameColumn
        // 
        fileNameColumn.Text = "File Name";
        fileNameColumn.Width = 360;
        // 
        // extensionColumn
        // 
        extensionColumn.Text = "Extension";
        extensionColumn.Width = 120;
        // 
        // historyPanel
        // 
        historyPanel.Controls.Add(noVersionsMessageLabel);
        historyPanel.Controls.Add(versionsListBox);
        historyPanel.Controls.Add(historyCaptionLabel);
        historyPanel.Controls.Add(revertButton);
        historyPanel.Controls.Add(metadataGroupBox);
        historyPanel.Dock = DockStyle.Fill;
        historyPanel.Location = new Point(8, 0);
        historyPanel.Name = "historyPanel";
        historyPanel.Size = new Size(394, 637);
        historyPanel.TabIndex = 0;
        // 
        // noVersionsMessageLabel
        // 
        noVersionsMessageLabel.Dock = DockStyle.Fill;
        noVersionsMessageLabel.Font = new Font("Segoe UI", 12F);
        noVersionsMessageLabel.ForeColor = Color.FromArgb(120, 120, 120);
        noVersionsMessageLabel.Location = new Point(0, 120);
        noVersionsMessageLabel.Name = "noVersionsMessageLabel";
        noVersionsMessageLabel.Size = new Size(394, 457);
        noVersionsMessageLabel.TabIndex = 1;
        noVersionsMessageLabel.Text = "No versions available";
        noVersionsMessageLabel.TextAlign = ContentAlignment.MiddleCenter;
        noVersionsMessageLabel.Visible = false;
        // 
        // versionsListBox
        // 
        versionsListBox.Dock = DockStyle.Fill;
        versionsListBox.FormattingEnabled = true;
        versionsListBox.HorizontalScrollbar = true;
        versionsListBox.Location = new Point(0, 120);
        versionsListBox.Name = "versionsListBox";
        versionsListBox.Size = new Size(394, 457);
        versionsListBox.TabIndex = 1;
        versionsListBox.SelectedIndexChanged += versionsListBox_SelectedIndexChanged;
        // 
        // historyCaptionLabel
        // 
        historyCaptionLabel.Dock = DockStyle.Bottom;
        historyCaptionLabel.Location = new Point(0, 577);
        historyCaptionLabel.Name = "historyCaptionLabel";
        historyCaptionLabel.Padding = new Padding(0, 6, 0, 0);
        historyCaptionLabel.Size = new Size(394, 24);
        historyCaptionLabel.TabIndex = 2;
        historyCaptionLabel.Text = "Version History";
        // 
        // revertButton
        // 
        revertButton.Dock = DockStyle.Bottom;
        revertButton.Enabled = false;
        revertButton.Location = new Point(0, 601);
        revertButton.Name = "revertButton";
        revertButton.Size = new Size(394, 36);
        revertButton.TabIndex = 3;
        revertButton.Text = "Revert";
        revertButton.UseVisualStyleBackColor = true;
        revertButton.Click += revertButton_Click;
        // 
        // metadataGroupBox
        // 
        metadataGroupBox.Controls.Add(saveVersionButton);
        metadataGroupBox.Controls.Add(updateMetadataButton);
        metadataGroupBox.Controls.Add(statusComboBox);
        metadataGroupBox.Controls.Add(statusLabel);
        metadataGroupBox.Controls.Add(deadlineDateTimePicker);
        metadataGroupBox.Controls.Add(editDeadlineLabel);
        metadataGroupBox.Controls.Add(dateAddedValueLabel);
        metadataGroupBox.Controls.Add(dateAddedLabel);
        metadataGroupBox.Controls.Add(deadlineValueLabel);
        metadataGroupBox.Controls.Add(deadlineLabel);
        metadataGroupBox.Dock = DockStyle.Top;
        metadataGroupBox.Location = new Point(0, 0);
        metadataGroupBox.Name = "metadataGroupBox";
        metadataGroupBox.Size = new Size(394, 120);
        metadataGroupBox.TabIndex = 1;
        metadataGroupBox.TabStop = false;
        metadataGroupBox.Text = "Repository Metadata";
        // 
        // saveVersionButton
        // 
        saveVersionButton.Enabled = false;
        saveVersionButton.Location = new Point(288, 39);
        saveVersionButton.Name = "saveVersionButton";
        saveVersionButton.Size = new Size(100, 25);
        saveVersionButton.TabIndex = 8;
        saveVersionButton.Text = "Save Version";
        saveVersionButton.UseVisualStyleBackColor = true;
        saveVersionButton.Click += saveVersionButton_Click;
        // 
        // updateMetadataButton
        // 
        updateMetadataButton.Enabled = false;
        updateMetadataButton.Location = new Point(288, 77);
        updateMetadataButton.Name = "updateMetadataButton";
        updateMetadataButton.Size = new Size(100, 25);
        updateMetadataButton.TabIndex = 9;
        updateMetadataButton.Text = "Update";
        updateMetadataButton.UseVisualStyleBackColor = true;
        updateMetadataButton.Click += updateMetadataButton_Click;
        // 
        // statusComboBox
        // 
        statusComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
        statusComboBox.Enabled = false;
        statusComboBox.FormattingEnabled = true;
        statusComboBox.Items.AddRange(new object[] { "in-progress", "completed", "late" });
        statusComboBox.Location = new Point(180, 80);
        statusComboBox.Name = "statusComboBox";
        statusComboBox.Size = new Size(90, 23);
        statusComboBox.TabIndex = 7;
        // 
        // statusLabel
        // 
        statusLabel.Location = new Point(180, 65);
        statusLabel.Name = "statusLabel";
        statusLabel.Size = new Size(90, 15);
        statusLabel.TabIndex = 6;
        statusLabel.Text = "Status";
        // 
        // deadlineDateTimePicker
        // 
        deadlineDateTimePicker.Enabled = false;
        deadlineDateTimePicker.Format = DateTimePickerFormat.Short;
        deadlineDateTimePicker.Location = new Point(180, 38);
        deadlineDateTimePicker.Name = "deadlineDateTimePicker";
        deadlineDateTimePicker.Size = new Size(90, 23);
        deadlineDateTimePicker.TabIndex = 5;
        // 
        // editDeadlineLabel
        // 
        editDeadlineLabel.Location = new Point(180, 19);
        editDeadlineLabel.Name = "editDeadlineLabel";
        editDeadlineLabel.Size = new Size(90, 16);
        editDeadlineLabel.TabIndex = 4;
        editDeadlineLabel.Text = "Edit Deadline";
        // 
        // dateAddedValueLabel
        // 
        dateAddedValueLabel.BorderStyle = BorderStyle.FixedSingle;
        dateAddedValueLabel.Location = new Point(10, 82);
        dateAddedValueLabel.Name = "dateAddedValueLabel";
        dateAddedValueLabel.Padding = new Padding(3, 2, 3, 2);
        dateAddedValueLabel.Size = new Size(150, 20);
        dateAddedValueLabel.TabIndex = 3;
        dateAddedValueLabel.Text = "-";
        // 
        // dateAddedLabel
        // 
        dateAddedLabel.Location = new Point(10, 65);
        dateAddedLabel.Name = "dateAddedLabel";
        dateAddedLabel.Size = new Size(150, 15);
        dateAddedLabel.TabIndex = 2;
        dateAddedLabel.Text = "Date Added";
        // 
        // deadlineValueLabel
        // 
        deadlineValueLabel.BorderStyle = BorderStyle.FixedSingle;
        deadlineValueLabel.Location = new Point(10, 38);
        deadlineValueLabel.Name = "deadlineValueLabel";
        deadlineValueLabel.Padding = new Padding(3, 2, 3, 2);
        deadlineValueLabel.Size = new Size(150, 20);
        deadlineValueLabel.TabIndex = 1;
        deadlineValueLabel.Text = "-";
        // 
        // deadlineLabel
        // 
        deadlineLabel.Location = new Point(10, 20);
        deadlineLabel.Name = "deadlineLabel";
        deadlineLabel.Size = new Size(150, 15);
        deadlineLabel.TabIndex = 0;
        deadlineLabel.Text = "Deadline";
        // 
        // _subjectSelectionView
        // 
        _subjectSelectionView.Dock = DockStyle.Fill;
        _subjectSelectionView.Location = new Point(0, 0);
        _subjectSelectionView.Name = "_subjectSelectionView";
        _subjectSelectionView.Size = new Size(966, 637);
        _subjectSelectionView.TabIndex = 3;
        // 
        // _startupView
        // 
        _startupView.Location = new Point(0, 0);
        _startupView.Name = "_startupView";
        _startupView.Size = new Size(1450, 573);
        _startupView.TabIndex = 4;
        // 
        // toolbarHeaderPanel
        // 
        toolbarHeaderPanel.BackColor = Color.FromArgb(218, 215, 205);
        toolbarHeaderPanel.Controls.Add(backToSubjectsButton);
        toolbarHeaderPanel.Controls.Add(createRepositoryButton);
        toolbarHeaderPanel.Controls.Add(createSubrepositoryButton);
        toolbarHeaderPanel.Controls.Add(createFileButton);
        toolbarHeaderPanel.Dock = DockStyle.Top;
        toolbarHeaderPanel.Location = new Point(10, 52);
        toolbarHeaderPanel.Name = "toolbarHeaderPanel";
        toolbarHeaderPanel.Size = new Size(966, 50);
        toolbarHeaderPanel.TabIndex = 0;
        // 
        // backToSubjectsButton
        // 
        backToSubjectsButton.BackColor = Color.FromArgb(218, 215, 205);
        backToSubjectsButton.Image = (Image)resources.GetObject("backToSubjectsButton.Image");
        backToSubjectsButton.ImageAlign = ContentAlignment.MiddleLeft;
        backToSubjectsButton.Location = new Point(6, 6);
        backToSubjectsButton.Name = "backToSubjectsButton";
        backToSubjectsButton.Size = new Size(135, 32);
        backToSubjectsButton.TabIndex = 0;
        backToSubjectsButton.Text = "      Back to Subject";
        backToSubjectsButton.UseVisualStyleBackColor = false;
        backToSubjectsButton.Click += backToSubjectsButton_Click;
        // 
        // createRepositoryButton
        // 
        createRepositoryButton.BackColor = Color.FromArgb(218, 215, 205);
        createRepositoryButton.Image = (Image)resources.GetObject("createRepositoryButton.Image");
        createRepositoryButton.ImageAlign = ContentAlignment.MiddleLeft;
        createRepositoryButton.Location = new Point(143, 6);
        createRepositoryButton.Name = "createRepositoryButton";
        createRepositoryButton.Size = new Size(145, 32);
        createRepositoryButton.TabIndex = 1;
        createRepositoryButton.Text = "     Create Repository";
        createRepositoryButton.UseVisualStyleBackColor = false;
        createRepositoryButton.Click += createRepositoryButton_Click;
        // 
        // createSubrepositoryButton
        // 
        createSubrepositoryButton.BackColor = Color.FromArgb(218, 215, 205);
        createSubrepositoryButton.Enabled = false;
        createSubrepositoryButton.Image = (Image)resources.GetObject("createSubrepositoryButton.Image");
        createSubrepositoryButton.ImageAlign = ContentAlignment.MiddleLeft;
        createSubrepositoryButton.Location = new Point(295, 6);
        createSubrepositoryButton.Name = "createSubrepositoryButton";
        createSubrepositoryButton.Size = new Size(155, 32);
        createSubrepositoryButton.TabIndex = 2;
        createSubrepositoryButton.Text = "     Create Subrepository";
        createSubrepositoryButton.UseVisualStyleBackColor = false;
        createSubrepositoryButton.Click += createSubrepositoryButton_Click;
        // 
        // createFileButton
        // 
        createFileButton.BackColor = Color.FromArgb(218, 215, 205);
        createFileButton.Enabled = false;
        createFileButton.Image = (Image)resources.GetObject("createFileButton.Image");
        createFileButton.ImageAlign = ContentAlignment.MiddleLeft;
        createFileButton.Location = new Point(456, 6);
        createFileButton.Name = "createFileButton";
        createFileButton.Size = new Size(120, 32);
        createFileButton.TabIndex = 3;
        createFileButton.Text = "Create File";
        createFileButton.UseVisualStyleBackColor = false;
        createFileButton.Click += createFileButton_Click;
        // 
        // pathHeaderPanel
        // 
        pathHeaderPanel.BackColor = Color.FromArgb(218, 215, 205);
        pathHeaderPanel.Controls.Add(selectedPathLabel);
        pathHeaderPanel.Controls.Add(selectedPathValueLabel);
        pathHeaderPanel.Controls.Add(selectedSubjectValueLabel);
        pathHeaderPanel.Controls.Add(selectedSubjectLabel);
        pathHeaderPanel.Dock = DockStyle.Top;
        pathHeaderPanel.Location = new Point(10, 10);
        pathHeaderPanel.Name = "pathHeaderPanel";
        pathHeaderPanel.Size = new Size(966, 42);
        pathHeaderPanel.TabIndex = 1;
        // 
        // selectedPathLabel
        // 
        selectedPathLabel.Location = new Point(336, 22);
        selectedPathLabel.Name = "selectedPathLabel";
        selectedPathLabel.Size = new Size(120, 20);
        selectedPathLabel.TabIndex = 5;
        selectedPathLabel.Text = "Selected Path";
        // 
        // selectedPathValueLabel
        // 
        selectedPathValueLabel.Location = new Point(336, 6);
        selectedPathValueLabel.Name = "selectedPathValueLabel";
        selectedPathValueLabel.Size = new Size(430, 20);
        selectedPathValueLabel.TabIndex = 6;
        selectedPathValueLabel.Text = "No item selected";
        // 
        // selectedSubjectValueLabel
        // 
        selectedSubjectValueLabel.Location = new Point(6, 22);
        selectedSubjectValueLabel.Name = "selectedSubjectValueLabel";
        selectedSubjectValueLabel.Size = new Size(400, 20);
        selectedSubjectValueLabel.TabIndex = 4;
        selectedSubjectValueLabel.Text = "No subject selected";
        // 
        // selectedSubjectLabel
        // 
        selectedSubjectLabel.Location = new Point(6, 6);
        selectedSubjectLabel.Name = "selectedSubjectLabel";
        selectedSubjectLabel.Size = new Size(120, 20);
        selectedSubjectLabel.TabIndex = 3;
        selectedSubjectLabel.Text = "Current Subject";
        // 
        // MainForm
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1320, 749);
        Controls.Add(mainSplitContainer);
        MinimumSize = new Size(1100, 680);
        Name = "MainForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "IskolRepository";
        mainSplitContainer.Panel1.ResumeLayout(false);
        mainSplitContainer.Panel2.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)mainSplitContainer).EndInit();
        mainSplitContainer.ResumeLayout(false);
        hostPanel.ResumeLayout(false);
        workspacePanel.ResumeLayout(false);
        contentSplitContainer.Panel1.ResumeLayout(false);
        contentSplitContainer.Panel2.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)contentSplitContainer).EndInit();
        contentSplitContainer.ResumeLayout(false);
        historyPanel.ResumeLayout(false);
        metadataGroupBox.ResumeLayout(false);
        toolbarHeaderPanel.ResumeLayout(false);
        pathHeaderPanel.ResumeLayout(false);
        ResumeLayout(false);
    }

    // Add this method to the MainForm partial class to fix CS0103
    private void selectedPathValueLabel_Click(object? sender, EventArgs e)
    {
        // You can add logic here if needed, or leave it empty if not required
    }
}