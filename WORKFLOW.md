# IskolRepository Application Workflow

## Overview
IskolRepository is a Windows Forms-based application that manages repositories of academic subjects organized by semesters. It allows users to create semesters, add subjects, create repositories within those subjects, manage files and versions within repositories, and track metadata (deadlines, status).

---

## Application Lifecycle

### 1. **Application Startup**
```
Program.Main()
  ↓
ApplicationConfiguration.Initialize()
  ↓ Initializes WinForms with high DPI settings
ServiceFactory.CreateServices()
  ↓ Creates and configures all dependency-injected services
    ├─ Infrastructure Services (FileSystemHelper, PathProvider, ValidationHelper)
    ├─ Domain Services (SemesterDomain, SubjectDomain, RepositoryDomain, FileDomain, etc.)
    └─ Application Services (SemesterApplicationService, RepositoryApplicationService, etc.)
  ↓
new MainForm(services)
  ↓ Constructor receives ApplicationServices container
  ├─ Initializes form components (InitializeComponent())
  ├─ Stores injected service references for later use
  └─ Calls ShowStartupView()
      ↓ Displays startup panel with semester selection options
  ↓
Application.Run(mainForm)
  ↓ Launches the message loop
```

**Key Classes Involved:**
- `Program.Main()` - Entry point
- `ServiceFactory.CreateServices()` - DI configuration
- `MainForm` - Main UI container

---

## Core Workflows

### 2. **Semester Management**

#### 2.1 Create New Semester
```
newSemesterButton_Click(sender, e)
  ↓ User clicks "New Semester" button
CreateFolderBrowserDialog("Select where to create semester")
  ↓ Dialog shown to user
User selects parent directory
  ↓
PromptDialog.ShowDialog("Enter semester name")
  ↓ Dialog prompts for semester name
User enters name
  ↓
_validationService.IsValidName(semesterName)
  ├─ TRUE: Create directory
  ├─ FALSE: Show error, return
  ↓
Directory.CreateDirectory(targetPath)
  ↓
_semesterService.CreateSemesterMarker(targetPath)
  └─ SemesterDomainService.CreateSemesterMarker()
    └─ Creates .semester.json marker file
      ↓
ActivateSemester(targetPath)
  ├─ Set currentSemesterPath
  ├─ Load semester tree view
  ├─ Load subjects UI
  └─ ShowSubjectView() - Show subject selection panel
```

**Key Classes Involved:**
- `MainForm.newSemesterButton_Click()`
- `SemesterApplicationService.CreateSemesterMarker()`
- `SemesterDomainService` - Creates marker file
- `ValidationHelperService` - Validates names

#### 2.2 Open Existing Semester
```
openSemesterButton_Click(sender, e)
  ↓ User clicks "Open Semester" button
CreateFolderBrowserDialog("Select semester folder")
  ↓ Dialog shown to user
User selects semester directory
  ↓
Directory.Exists(dialog.SelectedPath)?
  ├─ FALSE: Show error, return
  ├─ TRUE: Continue
  ↓
_semesterService.OpenSemester(dialog.SelectedPath)
  └─ SemesterApplicationService.OpenSemester()
    └─ SemesterDomainService.IsSemesterFolder()
      ├─ Checks for .semester.json marker
      ├─ TRUE: Return path
      ├─ FALSE: Return null
  ↓
ActivateSemester(openedSemesterPath)
  ├─ Set currentSemesterPath
  ├─ Load semester tree view
  ├─ Load subjects UI
  └─ ShowSubjectView() - Show subject selection panel
```

**Key Classes Involved:**
- `MainForm.openSemesterButton_Click()`
- `SemesterApplicationService.OpenSemester()`
- `SemesterDomainService.IsSemesterFolder()` - Validates semester structure

---

### 3. **Subject Management**

