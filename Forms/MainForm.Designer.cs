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
    private Panel topHeaderPanel = null!;
    private Label logoLabel = null!;
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
        topHeaderPanel = new Panel();
        logoLabel = new Label();
        selectedPathValueLabel = new Label();
        toolbarHeaderPanel = new Panel();
        selectedSubjectValueLabel = new Label();
        backToSubjectsButton = new Button();
        createRepositoryButton = new Button();
        createSubrepositoryButton = new Button();
        createFileButton = new Button();
        pathHeaderPanel = new Panel();
        selectedPathLabel = new Label();
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
        topHeaderPanel.SuspendLayout();
        toolbarHeaderPanel.SuspendLayout();
        SuspendLayout();
        // 
        // mainSplitContainer
        // 
        mainSplitContainer.Dock = DockStyle.Fill;
        mainSplitContainer.Location = new Point(0, 160);
        mainSplitContainer.Name = "mainSplitContainer";
        // 
        // mainSplitContainer.Panel1
        // 
        mainSplitContainer.Panel1.BackColor = Color.FromArgb(235, 240, 245);
        mainSplitContainer.Panel1.Controls.Add(repositoryTreeView);
        // 
        // mainSplitContainer.Panel2
        // 
        mainSplitContainer.Panel2.Controls.Add(hostPanel);
        mainSplitContainer.Size = new Size(1320, 589);
        mainSplitContainer.SplitterDistance = 319;
        mainSplitContainer.TabIndex = 0;
        // 
        // repositoryTreeView
        // 
        repositoryTreeView.BackColor = Color.FromArgb(235, 240, 245);
        repositoryTreeView.BorderStyle = BorderStyle.None;
        repositoryTreeView.Dock = DockStyle.Fill;
        repositoryTreeView.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        repositoryTreeView.ForeColor = Color.FromArgb(40, 55, 70);
        repositoryTreeView.HideSelection = false;
        repositoryTreeView.Indent = 20;
        repositoryTreeView.ItemHeight = 30;
        repositoryTreeView.Location = new Point(0, 0);
        repositoryTreeView.Name = "repositoryTreeView";
        repositoryTreeView.Size = new Size(319, 589);
        repositoryTreeView.TabIndex = 0;
        repositoryTreeView.AfterSelect += repositoryTreeView_AfterSelect;
        repositoryTreeView.NodeMouseDoubleClick += repositoryTreeView_NodeMouseDoubleClick;
        // 
        // hostPanel
        // 
        hostPanel.BackColor = Color.FromArgb(240, 245, 250);
        hostPanel.Controls.Add(workspacePanel);
        hostPanel.Controls.Add(_subjectSelectionView);
        hostPanel.Dock = DockStyle.Fill;
        hostPanel.Location = new Point(0, 0);
        hostPanel.Name = "hostPanel";
        hostPanel.Size = new Size(997, 589);
        hostPanel.TabIndex = 0;
        // 
        // workspacePanel
        // 
        workspacePanel.Controls.Add(contentSplitContainer);
        workspacePanel.Dock = DockStyle.Fill;
        workspacePanel.Location = new Point(0, 0);
        workspacePanel.Name = "workspacePanel";
        workspacePanel.Size = new Size(997, 589);
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
        contentSplitContainer.Panel1.BackColor = Color.White;
        contentSplitContainer.Panel1.Controls.Add(noRepositoryMessageLabel);
        contentSplitContainer.Panel1.Controls.Add(filesListView);
        contentSplitContainer.Panel1.Padding = new Padding(12);
        // 
        // contentSplitContainer.Panel2
        // 
        contentSplitContainer.Panel2.BackColor = Color.FromArgb(235, 240, 245);
        contentSplitContainer.Panel2.Controls.Add(historyPanel);
        contentSplitContainer.Panel2.Padding = new Padding(12);
        contentSplitContainer.Size = new Size(997, 589);
        contentSplitContainer.SplitterDistance = 600;
        contentSplitContainer.TabIndex = 2;
        // 
        // noRepositoryMessageLabel
        // 
        noRepositoryMessageLabel.BackColor = Color.White;
        noRepositoryMessageLabel.Dock = DockStyle.Fill;
        noRepositoryMessageLabel.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
        noRepositoryMessageLabel.ForeColor = Color.FromArgb(100, 115, 128);
        noRepositoryMessageLabel.Location = new Point(12, 12);
        noRepositoryMessageLabel.Name = "noRepositoryMessageLabel";
        noRepositoryMessageLabel.Size = new Size(576, 565);
        noRepositoryMessageLabel.TabIndex = 1;
        noRepositoryMessageLabel.Text = "Please select an Activity Repository";
        noRepositoryMessageLabel.TextAlign = ContentAlignment.MiddleCenter;
        noRepositoryMessageLabel.Visible = false;
        // 
        // filesListView
        // 
        filesListView.BackColor = Color.White;
        filesListView.BorderStyle = BorderStyle.None;
        filesListView.Columns.AddRange(new ColumnHeader[] { fileNameColumn, extensionColumn });
        filesListView.Dock = DockStyle.Fill;
        filesListView.Font = new Font("Segoe UI", 10F);
        filesListView.ForeColor = Color.FromArgb(40, 55, 70);
        filesListView.FullRowSelect = true;
        filesListView.HeaderStyle = ColumnHeaderStyle.Nonclickable;
        filesListView.Location = new Point(12, 12);
        filesListView.MultiSelect = false;
        filesListView.Name = "filesListView";
        filesListView.Size = new Size(576, 565);
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
        extensionColumn.Text = "Filetype";
        extensionColumn.Width = 150;
        // 
        // historyPanel
        // 
        historyPanel.BackColor = Color.FromArgb(235, 240, 245);
        historyPanel.Controls.Add(noVersionsMessageLabel);
        historyPanel.Controls.Add(versionsListBox);
        historyPanel.Controls.Add(historyCaptionLabel);
        historyPanel.Controls.Add(revertButton);
        historyPanel.Controls.Add(metadataGroupBox);
        historyPanel.Dock = DockStyle.Fill;
        historyPanel.Location = new Point(12, 12);
        historyPanel.Name = "historyPanel";
        historyPanel.Size = new Size(369, 565);
        historyPanel.TabIndex = 0;
        // 
        // noVersionsMessageLabel
        // 
        noVersionsMessageLabel.BackColor = Color.FromArgb(235, 240, 245);
        noVersionsMessageLabel.Dock = DockStyle.Fill;
        noVersionsMessageLabel.Font = new Font("Segoe UI", 10F);
        noVersionsMessageLabel.ForeColor = Color.FromArgb(100, 115, 128);
        noVersionsMessageLabel.Location = new Point(0, 220);
        noVersionsMessageLabel.Name = "noVersionsMessageLabel";
        noVersionsMessageLabel.Size = new Size(369, 301);
        noVersionsMessageLabel.TabIndex = 1;
        noVersionsMessageLabel.Text = "No versions available";
        noVersionsMessageLabel.TextAlign = ContentAlignment.MiddleCenter;
        noVersionsMessageLabel.Visible = false;
        // 
        // versionsListBox
        // 
        versionsListBox.BackColor = Color.White;
        versionsListBox.BorderStyle = BorderStyle.FixedSingle;
        versionsListBox.Dock = DockStyle.Fill;
        versionsListBox.Font = new Font("Segoe UI", 9F);
        versionsListBox.ForeColor = Color.FromArgb(40, 55, 70);
        versionsListBox.FormattingEnabled = true;
        versionsListBox.HorizontalScrollbar = true;
        versionsListBox.Location = new Point(0, 220);
        versionsListBox.Name = "versionsListBox";
        versionsListBox.Size = new Size(369, 301);
        versionsListBox.TabIndex = 1;
        versionsListBox.SelectedIndexChanged += versionsListBox_SelectedIndexChanged;
        // 
        // historyCaptionLabel
        // 
        historyCaptionLabel.BackColor = Color.FromArgb(220, 230, 235);
        historyCaptionLabel.Dock = DockStyle.Top;
        historyCaptionLabel.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
        historyCaptionLabel.ForeColor = Color.FromArgb(40, 55, 70);
        historyCaptionLabel.Location = new Point(0, 180);
        historyCaptionLabel.Name = "historyCaptionLabel";
        historyCaptionLabel.Size = new Size(369, 40);
        historyCaptionLabel.TabIndex = 2;
        historyCaptionLabel.Text = "Version History";
        historyCaptionLabel.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // revertButton
        // 
        revertButton.BackColor = Color.FromArgb(43, 87, 158);
        revertButton.Cursor = Cursors.Hand;
        revertButton.Dock = DockStyle.Bottom;
        revertButton.Enabled = false;
        revertButton.FlatAppearance.BorderSize = 0;
        revertButton.FlatAppearance.MouseOverBackColor = Color.FromArgb(57, 97, 163);
        revertButton.FlatStyle = FlatStyle.Flat;
        revertButton.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        revertButton.ForeColor = Color.White;
        revertButton.Image = (Image)resources.GetObject("revertButton.Image");
        revertButton.ImageAlign = ContentAlignment.MiddleLeft;
        revertButton.Location = new Point(0, 521);
        revertButton.Margin = new Padding(8);
        revertButton.Name = "revertButton";
        revertButton.RightToLeft = RightToLeft.No;
        revertButton.Size = new Size(369, 44);
        revertButton.TabIndex = 3;
        revertButton.Text = "Revert to Selected";
        revertButton.UseVisualStyleBackColor = false;
        revertButton.Click += revertButton_Click;
        // 
        // metadataGroupBox
        // 
        metadataGroupBox.BackColor = Color.White;
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
        metadataGroupBox.FlatStyle = FlatStyle.Flat;
        metadataGroupBox.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        metadataGroupBox.ForeColor = Color.FromArgb(40, 55, 70);
        metadataGroupBox.Location = new Point(0, 0);
        metadataGroupBox.Name = "metadataGroupBox";
        metadataGroupBox.Size = new Size(369, 180);
        metadataGroupBox.TabIndex = 1;
        metadataGroupBox.TabStop = false;
        metadataGroupBox.Text = "Repository Metadata";
        // 
        // saveVersionButton
        // 
        saveVersionButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        saveVersionButton.BackColor = Color.FromArgb(43, 87, 158);
        saveVersionButton.Cursor = Cursors.Hand;
        saveVersionButton.Enabled = false;
        saveVersionButton.FlatAppearance.BorderSize = 0;
        saveVersionButton.FlatAppearance.MouseOverBackColor = Color.FromArgb(57, 97, 163);
        saveVersionButton.FlatStyle = FlatStyle.Flat;
        saveVersionButton.Font = new Font("Segoe UI", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
        saveVersionButton.ForeColor = Color.White;
        saveVersionButton.Image = (Image)resources.GetObject("saveVersionButton.Image");
        saveVersionButton.ImageAlign = ContentAlignment.MiddleLeft;
        saveVersionButton.Location = new Point(226, 43);
        saveVersionButton.Name = "saveVersionButton";
        saveVersionButton.Size = new Size(137, 49);
        saveVersionButton.TabIndex = 8;
        saveVersionButton.Text = "Save New Version";
        saveVersionButton.TextAlign = ContentAlignment.MiddleRight;
        saveVersionButton.TextImageRelation = TextImageRelation.ImageBeforeText;
        saveVersionButton.UseVisualStyleBackColor = false;
        saveVersionButton.Click += saveVersionButton_Click;
        // 
        // updateMetadataButton
        // 
        updateMetadataButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        updateMetadataButton.BackColor = Color.FromArgb(43, 87, 158);
        updateMetadataButton.Cursor = Cursors.Hand;
        updateMetadataButton.Enabled = false;
        updateMetadataButton.FlatAppearance.BorderSize = 0;
        updateMetadataButton.FlatAppearance.MouseOverBackColor = Color.FromArgb(57, 97, 163);
        updateMetadataButton.FlatStyle = FlatStyle.Flat;
        updateMetadataButton.Font = new Font("Segoe UI", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
        updateMetadataButton.ForeColor = Color.White;
        updateMetadataButton.Image = (Image)resources.GetObject("updateMetadataButton.Image");
        updateMetadataButton.ImageAlign = ContentAlignment.MiddleLeft;
        updateMetadataButton.Location = new Point(226, 110);
        updateMetadataButton.Name = "updateMetadataButton";
        updateMetadataButton.Size = new Size(137, 43);
        updateMetadataButton.TabIndex = 9;
        updateMetadataButton.Text = "Update Metadata";
        updateMetadataButton.TextAlign = ContentAlignment.MiddleRight;
        updateMetadataButton.UseVisualStyleBackColor = false;
        updateMetadataButton.Click += updateMetadataButton_Click;
        // 
        // statusComboBox
        // 
        statusComboBox.Anchor = AnchorStyles.Top;
        statusComboBox.BackColor = Color.White;
        statusComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
        statusComboBox.Enabled = false;
        statusComboBox.FlatStyle = FlatStyle.Flat;
        statusComboBox.Font = new Font("Segoe UI", 9F);
        statusComboBox.ForeColor = Color.FromArgb(40, 55, 70);
        statusComboBox.FormattingEnabled = true;
        statusComboBox.Items.AddRange(new object[] { "in-progress", "completed", "submitted" });
        statusComboBox.Location = new Point(120, 130);
        statusComboBox.Name = "statusComboBox";
        statusComboBox.Size = new Size(100, 23);
        statusComboBox.TabIndex = 7;
        // 
        // statusLabel
        // 
        statusLabel.Anchor = AnchorStyles.Top;
        statusLabel.Font = new Font("Segoe UI", 9F);
        statusLabel.ForeColor = Color.FromArgb(100, 115, 128);
        statusLabel.Location = new Point(120, 110);
        statusLabel.Name = "statusLabel";
        statusLabel.Size = new Size(100, 20);
        statusLabel.TabIndex = 6;
        statusLabel.Text = "Status";
        // 
        // deadlineDateTimePicker
        // 
        deadlineDateTimePicker.Anchor = AnchorStyles.Top;
        deadlineDateTimePicker.CalendarForeColor = Color.FromArgb(40, 55, 70);
        deadlineDateTimePicker.Enabled = false;
        deadlineDateTimePicker.Font = new Font("Segoe UI", 9F);
        deadlineDateTimePicker.Format = DateTimePickerFormat.Short;
        deadlineDateTimePicker.Location = new Point(120, 55);
        deadlineDateTimePicker.Name = "deadlineDateTimePicker";
        deadlineDateTimePicker.Size = new Size(100, 23);
        deadlineDateTimePicker.TabIndex = 5;
        // 
        // editDeadlineLabel
        // 
        editDeadlineLabel.Anchor = AnchorStyles.Top;
        editDeadlineLabel.Font = new Font("Segoe UI", 9F);
        editDeadlineLabel.ForeColor = Color.FromArgb(100, 115, 128);
        editDeadlineLabel.Location = new Point(120, 35);
        editDeadlineLabel.Name = "editDeadlineLabel";
        editDeadlineLabel.Size = new Size(110, 20);
        editDeadlineLabel.TabIndex = 4;
        editDeadlineLabel.Text = "Edit Deadline";
        // 
        // dateAddedValueLabel
        // 
        dateAddedValueLabel.BackColor = Color.FromArgb(240, 245, 250);
        dateAddedValueLabel.BorderStyle = BorderStyle.FixedSingle;
        dateAddedValueLabel.Font = new Font("Segoe UI", 9F);
        dateAddedValueLabel.ForeColor = Color.FromArgb(40, 55, 70);
        dateAddedValueLabel.Location = new Point(10, 130);
        dateAddedValueLabel.Name = "dateAddedValueLabel";
        dateAddedValueLabel.Size = new Size(100, 23);
        dateAddedValueLabel.TabIndex = 3;
        dateAddedValueLabel.Text = "-";
        dateAddedValueLabel.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // dateAddedLabel
        // 
        dateAddedLabel.Font = new Font("Segoe UI", 9F);
        dateAddedLabel.ForeColor = Color.FromArgb(100, 115, 128);
        dateAddedLabel.Location = new Point(10, 110);
        dateAddedLabel.Name = "dateAddedLabel";
        dateAddedLabel.Size = new Size(100, 20);
        dateAddedLabel.TabIndex = 2;
        dateAddedLabel.Text = "Date Added";
        // 
        // deadlineValueLabel
        // 
        deadlineValueLabel.BackColor = Color.FromArgb(240, 245, 250);
        deadlineValueLabel.BorderStyle = BorderStyle.FixedSingle;
        deadlineValueLabel.Font = new Font("Segoe UI", 9F);
        deadlineValueLabel.ForeColor = Color.FromArgb(40, 55, 70);
        deadlineValueLabel.Location = new Point(10, 55);
        deadlineValueLabel.Name = "deadlineValueLabel";
        deadlineValueLabel.Size = new Size(100, 23);
        deadlineValueLabel.TabIndex = 1;
        deadlineValueLabel.Text = "-";
        deadlineValueLabel.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // deadlineLabel
        // 
        deadlineLabel.Font = new Font("Segoe UI", 9F);
        deadlineLabel.ForeColor = Color.FromArgb(100, 115, 128);
        deadlineLabel.Location = new Point(10, 35);
        deadlineLabel.Name = "deadlineLabel";
        deadlineLabel.Size = new Size(100, 20);
        deadlineLabel.TabIndex = 0;
        deadlineLabel.Text = "Deadline";
        // 
        // _subjectSelectionView
        // 
        _subjectSelectionView.BackColor = Color.FromArgb(240, 245, 250);
        _subjectSelectionView.Dock = DockStyle.Fill;
        _subjectSelectionView.Font = new Font("Segoe UI", 10F);
        _subjectSelectionView.Location = new Point(0, 0);
        _subjectSelectionView.Name = "_subjectSelectionView";
        _subjectSelectionView.Size = new Size(997, 589);
        _subjectSelectionView.TabIndex = 3;
        // 
        // topHeaderPanel
        // 
        topHeaderPanel.BackColor = Color.FromArgb(28, 77, 141);
        topHeaderPanel.Controls.Add(logoLabel);
        topHeaderPanel.Controls.Add(selectedPathValueLabel);
        topHeaderPanel.Dock = DockStyle.Top;
        topHeaderPanel.Location = new Point(0, 0);
        topHeaderPanel.Name = "topHeaderPanel";
        topHeaderPanel.Size = new Size(1320, 100);
        topHeaderPanel.TabIndex = 1;
        // 
        // logoLabel
        // 
        logoLabel.Font = new Font("Segoe UI", 28F, FontStyle.Bold);
        logoLabel.ForeColor = Color.White;
        logoLabel.Image = (Image)resources.GetObject("logoLabel.Image");
        logoLabel.ImageAlign = ContentAlignment.TopLeft;
        logoLabel.Location = new Point(36, 17);
        logoLabel.Name = "logoLabel";
        logoLabel.Size = new Size(259, 67);
        logoLabel.TabIndex = 0;
        logoLabel.Text = "IskolRepo";
        logoLabel.TextAlign = ContentAlignment.MiddleRight;
        // 
        // selectedPathValueLabel
        // 
        selectedPathValueLabel.AutoSize = true;
        selectedPathValueLabel.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
        selectedPathValueLabel.ForeColor = Color.FromArgb(235, 240, 245);
        selectedPathValueLabel.ImageAlign = ContentAlignment.MiddleLeft;
        selectedPathValueLabel.Location = new Point(335, 48);
        selectedPathValueLabel.Name = "selectedPathValueLabel";
        selectedPathValueLabel.Size = new Size(139, 21);
        selectedPathValueLabel.TabIndex = 1;
        selectedPathValueLabel.Text = "No item selected";
        selectedPathValueLabel.TextAlign = ContentAlignment.MiddleCenter;
        selectedPathValueLabel.Click += selectedPathValueLabel_Click;
        // 
        // toolbarHeaderPanel
        // 
        toolbarHeaderPanel.BackColor = Color.White;
        toolbarHeaderPanel.Controls.Add(selectedSubjectValueLabel);
        toolbarHeaderPanel.Controls.Add(backToSubjectsButton);
        toolbarHeaderPanel.Controls.Add(createRepositoryButton);
        toolbarHeaderPanel.Controls.Add(createSubrepositoryButton);
        toolbarHeaderPanel.Controls.Add(createFileButton);
        toolbarHeaderPanel.Dock = DockStyle.Top;
        toolbarHeaderPanel.Location = new Point(0, 100);
        toolbarHeaderPanel.Name = "toolbarHeaderPanel";
        toolbarHeaderPanel.Padding = new Padding(12);
        toolbarHeaderPanel.Size = new Size(1320, 60);
        toolbarHeaderPanel.TabIndex = 2;
        // 
        // selectedSubjectValueLabel
        // 
        selectedSubjectValueLabel.AutoSize = true;
        selectedSubjectValueLabel.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
        selectedSubjectValueLabel.ForeColor = Color.FromArgb(40, 55, 70);
        selectedSubjectValueLabel.Location = new Point(188, 12);
        selectedSubjectValueLabel.Name = "selectedSubjectValueLabel";
        selectedSubjectValueLabel.Size = new Size(237, 32);
        selectedSubjectValueLabel.TabIndex = 4;
        selectedSubjectValueLabel.Text = "No subject selected";
        // 
        // backToSubjectsButton
        // 
        backToSubjectsButton.BackColor = Color.FromArgb(43, 87, 158);
        backToSubjectsButton.Cursor = Cursors.Hand;
        backToSubjectsButton.FlatAppearance.BorderSize = 0;
        backToSubjectsButton.FlatAppearance.MouseOverBackColor = Color.FromArgb(57, 97, 163);
        backToSubjectsButton.FlatStyle = FlatStyle.Flat;
        backToSubjectsButton.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        backToSubjectsButton.ForeColor = Color.White;
        backToSubjectsButton.Image = (Image)resources.GetObject("backToSubjectsButton.Image");
        backToSubjectsButton.ImageAlign = ContentAlignment.MiddleLeft;
        backToSubjectsButton.Location = new Point(12, 10);
        backToSubjectsButton.Name = "backToSubjectsButton";
        backToSubjectsButton.Size = new Size(160, 40);
        backToSubjectsButton.TabIndex = 0;
        backToSubjectsButton.Text = "      Back to Subject";
        backToSubjectsButton.UseVisualStyleBackColor = false;
        backToSubjectsButton.Click += backToSubjectsButton_Click;
        // 
        // createRepositoryButton
        // 
        createRepositoryButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        createRepositoryButton.BackColor = Color.FromArgb(43, 87, 158);
        createRepositoryButton.Cursor = Cursors.Hand;
        createRepositoryButton.FlatAppearance.BorderSize = 0;
        createRepositoryButton.FlatAppearance.MouseOverBackColor = Color.FromArgb(57, 97, 163);
        createRepositoryButton.FlatStyle = FlatStyle.Flat;
        createRepositoryButton.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        createRepositoryButton.ForeColor = Color.White;
        createRepositoryButton.Image = (Image)resources.GetObject("createRepositoryButton.Image");
        createRepositoryButton.ImageAlign = ContentAlignment.MiddleLeft;
        createRepositoryButton.Location = new Point(791, 10);
        createRepositoryButton.Name = "createRepositoryButton";
        createRepositoryButton.Size = new Size(170, 40);
        createRepositoryButton.TabIndex = 1;
        createRepositoryButton.Text = "     Create Repository";
        createRepositoryButton.UseVisualStyleBackColor = false;
        createRepositoryButton.Click += createRepositoryButton_Click;
        // 
        // createSubrepositoryButton
        // 
        createSubrepositoryButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        createSubrepositoryButton.BackColor = Color.FromArgb(43, 87, 158);
        createSubrepositoryButton.Cursor = Cursors.Hand;
        createSubrepositoryButton.Enabled = false;
        createSubrepositoryButton.FlatAppearance.BorderSize = 0;
        createSubrepositoryButton.FlatAppearance.MouseOverBackColor = Color.FromArgb(57, 97, 163);
        createSubrepositoryButton.FlatStyle = FlatStyle.Flat;
        createSubrepositoryButton.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        createSubrepositoryButton.ForeColor = Color.White;
        createSubrepositoryButton.Image = (Image)resources.GetObject("createSubrepositoryButton.Image");
        createSubrepositoryButton.ImageAlign = ContentAlignment.MiddleLeft;
        createSubrepositoryButton.Location = new Point(967, 10);
        createSubrepositoryButton.Name = "createSubrepositoryButton";
        createSubrepositoryButton.Size = new Size(196, 40);
        createSubrepositoryButton.TabIndex = 2;
        createSubrepositoryButton.Text = "     Create Subrepository";
        createSubrepositoryButton.UseVisualStyleBackColor = false;
        createSubrepositoryButton.Click += createSubrepositoryButton_Click;
        // 
        // createFileButton
        // 
        createFileButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        createFileButton.BackColor = Color.FromArgb(43, 87, 158);
        createFileButton.Cursor = Cursors.Hand;
        createFileButton.Enabled = false;
        createFileButton.FlatAppearance.BorderSize = 0;
        createFileButton.FlatAppearance.MouseOverBackColor = Color.FromArgb(57, 97, 163);
        createFileButton.FlatStyle = FlatStyle.Flat;
        createFileButton.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        createFileButton.ForeColor = Color.White;
        createFileButton.Image = (Image)resources.GetObject("createFileButton.Image");
        createFileButton.ImageAlign = ContentAlignment.MiddleLeft;
        createFileButton.Location = new Point(1169, 10);
        createFileButton.Name = "createFileButton";
        createFileButton.Size = new Size(140, 40);
        createFileButton.TabIndex = 3;
        createFileButton.Text = "     Create File";
        createFileButton.UseVisualStyleBackColor = false;
        createFileButton.Click += createFileButton_Click;
        // 
        // pathHeaderPanel
        // 
        pathHeaderPanel.Location = new Point(0, 0);
        pathHeaderPanel.Name = "pathHeaderPanel";
        pathHeaderPanel.Size = new Size(200, 100);
        pathHeaderPanel.TabIndex = 0;
        pathHeaderPanel.Visible = false;
        // 
        // selectedPathLabel
        // 
        selectedPathLabel.Location = new Point(0, 0);
        selectedPathLabel.Name = "selectedPathLabel";
        selectedPathLabel.Size = new Size(100, 23);
        selectedPathLabel.TabIndex = 0;
        // 
        // selectedSubjectLabel
        // 
        selectedSubjectLabel.Location = new Point(0, 0);
        selectedSubjectLabel.Name = "selectedSubjectLabel";
        selectedSubjectLabel.Size = new Size(100, 23);
        selectedSubjectLabel.TabIndex = 0;
        // 
        // MainForm
        // 
        AutoScaleDimensions = new SizeF(7F, 17F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(240, 245, 250);
        ClientSize = new Size(1320, 749);
        Controls.Add(mainSplitContainer);
        Controls.Add(toolbarHeaderPanel);
        Controls.Add(topHeaderPanel);
        Font = new Font("Segoe UI", 10F);
        MinimumSize = new Size(1336, 749);
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
        topHeaderPanel.ResumeLayout(false);
        topHeaderPanel.PerformLayout();
        toolbarHeaderPanel.ResumeLayout(false);
        toolbarHeaderPanel.PerformLayout();
        ResumeLayout(false);
    }

    // Add this method to the MainForm partial class to fix CS0103
    private void selectedPathValueLabel_Click(object? sender, EventArgs e)
    {
        // You can add logic here if needed, or leave it empty if not required
    }
}