namespace IskolRepository.Forms;

partial class MainForm
{
    private System.ComponentModel.IContainer components = null;
    private SplitContainer mainSplitContainer = null!;
    private TreeView repositoryTreeView = null!;
    private Panel hostPanel = null!;
    private Panel startupPanel = null!;
    private Button openSemesterButton = null!;
    private Button newSemesterButton = null!;
    private Label startupSubtitleLabel = null!;
    private Label startupTitleLabel = null!;
    private Panel subjectSelectionPanel = null!;
    private FlowLayoutPanel subjectCardsPanel = null!;
    private Panel subjectHeaderPanel = null!;
    private Button changeSemesterButton = null!;
    private Button addSubjectButton = null!;
    private Label semesterPathValueLabel = null!;
    private Label semesterPathLabel = null!;
    private Label semesterNameValueLabel = null!;
    private Label semesterNameLabel = null!;
    private Panel workspacePanel = null!;
    private SplitContainer contentSplitContainer = null!;
    private ListView filesListView = null!;
    private ColumnHeader fileNameColumn = null!;
    private ColumnHeader extensionColumn = null!;
    private Panel rightSidePanel = null!;
    private Panel historyPanel = null!;
    private ListBox versionsListBox = null!;
    private Label historyCaptionLabel = null!;
    private Button revertButton = null!;
    private Panel filePreviewPanel = null!;
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
    private Panel workspaceHeaderPanel = null!;
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
        rightSidePanel = new Panel();
        historyPanel = new Panel();
        noVersionsMessageLabel = new Label();
        versionsListBox = new ListBox();
        historyCaptionLabel = new Label();
        revertButton = new Button();
        filePreviewPanel = new Panel();
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
        workspaceHeaderPanel = new Panel();
        createFileButton = new Button();
        createSubrepositoryButton = new Button();
        createRepositoryButton = new Button();
        backToSubjectsButton = new Button();
        selectedPathValueLabel = new Label();
        selectedPathLabel = new Label();
        selectedSubjectValueLabel = new Label();
        selectedSubjectLabel = new Label();
        subjectSelectionPanel = new Panel();
        subjectCardsPanel = new FlowLayoutPanel();
        subjectHeaderPanel = new Panel();
        changeSemesterButton = new Button();
        addSubjectButton = new Button();
        semesterPathValueLabel = new Label();
        semesterPathLabel = new Label();
        semesterNameValueLabel = new Label();
        semesterNameLabel = new Label();
        startupPanel = new Panel();
        openSemesterButton = new Button();
        newSemesterButton = new Button();
        startupSubtitleLabel = new Label();
        startupTitleLabel = new Label();
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
        rightSidePanel.SuspendLayout();
        historyPanel.SuspendLayout();
        metadataGroupBox.SuspendLayout();
        workspaceHeaderPanel.SuspendLayout();
        subjectSelectionPanel.SuspendLayout();
        subjectHeaderPanel.SuspendLayout();
        startupPanel.SuspendLayout();
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
        hostPanel.Controls.Add(subjectSelectionPanel);
        hostPanel.Controls.Add(startupPanel);
        hostPanel.Dock = DockStyle.Fill;
        hostPanel.Location = new Point(10, 10);
        hostPanel.Name = "hostPanel";
        hostPanel.Size = new Size(966, 729);
        hostPanel.TabIndex = 0;
        // 
        // workspacePanel
        // 
        workspacePanel.Controls.Add(contentSplitContainer);
        workspacePanel.Controls.Add(workspaceHeaderPanel);
        workspacePanel.Dock = DockStyle.Fill;
        workspacePanel.Location = new Point(0, 0);
        workspacePanel.Name = "workspacePanel";
        workspacePanel.Size = new Size(966, 729);
        workspacePanel.TabIndex = 2;
        // 
        // contentSplitContainer
        // 
        contentSplitContainer.Dock = DockStyle.Fill;
        contentSplitContainer.Location = new Point(0, 88);
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
        contentSplitContainer.Panel2.Controls.Add(rightSidePanel);
        contentSplitContainer.Panel2.Padding = new Padding(8, 0, 0, 0);
        contentSplitContainer.Size = new Size(966, 641);
        contentSplitContainer.SplitterDistance = 477;
        contentSplitContainer.TabIndex = 2;
        // 
        // noRepositoryMessageLabel
        // 
        noRepositoryMessageLabel.Dock = DockStyle.Fill;
        noRepositoryMessageLabel.Font = new Font("Segoe UI", 12F);
        noRepositoryMessageLabel.ForeColor = Color.FromArgb(58, 90, 64);
        noRepositoryMessageLabel.Location = new Point(0, 0);
        noRepositoryMessageLabel.Name = "noRepositoryMessageLabel";
        noRepositoryMessageLabel.Size = new Size(469, 641);
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
        filesListView.Size = new Size(469, 641);
        filesListView.TabIndex = 0;
        filesListView.UseCompatibleStateImageBehavior = false;
        filesListView.View = View.Details;
        filesListView.SelectedIndexChanged += filesListView_SelectedIndexChanged;
        filesListView.DoubleClick += filesListView_DoubleClick;
        // 
        // fileNameColumn
        // 
        fileNameColumn.Text = "File Name";
        fileNameColumn.Width = 400;
        // 
        // extensionColumn
        // 
        extensionColumn.Text = "Extension";
        extensionColumn.Width = 140;
        // 
        // rightSidePanel
        // 
        rightSidePanel.Controls.Add(historyPanel);
        rightSidePanel.Controls.Add(filePreviewPanel);
        rightSidePanel.Controls.Add(metadataGroupBox);
        rightSidePanel.Dock = DockStyle.Fill;
        rightSidePanel.Location = new Point(8, 0);
        rightSidePanel.Name = "rightSidePanel";
        rightSidePanel.Size = new Size(477, 641);
        rightSidePanel.TabIndex = 0;
        // 
        // historyPanel
        // 
        historyPanel.Controls.Add(noVersionsMessageLabel);
        historyPanel.Controls.Add(versionsListBox);
        historyPanel.Controls.Add(historyCaptionLabel);
        historyPanel.Controls.Add(revertButton);
        historyPanel.Dock = DockStyle.Bottom;
        historyPanel.Location = new Point(0, 581);
        historyPanel.Name = "historyPanel";
        historyPanel.Size = new Size(477, 60);
        historyPanel.TabIndex = 2;
        // 
        // noVersionsMessageLabel
        // 
        noVersionsMessageLabel.Dock = DockStyle.Fill;
        noVersionsMessageLabel.Font = new Font("Segoe UI", 12F);
        noVersionsMessageLabel.ForeColor = Color.FromArgb(120, 120, 120);
        noVersionsMessageLabel.Location = new Point(0, 24);
        noVersionsMessageLabel.Name = "noVersionsMessageLabel";
        noVersionsMessageLabel.Size = new Size(477, 0);
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
        versionsListBox.Location = new Point(0, 24);
        versionsListBox.Name = "versionsListBox";
        versionsListBox.Size = new Size(477, 0);
        versionsListBox.TabIndex = 1;
        versionsListBox.SelectedIndexChanged += versionsListBox_SelectedIndexChanged;
        // 
        // historyCaptionLabel
        // 
        historyCaptionLabel.Dock = DockStyle.Top;
        historyCaptionLabel.Location = new Point(0, 0);
        historyCaptionLabel.Name = "historyCaptionLabel";
        historyCaptionLabel.Padding = new Padding(0, 4, 0, 0);
        historyCaptionLabel.Size = new Size(477, 24);
        historyCaptionLabel.TabIndex = 2;
        historyCaptionLabel.Text = "Version History";
        // 
        // revertButton
        // 
        revertButton.Dock = DockStyle.Bottom;
        revertButton.Enabled = false;
        revertButton.Location = new Point(0, 24);
        revertButton.Name = "revertButton";
        revertButton.Size = new Size(477, 36);
        revertButton.TabIndex = 3;
        revertButton.Text = "Revert";
        revertButton.UseVisualStyleBackColor = true;
        revertButton.Click += revertButton_Click;
        // 
        // filePreviewPanel
        // 
        filePreviewPanel.BorderStyle = BorderStyle.FixedSingle;
        filePreviewPanel.Dock = DockStyle.Fill;
        filePreviewPanel.Location = new Point(0, 160);
        filePreviewPanel.Name = "filePreviewPanel";
        filePreviewPanel.Size = new Size(477, 481);
        filePreviewPanel.TabIndex = 1;
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
        metadataGroupBox.Size = new Size(477, 160);
        metadataGroupBox.TabIndex = 0;
        metadataGroupBox.TabStop = false;
        metadataGroupBox.Text = "Repository Metadata";
        // 
        // saveVersionButton
        // 
        saveVersionButton.Enabled = false;
        saveVersionButton.Location = new Point(300, 40);
        saveVersionButton.Name = "saveVersionButton";
        saveVersionButton.Size = new Size(94, 23);
        saveVersionButton.TabIndex = 8;
        saveVersionButton.Text = "Select a file to";
        saveVersionButton.UseVisualStyleBackColor = true;
        saveVersionButton.Click += saveVersionButton_Click;
        // 
        // updateMetadataButton
        // 
        updateMetadataButton.Enabled = false;
        updateMetadataButton.Location = new Point(300, 94);
        updateMetadataButton.Name = "updateMetadataButton";
        updateMetadataButton.Size = new Size(94, 23);
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
        statusComboBox.Location = new Point(172, 94);
        statusComboBox.Name = "statusComboBox";
        statusComboBox.Size = new Size(120, 23);
        statusComboBox.TabIndex = 7;
        // 
        // statusLabel
        // 
        statusLabel.Location = new Point(172, 76);
        statusLabel.Name = "statusLabel";
        statusLabel.Size = new Size(64, 20);
        statusLabel.TabIndex = 6;
        statusLabel.Text = "Status";
        // 
        // deadlineDateTimePicker
        // 
        deadlineDateTimePicker.Enabled = false;
        deadlineDateTimePicker.Format = DateTimePickerFormat.Short;
        deadlineDateTimePicker.Location = new Point(172, 40);
        deadlineDateTimePicker.Name = "deadlineDateTimePicker";
        deadlineDateTimePicker.Size = new Size(120, 23);
        deadlineDateTimePicker.TabIndex = 5;
        // 
        // editDeadlineLabel
        // 
        editDeadlineLabel.Location = new Point(172, 22);
        editDeadlineLabel.Name = "editDeadlineLabel";
        editDeadlineLabel.Size = new Size(82, 20);
        editDeadlineLabel.TabIndex = 4;
        editDeadlineLabel.Text = "Edit Deadline";
        // 
        // dateAddedValueLabel
        // 
        dateAddedValueLabel.BorderStyle = BorderStyle.FixedSingle;
        dateAddedValueLabel.Location = new Point(8, 94);
        dateAddedValueLabel.Name = "dateAddedValueLabel";
        dateAddedValueLabel.Padding = new Padding(4, 3, 4, 3);
        dateAddedValueLabel.Size = new Size(150, 23);
        dateAddedValueLabel.TabIndex = 3;
        dateAddedValueLabel.Text = "-";
        // 
        // dateAddedLabel
        // 
        dateAddedLabel.Location = new Point(8, 76);
        dateAddedLabel.Name = "dateAddedLabel";
        dateAddedLabel.Size = new Size(102, 20);
        dateAddedLabel.TabIndex = 2;
        dateAddedLabel.Text = "Date Added";
        // 
        // deadlineValueLabel
        // 
        deadlineValueLabel.BorderStyle = BorderStyle.FixedSingle;
        deadlineValueLabel.Location = new Point(8, 40);
        deadlineValueLabel.Name = "deadlineValueLabel";
        deadlineValueLabel.Padding = new Padding(4, 3, 4, 3);
        deadlineValueLabel.Size = new Size(150, 23);
        deadlineValueLabel.TabIndex = 1;
        deadlineValueLabel.Text = "-";
        // 
        // deadlineLabel
        // 
        deadlineLabel.Location = new Point(8, 22);
        deadlineLabel.Name = "deadlineLabel";
        deadlineLabel.Size = new Size(79, 20);
        deadlineLabel.TabIndex = 0;
        deadlineLabel.Text = "Deadline";
        // 
        // workspaceHeaderPanel
        // 
        workspaceHeaderPanel.BackColor = Color.FromArgb(218, 215, 205);
        workspaceHeaderPanel.Controls.Add(createFileButton);
        workspaceHeaderPanel.Controls.Add(createSubrepositoryButton);
        workspaceHeaderPanel.Controls.Add(createRepositoryButton);
        workspaceHeaderPanel.Controls.Add(backToSubjectsButton);
        workspaceHeaderPanel.Controls.Add(selectedPathValueLabel);
        workspaceHeaderPanel.Controls.Add(selectedPathLabel);
        workspaceHeaderPanel.Controls.Add(selectedSubjectValueLabel);
        workspaceHeaderPanel.Controls.Add(selectedSubjectLabel);
        workspaceHeaderPanel.Dock = DockStyle.Top;
        workspaceHeaderPanel.Location = new Point(0, 0);
        workspaceHeaderPanel.Name = "workspaceHeaderPanel";
        workspaceHeaderPanel.Size = new Size(966, 88);
        workspaceHeaderPanel.TabIndex = 0;
        // 
        // createFileButton
        // 
        createFileButton.BackColor = Color.FromArgb(218, 215, 205);
        createFileButton.Enabled = false;
        createFileButton.Image = (Image)resources.GetObject("createFileButton.Image");
        createFileButton.ImageAlign = ContentAlignment.MiddleLeft;
        createFileButton.Location = new Point(467, 9);
        createFileButton.Margin = new Padding(0);
        createFileButton.Name = "createFileButton";
        createFileButton.Size = new Size(120, 32);
        createFileButton.TabIndex = 3;
        createFileButton.Text = "Create File";
        createFileButton.UseVisualStyleBackColor = false;
        createFileButton.Click += createFileButton_Click;
        // 
        // createSubrepositoryButton
        // 
        createSubrepositoryButton.BackColor = Color.FromArgb(218, 215, 205);
        createSubrepositoryButton.Enabled = false;
        createSubrepositoryButton.Image = (Image)resources.GetObject("createSubrepositoryButton.Image");
        createSubrepositoryButton.ImageAlign = ContentAlignment.MiddleLeft;
        createSubrepositoryButton.Location = new Point(307, 9);
        createSubrepositoryButton.Margin = new Padding(0);
        createSubrepositoryButton.Name = "createSubrepositoryButton";
        createSubrepositoryButton.Size = new Size(154, 32);
        createSubrepositoryButton.TabIndex = 2;
        createSubrepositoryButton.Text = "     Create Subrepository";
        createSubrepositoryButton.UseVisualStyleBackColor = false;
        createSubrepositoryButton.Click += createSubrepositoryButton_Click;
        // 
        // createRepositoryButton
        // 
        createRepositoryButton.BackColor = Color.FromArgb(218, 215, 205);
        createRepositoryButton.Image = (Image)resources.GetObject("createRepositoryButton.Image");
        createRepositoryButton.ImageAlign = ContentAlignment.MiddleLeft;
        createRepositoryButton.Location = new Point(156, 9);
        createRepositoryButton.Name = "createRepositoryButton";
        createRepositoryButton.Size = new Size(145, 32);
        createRepositoryButton.TabIndex = 1;
        createRepositoryButton.Text = "     Create Repository";
        createRepositoryButton.UseVisualStyleBackColor = false;
        createRepositoryButton.Click += createRepositoryButton_Click;
        // 
        // backToSubjectsButton
        // 
        backToSubjectsButton.BackColor = Color.FromArgb(218, 215, 205);
        backToSubjectsButton.Image = (Image)resources.GetObject("backToSubjectsButton.Image");
        backToSubjectsButton.ImageAlign = ContentAlignment.MiddleLeft;
        backToSubjectsButton.Location = new Point(9, 9);
        backToSubjectsButton.Name = "backToSubjectsButton";
        backToSubjectsButton.Size = new Size(141, 32);
        backToSubjectsButton.TabIndex = 0;
        backToSubjectsButton.Text = "      Back to Subject";
        backToSubjectsButton.UseVisualStyleBackColor = false;
        backToSubjectsButton.Click += backToSubjectsButton_Click;
        // 
        // selectedPathValueLabel
        // 
        selectedPathValueLabel.Location = new Point(326, 65);
        selectedPathValueLabel.Name = "selectedPathValueLabel";
        selectedPathValueLabel.Size = new Size(430, 20);
        selectedPathValueLabel.TabIndex = 6;
        selectedPathValueLabel.Text = "No item selected";
        // 
        // selectedPathLabel
        // 
        selectedPathLabel.Location = new Point(326, 48);
        selectedPathLabel.Name = "selectedPathLabel";
        selectedPathLabel.Size = new Size(120, 20);
        selectedPathLabel.TabIndex = 5;
        selectedPathLabel.Text = "Selected Path";
        // 
        // selectedSubjectValueLabel
        // 
        selectedSubjectValueLabel.Location = new Point(0, 65);
        selectedSubjectValueLabel.Name = "selectedSubjectValueLabel";
        selectedSubjectValueLabel.Size = new Size(400, 20);
        selectedSubjectValueLabel.TabIndex = 4;
        selectedSubjectValueLabel.Text = "No subject selected";
        // 
        // selectedSubjectLabel
        // 
        selectedSubjectLabel.Location = new Point(0, 48);
        selectedSubjectLabel.Name = "selectedSubjectLabel";
        selectedSubjectLabel.Size = new Size(120, 20);
        selectedSubjectLabel.TabIndex = 3;
        selectedSubjectLabel.Text = "Current Subject";
        // 
        // subjectSelectionPanel
        // 
        subjectSelectionPanel.Controls.Add(subjectCardsPanel);
        subjectSelectionPanel.Controls.Add(subjectHeaderPanel);
        subjectSelectionPanel.Dock = DockStyle.Fill;
        subjectSelectionPanel.Location = new Point(0, 0);
        subjectSelectionPanel.Name = "subjectSelectionPanel";
        subjectSelectionPanel.Size = new Size(966, 729);
        subjectSelectionPanel.TabIndex = 1;
        // 
        // subjectCardsPanel
        // 
        subjectCardsPanel.AutoScroll = true;
        subjectCardsPanel.Dock = DockStyle.Fill;
        subjectCardsPanel.Location = new Point(0, 120);
        subjectCardsPanel.Name = "subjectCardsPanel";
        subjectCardsPanel.Padding = new Padding(0, 8, 0, 0);
        subjectCardsPanel.Size = new Size(966, 609);
        subjectCardsPanel.TabIndex = 1;
        // 
        // subjectHeaderPanel
        // 
        subjectHeaderPanel.Controls.Add(changeSemesterButton);
        subjectHeaderPanel.Controls.Add(addSubjectButton);
        subjectHeaderPanel.Controls.Add(semesterPathValueLabel);
        subjectHeaderPanel.Controls.Add(semesterPathLabel);
        subjectHeaderPanel.Controls.Add(semesterNameValueLabel);
        subjectHeaderPanel.Controls.Add(semesterNameLabel);
        subjectHeaderPanel.Dock = DockStyle.Top;
        subjectHeaderPanel.Location = new Point(0, 0);
        subjectHeaderPanel.Name = "subjectHeaderPanel";
        subjectHeaderPanel.Size = new Size(966, 120);
        subjectHeaderPanel.TabIndex = 0;
        // 
        // changeSemesterButton
        // 
        changeSemesterButton.Location = new Point(156, 14);
        changeSemesterButton.Name = "changeSemesterButton";
        changeSemesterButton.Size = new Size(138, 32);
        changeSemesterButton.TabIndex = 1;
        changeSemesterButton.Text = "Change Semester";
        changeSemesterButton.UseVisualStyleBackColor = true;
        changeSemesterButton.Click += changeSemesterButton_Click;
        // 
        // addSubjectButton
        // 
        addSubjectButton.Location = new Point(0, 14);
        addSubjectButton.Name = "addSubjectButton";
        addSubjectButton.Size = new Size(138, 32);
        addSubjectButton.TabIndex = 0;
        addSubjectButton.Text = "Add Subject";
        addSubjectButton.UseVisualStyleBackColor = true;
        addSubjectButton.Click += addSubjectButton_Click;
        // 
        // semesterPathValueLabel
        // 
        semesterPathValueLabel.Location = new Point(0, 88);
        semesterPathValueLabel.Name = "semesterPathValueLabel";
        semesterPathValueLabel.Size = new Size(930, 20);
        semesterPathValueLabel.TabIndex = 5;
        semesterPathValueLabel.Text = "-";
        // 
        // semesterPathLabel
        // 
        semesterPathLabel.Location = new Point(0, 68);
        semesterPathLabel.Name = "semesterPathLabel";
        semesterPathLabel.Size = new Size(140, 20);
        semesterPathLabel.TabIndex = 4;
        semesterPathLabel.Text = "Semester Path";
        // 
        // semesterNameValueLabel
        // 
        semesterNameValueLabel.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
        semesterNameValueLabel.Location = new Point(0, 44);
        semesterNameValueLabel.Name = "semesterNameValueLabel";
        semesterNameValueLabel.Size = new Size(320, 24);
        semesterNameValueLabel.TabIndex = 3;
        semesterNameValueLabel.Text = "-";
        // 
        // semesterNameLabel
        // 
        semesterNameLabel.Location = new Point(0, 24);
        semesterNameLabel.Name = "semesterNameLabel";
        semesterNameLabel.Size = new Size(140, 20);
        semesterNameLabel.TabIndex = 2;
        semesterNameLabel.Text = "Active Semester";
        // 
        // startupPanel
        // 
        startupPanel.Controls.Add(openSemesterButton);
        startupPanel.Controls.Add(newSemesterButton);
        startupPanel.Controls.Add(startupSubtitleLabel);
        startupPanel.Controls.Add(startupTitleLabel);
        startupPanel.Dock = DockStyle.Fill;
        startupPanel.Location = new Point(0, 0);
        startupPanel.Name = "startupPanel";
        startupPanel.Size = new Size(966, 729);
        startupPanel.TabIndex = 0;
        // 
        // openSemesterButton
        // 
        openSemesterButton.Location = new Point(40, 170);
        openSemesterButton.Name = "openSemesterButton";
        openSemesterButton.Size = new Size(170, 44);
        openSemesterButton.TabIndex = 2;
        openSemesterButton.Text = "Open Semester";
        openSemesterButton.UseVisualStyleBackColor = true;
        openSemesterButton.Click += openSemesterButton_Click;
        // 
        // newSemesterButton
        // 
        newSemesterButton.Location = new Point(226, 170);
        newSemesterButton.Name = "newSemesterButton";
        newSemesterButton.Size = new Size(170, 44);
        newSemesterButton.TabIndex = 3;
        newSemesterButton.Text = "New Semester";
        newSemesterButton.UseVisualStyleBackColor = true;
        newSemesterButton.Click += newSemesterButton_Click;
        // 
        // startupSubtitleLabel
        // 
        startupSubtitleLabel.Location = new Point(40, 90);
        startupSubtitleLabel.Name = "startupSubtitleLabel";
        startupSubtitleLabel.Size = new Size(560, 48);
        startupSubtitleLabel.TabIndex = 1;
        startupSubtitleLabel.Text = "Open an existing semester folder or create a new empty semester workspace to start organizing subjects, repositories, files, and versions.";
        // 
        // startupTitleLabel
        // 
        startupTitleLabel.Font = new Font("Segoe UI", 22F, FontStyle.Bold);
        startupTitleLabel.Location = new Point(32, 32);
        startupTitleLabel.Name = "startupTitleLabel";
        startupTitleLabel.Size = new Size(420, 48);
        startupTitleLabel.TabIndex = 0;
        startupTitleLabel.Text = "IskolRepository";
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
        rightSidePanel.ResumeLayout(false);
        historyPanel.ResumeLayout(false);
        metadataGroupBox.ResumeLayout(false);
        workspaceHeaderPanel.ResumeLayout(false);
        subjectSelectionPanel.ResumeLayout(false);
        subjectHeaderPanel.ResumeLayout(false);
        startupPanel.ResumeLayout(false);
        ResumeLayout(false);
    }
}