#### 3.1 Create Subject in Active Semester
```
addSubjectButton_Click(sender, e)
  ↓ User clicks "Add Subject" button
Validate currentSemesterPath is set
  ├─ Not set: Show warning, return
  ├─ Set: Continue
  ↓
PromptDialog.ShowDialog("Enter subject name")
  ↓ Dialog prompts for subject name
User enters name
  ↓
_subjectService.CreateSubject(currentSemesterPath, subjectName)
  └─ SubjectApplicationService.CreateSubject()
    └─ SubjectDomainService.CreateSubject()
      ├─ Creates directory: {semesterPath}/{subjectName}
      └─ Initializes subject folder structure
  ↓
LoadSubjectsUI()
  └─ Reloads subject cards in UI
    ├─ Iterates through all subjects in semester
    └─ Creates clickable subject card buttons
```

**Key Classes Involved:**
- `MainForm.addSubjectButton_Click()`
- `SubjectApplicationService.CreateSubject()`
- `SubjectDomainService` - Creates folder structure

#### 3.2 Open Subject
```
CreateSubjectCard(subjectPath)
  ↓ Subject card created with click event
User clicks subject card
  ↓
button.Click event → OpenSubject(subjectPath)
  ↓
OpenSubject(subjectPath)
  ├─ Set currentSubjectPath
  ├─ Update UI label to show selected subject
  ├─ LoadSubjectTree(subjectPath)
  │  └─ TreeViewDomainService.LoadSubjectTree()
  │     ├─ Loads folder structure into TreeView
  │     └─ Creates TreeNode hierarchy (repositories, files, etc.)
  └─ ShowWorkspaceView()
     ├─ Hide startup panel
     ├─ Hide subject selection panel
     └─ Show workspace panel (tree view, file list, metadata)
```

**Key Classes Involved:**
- `MainForm.OpenSubject()`
- `TreeViewDomainService.LoadSubjectTree()` - Loads folder hierarchy
- `MainForm.ShowWorkspaceView()` - Updates UI panels

---

### 4. **Repository Management**

#### 4.1 Create Repository in Subject
```
createRepositoryButton_Click(sender, e)
  ↓ User clicks "Create Repository" button
Validate currentSubjectPath is set
  ├─ Not set: Show warning, return
  ├─ Set: Continue
  ↓
RepoCreationDialog.ShowCreateDialog(this)
  ↓ Dialog shows form to collect repository details
User enters:
  ├─ Repository name
  └─ Deadline date
  ↓
Validate repository name
  ├─ _validationService.IsValidName(repositoryName)
  ├─ FALSE: Show error, return
  ├─ TRUE: Continue
  ↓
Check if directory exists
  ├─ EXISTS: Show duplicate error, return
  ├─ NOT EXISTS: Continue
  ↓
_repositoryService.CreateRepository(
    currentSubjectPath, 
    repositoryName, 
    deadline)
  └─ RepositoryApplicationService.CreateRepository()
    └─ RepositoryDomainService.CreateRepository()
      ├─ Creates directory: {subjectPath}/{repositoryName}
      ├─ Creates .repo.json metadata file
      │  └─ Contains: deadline, dateAdded, status (default: "in-progress")
      └─ Returns repository path
  ↓
LoadSubjectTree(createdPath)
  └─ Refreshes tree view to show new repository
    ├─ Updates tree nodes
    └─ Auto-selects the newly created repository
```

**Key Classes Involved:**
- `MainForm.createRepositoryButton_Click()`
- `RepoCreationDialog` - Collects repository details
- `RepositoryApplicationService.CreateRepository()`
- `RepositoryDomainService` - Creates folder and metadata file
- `ValidationHelperService` - Validates names

#### 4.2 Select/Open Repository
```
repositoryTreeView_AfterSelect(sender, TreeViewEventArgs)
  ↓ User selects repository node in tree view
Extract TreeNode data (NodeData tag)
  ↓
Check nodeData.NodeType
  ├─ SEMESTER/SUBJECT: Clear file list, hide metadata
  ├─ REPOSITORY: 
  │  └─ SelectRepository(nodeData.Path)
  │      ├─ Set selectedRepositoryPath
  │      ├─ LoadFiles(repositoryPath)
  │      │  └─ FileApplicationService.LoadFiles()
  │      │     └─ FileDomainService.GetFiles()
  │      │        ├─ Lists all files in repository
  │      │        └─ Populates filesListView
  │      ├─ LoadRepositoryMetadataUI(repositoryPath)
  │      │  └─ RepositoryApplicationService.EnsureMetadata()
  │      │     └─ RepositoryDomainService.EnsureMetadata()
  │      │        ├─ Reads .repo.json
  │      │        └─ Displays deadline, status in UI
  │      └─ UpdateRepositoryUiState() - Enable metadata buttons
  └─ FILE:
     ├─ Find repository root
     ├─ SelectRepository(repoPath)
     ├─ SelectFileInList(filePath)
     ├─ Set selectedFilePath
     └─ LoadVersionHistory(filePath)
        └─ Display file versions in list
```

**Key Classes Involved:**
- `MainForm.repositoryTreeView_AfterSelect()`
- `MainForm.SelectRepository()`
- `FileApplicationService.LoadFiles()` - Lists repository files
- `RepositoryApplicationService.EnsureMetadata()` - Loads repository metadata

#### 4.3 Update Repository Metadata
```
updateMetadataButton_Click(sender, e)
  ↓ User modifies deadline or status and clicks update
Validate selectedRepositoryPath is set
  ├─ Not set: Show warning, return
  ├─ Set: Continue
  ↓
Get selected status from statusComboBox
  ↓
_validationService.IsValidStatus(status)
  ├─ FALSE: Show error, return
  ├─ TRUE: Continue
  ↓
_repositoryService.UpdateRepositoryMetadata(
    selectedRepositoryPath, 
    deadline, 
    status)
  └─ RepositoryApplicationService.UpdateRepositoryMetadata()
    └─ RepositoryDomainService.UpdateRepositoryMetadata()
      └─ Writes updated metadata to .repo.json
        ├─ deadline: DateTime
        ├─ status: "in-progress" | "completed" | "late"
        └─ dateAdded: DateTime (unchanged)
  ↓
LoadRepositoryMetadataUI(selectedRepositoryPath)
  └─ Refresh metadata display on UI
  ↓
Show confirmation message
```

**Key Classes Involved:**
- `MainForm.updateMetadataButton_Click()`
- `RepositoryApplicationService.UpdateRepositoryMetadata()`
- `RepositoryDomainService` - Persists metadata

---

### 5. **File Management**

#### 5.1 Create File in Repository
```
createFileButton_Click(sender, e)
  ↓ User clicks "Create File" button
Validate selectedRepositoryPath is set
  ├─ Not set: Show warning, return
  ├─ Set: Continue
  ↓
PromptDialog.ShowDialog("Enter file name")
  ↓ Dialog prompts for file name
User enters name
  ↓
_validationService.IsValidName(fileName)
  ├─ FALSE: Show error, return
  ├─ TRUE: Continue
  ↓
FileTypeDialog.ShowCreateDialog(this)
  ↓ Dialog shows file type/extension selection
User selects extension (e.g., .txt, .pdf)
  ↓
_fileDomainService.CreateRepositoryFile(
    selectedRepositoryPath, 
    fileName, 
    extension, 
    out error)
  └─ FileDomainService.CreateRepositoryFile()
    ├─ Validates file doesn't already exist
    ├─ Creates file: {repositoryPath}/{fileName}.{extension}
    ├─ Initializes version history folder (.versions)
    └─ Returns file path or error
```

**Key Classes Involved:**
- `MainForm.createFileButton_Click()`
- `FileTypeDialog` - Collects file extension
- `FileDomainService.CreateRepositoryFile()` - Creates file

#### 5.2 Open File and Track Changes
```
filesListView_DoubleClick(sender, e)
  ↓ User double-clicks file in list
  OR
repositoryTreeView_NodeMouseDoubleClick(sender, e)
  ↓ User double-clicks file node in tree
  ↓
Extract file path from selected item
  ↓
OpenFile(filePath)
  ├─ Validate file exists
  │  └─ If not: Show error, return
  ├─ Create ProcessStartInfo
  │  └─ UseShellExecute = true (use default application)
  ├─ System.Diagnostics.Process.Start(filePath)
  │  └─ Opens file in default application (Word, Excel, etc.)
  ├─ Hook process exit event
  │  ↓ When file is closed
  │  └─ Trigger PromptAndSaveVersion(filePath)
  │     └─ User prompted to save version
  │        ├─ YES: _versionService.PromptAndSaveVersion()
  │        └─ NO: Discard changes
```

**Key Classes Involved:**
- `MainForm.filesListView_DoubleClick()`
- `MainForm.OpenFile()` - Launches file with default app
- `MainForm.PromptAndSaveVersion()` - Saves version on close

#### 5.3 File Selection in List
```
filesListView_SelectedIndexChanged(sender, e)
  ↓ User selects file in list view
Check if any files are selected
  ├─ NONE: Clear selectedFilePath, disable history buttons
  ├─ ONE: Continue
  ↓
Extract file path from selected item
  ↓
Set selectedFilePath
  ↓
LoadVersionHistory(selectedFilePath)
  └─ VersionDomainService.LoadVersionHistory()
    ├─ Reads version history for file
    └─ Displays versions in versionsListBox
  ↓
UpdateHistoryUiState(selectedFilePath)
  └─ Enable "View History" and "Revert" buttons
```

**Key Classes Involved:**
- `MainForm.filesListView_SelectedIndexChanged()`
- `VersionDomainService.LoadVersionHistory()`

---

### 6. **Version History & Management**

#### 6.1 Load Version History
```
LoadVersionHistory(filePath)
  ↓
Validate filePath is not null/empty
  ├─ INVALID: Clear version list, show "No file"
  ├─ VALID: Continue
  ↓
_versionService.LoadVersionHistory(filePath, versionsListBox, historyCaptionLabel)
  └─ VersionDomainService.LoadVersionHistory()
    ├─ Scans {filePath}/.versions directory
    ├─ Reads version metadata (timestamps, file size, etc.)
    ├─ Returns sorted list of FileVersion objects
    ├─ Populates versionsListBox with versions
    └─ Updates caption label with file name
  ↓
UpdateHistoryUiState(filePath)
  └─ Enable version-related buttons
```

**Key Classes Involved:**
- `MainForm.LoadVersionHistory()`
- `VersionDomainService.LoadVersionHistory()` - Reads version metadata

#### 6.2 Save New Version
```
PromptAndSaveVersion(filePath)
  ↓ Called when file process exits
_versionService.PromptAndSaveVersion(
    filePath, 
    selectedFilePath, 
    versionsListBox)
  └─ VersionDomainService.PromptAndSaveVersion()
    ├─ User prompted: "Save changes as new version?"
    ├─ YES:
    │  ├─ Copy current file → {filePath}/.versions/v{n}
    │  ├─ Store metadata (timestamp, file size)
    │  └─ Update versionsListBox
    └─ NO:
       └─ Discard changes (user keeps old version)
```

**Key Classes Involved:**
- `MainForm.PromptAndSaveVersion()`
- `VersionDomainService.PromptAndSaveVersion()` - Manages versioning

#### 6.3 Revert to Previous Version
```
revertButton_Click(sender, e)
  ↓ User selects version and clicks "Revert"
Validate selectedFilePath is set
  ├─ Not set: Show warning, return
  ├─ Set: Continue
  ↓
Check versionsListBox.SelectedItem
  ├─ NULL: Show warning, return
  ├─ FileVersion: Continue
  ↓
Show confirmation dialog
  ├─ Message: "Restore version and delete newer versions?"
  ├─ NO: Return
  ├─ YES: Continue
  ↓
_fileApplicationService.RevertFileVersion(
    selectedFilePath, 
    selectedVersion)
  └─ FileApplicationService.RevertFileVersion()
    └─ VersionDomainService.RevertToVersion()
      ├─ Delete current file
      ├─ Copy selected version → {filePath}
      ├─ Delete all versions newer than selected
      └─ Update version history
  ↓
LoadFiles(selectedRepositoryPath)
  └─ Refresh file list
  ↓
LoadSubjectTree(selectedFilePath)
  └─ Update tree view
  ↓
LoadVersionHistory(selectedFilePath)
  └─ Refresh version history
  ↓
Show success message
```

**Key Classes Involved:**
- `MainForm.revertButton_Click()`
- `FileApplicationService.RevertFileVersion()`
- `VersionDomainService.RevertToVersion()` - Handles revert logic

---

### 7. **TreeView Navigation**

#### 7.1 Load Child Nodes (Lazy Loading)
```
repositoryTreeView_AfterSelect(sender, TreeViewEventArgs)
  ↓ User selects tree node
Extract TreeNode.Tag (NodeData)
  ↓
LoadChildNodes(selectedNode)
  └─ TreeViewDomainService.LoadChildNodes()
    ├─ Check if node already has children
    ├─ If not: Scan folder for subdirectories/files
    ├─ Create TreeNode for each item
    │  ├─ Set Text = folder/file name
    │  ├─ Set Tag = NodeData(path, type)
    │  ├─ Assign appropriate icon
    │  └─ Add to parent node
    └─ Enable tree node expansion
```

**Key Classes Involved:**
- `MainForm.LoadChildNodes()`
- `TreeViewDomainService.LoadChildNodes()` - Lazy loads tree structure

---

### 8. **UI State Management**

#### 8.1 Update Repository UI State
```
UpdateRepositoryUiState(repositoryPath)
  ↓
Check if repositoryPath is set
  ├─ NULL/EMPTY:
  │  ├─ DISABLE: createFileButton
  │  ├─ DISABLE: deadlineDateTimePicker
  │  ├─ DISABLE: statusComboBox
  │  └─ DISABLE: updateMetadataButton
  ├─ SET:
  │  ├─ ENABLE: createFileButton
  │  ├─ ENABLE: deadlineDateTimePicker
  │  ├─ ENABLE: statusComboBox
  │  └─ ENABLE: updateMetadataButton
```

**Key Classes Involved:**
- `MainForm.UpdateRepositoryUiState()`

#### 8.2 Update History UI State
```
UpdateHistoryUiState(filePath)
  ↓
Check if filePath is set
  ├─ NULL/EMPTY:
  │  ├─ DISABLE: viewHistoryButton
  │  ├─ DISABLE: revertButton
  │  └─ UPDATE: historyCaptionLabel = "Version History"
  ├─ SET:
  │  ├─ ENABLE: viewHistoryButton
  │  ├─ UPDATE: historyCaptionLabel = "Version History - {fileName}"
  │  └─ Update revertButton based on version selection
```

**Key Classes Involved:**
- `MainForm.UpdateHistoryUiState()`

---

### 9. **Navigation & View Switching**

#### 9.1 Show Startup View
```
ShowStartupView()
  ↓ Displays initial UI when app starts or after reset
  ├─ mainSplitContainer.Panel1Collapsed = true (hide tree panel)
  ├─ startupPanel.Visible = true (show startup buttons)
  ├─ subjectSelectionPanel.Visible = false
  └─ workspacePanel.Visible = false
```

#### 9.2 Show Subject View
```
ShowSubjectView()
  ↓ Displays subject selection after semester is activated
  ├─ mainSplitContainer.Panel1Collapsed = true (hide tree panel)
  ├─ startupPanel.Visible = false
  ├─ subjectSelectionPanel.Visible = true (show subject cards)
  └─ workspacePanel.Visible = false
```

#### 9.3 Show Workspace View
```
ShowWorkspaceView()
  ↓ Displays full workspace when subject is opened
  ├─ mainSplitContainer.Panel1Collapsed = false (show tree panel)
  ├─ startupPanel.Visible = false
  ├─ subjectSelectionPanel.Visible = false
  └─ workspacePanel.Visible = true (show tree, files, metadata)
```

---

## Service Architecture

### Dependency Injection Layers

```
┌─────────────────────────────────────────────────────────┐
│                     MainForm (UI)                        │
└──────────────┬──────────────────────────────────────────┘
               │
┌──────────────▼──────────────────────────────────────────┐
│           Application Services Layer                     │
├──────────────────────────────────────────────────────────┤
│ • SemesterApplicationService                             │
│ • RepositoryApplicationService                           │
│ • FileApplicationService                                 │
│ • SubjectApplicationService                              │
└──────────────┬──────────────────────────────────────────┘
               │
┌──────────────▼──────────────────────────────────────────┐
│            Domain Services Layer                         │
├──────────────────────────────────────────────────────────┤
│ • SemesterDomainService                                  │
│ • RepositoryDomainService                                │
│ • FileDomainService                                      │
│ • SubjectDomainService                                   │
│ • TreeViewDomainService                                  │
│ • VersionDomainService                                   │
└──────────────┬──────────────────────────────────────────┘
               │
┌──────────────▼──────────────────────────────────────────┐
│         Infrastructure Services Layer                    │
├──────────────────────────────────────────────────────────┤
│ • FileSystemHelper (file I/O)                            │
│ • PathProvider (path utilities)                          │
│ • ValidationHelper (validation logic)                    │
└──────────────────────────────────────────────────────────┘
               │
               ▼
        File System / Disk
```

---

## Data Persistence

### Folder Structure
```
Semester/
├── .semester.json                    [Marker file identifying folder as semester]
├── Subject1/
│   ├── Repository1/
│   │   ├── .repo.json               [Repository metadata: deadline, status, dateAdded]
│   │   ├── file1.txt
│   │   ├── file2.pdf
│   │   └── .versions/
│   │       ├── v1_file1.txt         [Version snapshots]
│   │       ├── v2_file1.txt
│   │       └── ...
│   └── Repository2/
│       └── ...
└── Subject2/
    └── ...
```

### Metadata Files
- `.semester.json` - Empty marker file (identifies semester folder)
- `.repo.json` - Repository metadata:
  ```json
  {
    "deadline": "2024-12-25",
    "dateAdded": "2024-01-01",
    "status": "in-progress"
  }
  ```

---

## Key Data Models

- `RepoMetadata` - Stores deadline, dateAdded, status
- `FileVersion` - Represents a file snapshot with timestamp and metadata
- `NodeData` - Tree node data with path and type (Semester, Subject, Repository, SubRepository, File)
- `RepoCreationInfo` - Input from repository creation dialog

---

## Error Handling

All operations include try-catch blocks that:
1. Catch exceptions during operations
2. Display user-friendly error dialogs
3. Log operation state (return early on validation failure)
4. Prevent UI crashes

**Common Errors:**
- Invalid names (special characters, empty strings)
- Duplicate folders/repositories
- Missing files/directories
- Invalid metadata format
- File access denied

---

## Summary of Major Workflows

| Workflow | Entry Point | Key Services | Output |
|----------|------------|--------------|--------|
| **Create Semester** | `newSemesterButton_Click()` | SemesterDomainService | New `.semester.json` marker |
| **Open Semester** | `openSemesterButton_Click()` | SemesterDomainService | Loaded semester tree |
| **Create Subject** | `addSubjectButton_Click()` | SubjectDomainService | New subject folder |
| **Create Repository** | `createRepositoryButton_Click()` | RepositoryDomainService | New repo with `.repo.json` |
| **Create File** | `createFileButton_Click()` | FileDomainService | New file + `.versions` folder |
| **Save Version** | `PromptAndSaveVersion()` | VersionDomainService | New version snapshot |
| **Revert Version** | `revertButton_Click()` | VersionDomainService | File restored, newer versions deleted |
| **Update Metadata** | `updateMetadataButton_Click()` | RepositoryDomainService | Updated `.repo.json` |
