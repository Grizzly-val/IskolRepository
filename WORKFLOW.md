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

## Validation & UI State Management

### 1.5 **File Validation System**

Files in the tree view are validated during tree construction to determine if they are valid or invalid:

**Validation Logic:**
```
File Validation Check
  ↓
Is file's parent node a Subject?
  ├─ YES: File is INVALID (directly under subject, outside any repository)
  │  ├─ Mark as RED in tree view
  │  ├─ Set NodeData.IsValidFile = false
  │  └─ Cannot be opened, edited, or interacted with
  │
  └─ NO: File is VALID (inside repository or subrepo)
     ├─ Mark as normal (black) in tree view
     ├─ Set NodeData.IsValidFile = true
     └─ Can be opened and edited normally
```

**Where Validation Happens:**
- `TreeViewDomainService.LoadChildNodes()` - When building tree structure
- `IsValidFileNode()` helper method - Determines validity based on parent node type

**NodeData Enhancement:**
- Added `IsValidFile` property (boolean)
- Tracks validation status during tree construction
- Default: `true` (backward compatible)

---

### 1.6 **Message Panel & Right Panel Visibility**

The right side of MainForm now intelligently hides/shows content based on selection:

**Right Panel Composition:**
```
Right Panel Contents
  ├─ Message Panel (when no valid selection)
  │  └─ Bold, prominent text with contextual message
  ├─ Metadata GroupBox (repository deadline, status)
  ├─ Files ListBox (files in repository)
  └─ History Panel (version history)
```

**Message Panel Styling:**
- **Font:** Segoe UI, 14pt, Bold (highly visible)
- **Color:** Dark Gray (#5A5A5A) for excellent contrast
- **Padding:** 20px for breathing room around text
- **Alignment:** Centered both horizontally and vertically
- **Area:** Fills entire right panel (394x512 pixels)
- **Purpose:** Guide users to make valid selections

**Visibility Rules:**
```
Selection Type          Message Panel    Content Panels
─────────────────────────────────────────────────────
No selection            VISIBLE          HIDDEN
Semester selected       VISIBLE          HIDDEN
Subject selected        VISIBLE          HIDDEN
Invalid file selected   VISIBLE          HIDDEN
Valid repository        HIDDEN           VISIBLE
Valid file              HIDDEN           VISIBLE
```

**Message Display Logic:**
```
ShowMessage(message)
  ├─ messageLabel.Text = message
  ├─ messagePanel.Visible = true
  ├─ metadataGroupBox.Visible = false
  ├─ filesListView.Visible = false
  ├─ historyPanel.Visible = false
  └─ createFileButton.Enabled = false

HideMessage()
  ├─ messagePanel.Visible = false
  ├─ metadataGroupBox.Visible = true
  ├─ filesListView.Visible = true
  └─ historyPanel.Visible = true
```

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
  ↓ User selects node in tree view
Extract TreeNode data (NodeData tag)
  ↓
Check nodeData.NodeType & nodeData.IsValidFile
  ├─ SEMESTER/SUBJECT:
  │  ├─ Clear file list, history, metadata
  │  ├─ ShowMessage("Please select a repository")
  │  └─ Hide right panel content
  │
  ├─ REPOSITORY:
  │  ├─ SelectRepository(nodeData.Path)
  │  │  ├─ Set selectedRepositoryPath
  │  │  ├─ LoadFiles(repositoryPath)
  │  │  ├─ LoadRepositoryMetadataUI(repositoryPath)
  │  │  └─ UpdateRepositoryUiState()
  │  ├─ HideMessage()
  │  └─ Show right panel content
  │
  ├─ SUBREPOSITORY:
  │  ├─ Same as Repository handling
  │  ├─ HideMessage()
  │  └─ Show right panel content
  │
  └─ FILE:
     ├─ Check nodeData.IsValidFile
     ├─ If FALSE (invalid file):
     │  ├─ ShowMessage("File not under a repository / unknown activity")
     │  ├─ Clear all selections
     │  └─ Hide right panel content
     ├─ If TRUE (valid file):
     │  ├─ Find repository root
     │  ├─ SelectRepository(repoPath)
     │  ├─ LoadVersionHistory(filePath)
     │  ├─ HideMessage()
     │  └─ Show right panel content
     └─ Return early if errors
```

**Key Changes:**
- Added `nodeData.IsValidFile` check for File nodes
- Invalid files trigger error message and hide content
- Valid files proceed normally with content shown

**Related Classes:**
- `MainForm.repositoryTreeView_AfterSelect()`
- `NodeData` - Now includes `IsValidFile` property
- `ShowMessage()` / `HideMessage()` - New helper methods

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
Double-click on file in tree or file list
  ↓
repositoryTreeView_NodeMouseDoubleClick(sender, e)
  OR
filesListView_DoubleClick(sender, e)
  ↓
Extract file path and NodeData
  ↓
Check if it's a File node AND nodeData.IsValidFile
  ├─ NOT a File type:
  │  └─ Return (silent, do nothing)
  │
  ├─ Is a File BUT IsValidFile = false:
  │  └─ Return (silent, prevent opening invalid files)
  │
  └─ Is a File AND IsValidFile = true:
     └─ OpenFile(filePath)
        ├─ Validate file exists
        ├─ Create ProcessStartInfo (UseShellExecute = true)
        ├─ Process.Start() - Opens in default application
        └─ Catch and display errors
```

**New Behavior:**
- Invalid files (outside repositories) CANNOT be opened
- Double-clicking invalid file does nothing (silent block)
- Only valid files under repositories can be edited

**Related Classes:**
- `MainForm.repositoryTreeView_NodeMouseDoubleClick()` - Prevents invalid file opening
- `MainForm.OpenFile()` - Opens valid files only

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
User selects file in tree/list
  ↓
Save Version button text updates:
  ├─ Selected file: "Save version for {FileName}"
  └─ No file: "Select a file to save a version of"
  ↓
saveVersionButton_Click(sender, e)
  ↓
_versionService.CanSaveVersion(filePath)
  ├─ FALSE: keep button disabled
  └─ TRUE: continue
  ↓
_versionService.PromptAndSaveVersion(filePath, selectedFilePath, versionsListBox)
  ├─ User enters version comment
  ├─ Save snapshot to history path
  └─ Reload version list
  
Eligibility rules:
  1) A file is selected
  2) File extension is supported (.txt, .docx)
  3) LastWriteTime(file) differs from last saved snapshot
```

**Key Classes Involved:**
- `MainForm.saveVersionButton_Click()`
- `MainForm.UpdateSaveVersionButtonState()`
- `VersionDomainService.CanSaveVersion()`
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

### 7. **TreeView Navigation & Validation**

#### 7.1 Load Child Nodes with Validation (Lazy Loading)
```
repositoryTreeView_AfterSelect(sender, TreeViewEventArgs)
  ↓ User selects tree node
Extract TreeNode.Tag (NodeData)
  ↓
LoadChildNodes(selectedNode)
  └─ TreeViewDomainService.LoadChildNodes()
    ├─ Check if node already has children
    ├─ If not: Scan folder for subdirectories/files
    │
    ├─ For each DIRECTORY:
    │  ├─ Create TreeNode
    │  ├─ Determine NodeType (Subject, Repository, etc.)
    │  ├─ Create NodeData(path, nodeType, isValidFile=true)
    │  └─ Add to parent node
    │
    └─ For each FILE:
       ├─ Check IsValidFileNode(parentNode, parentData)
       │  └─ Validation Rule:
       │     ├─ parentData.NodeType == Subject?
       │     ├─ YES: isValidFile = false (file directly under subject)
       │     └─ NO: isValidFile = true (file in repo or subrepo)
       │
       ├─ Create NodeData(path, NodeType.File, isValidFile)
       ├─ Set ForeColor:
       │  ├─ isValidFile = true: SystemColors.WindowText (black/normal)
       │  └─ isValidFile = false: Color.Red (invalid)
       └─ Add to parent node
```

**Validation Method:**
```csharp
private static bool IsValidFileNode(TreeNode parentNode, NodeData parentData)
{
    // Files are valid if parent is NOT a Subject
    return parentData.NodeType != NodeType.Subject;
}
```

**Key Points:**
- Validation happens DURING tree construction, not on selection
- Invalid files are marked RED immediately
- Valid files appear normal (black)
- Red files cannot be opened or edited
- Reduces runtime checks, improves performance

**Related Classes:**
- `MainForm.LoadChildNodes()`
- `TreeViewDomainService.LoadChildNodes()` - Validates during load
- `TreeViewDomainService.IsValidFileNode()` - Validation logic

#### 7.2 Load Child Nodes (Lazy Loading)
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

**Related Classes:**
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

**Related Classes:**
- `MainForm.UpdateHistoryUiState()`

#### 8.3 Update Message Panel Visibility ⭐ NEW

The right panel intelligently switches between showing the message panel and content panels based on selection validity.

```
ShowMessage(message)
  ├─ Hide: metadataGroupBox, filesListView, historyPanel
  ├─ Show: messagePanel (visible = true)
  ├─ Set: messageLabel.Text = message
  └─ Call: messagePanel.BringToFront()

HideMessage()
  ├─ Hide: messagePanel (visible = false)
  └─ Show: metadataGroupBox, filesListView, historyPanel
```

**Message Panel Structure:**
```
Panel2 (contentSplitContainer.Panel2)
  └─ messagePanel (Dock.Fill)
      └─ messageLabel (Dock.Fill)
          ├─ Font: Segoe UI, 14pt, Bold
          ├─ Color: Dark Gray RGB(90, 90, 90)
          ├─ Padding: 20px
          └─ TextAlign: MiddleCenter
```

**Message Panel Visibility Logic:**
- **Show:** When no valid selection, invalid file selected, or semester/subject selected
- **Hide:** When valid repository or valid file selected
- **Initial State:** Hidden (visible = false)
- **Z-order:** Brings to front when shown to ensure visibility

**Visual Behavior:**
- Message panel and content panels are mutually exclusive (never both visible)
- Message panel fills entire Panel2 area when visible
- Clean switching between states without overlap
- Message text is always readable and centered

**When to Show:**
- Semester or Subject selected: "Please select a repository"
- Invalid file selected: "File not under a repository / unknown activity"
- No selection or reset: "Please select a repository"

**When to Hide:**
- Valid repository selected
- Valid file under repository selected

**Related Classes:**
- `MainForm.ShowMessage(string message)` - Shows message panel, hides content
- `MainForm.HideMessage()` - Hides message panel, shows content

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

- **RepoMetadata** - Stores deadline, dateAdded, status
- **FileVersion** - Represents a file snapshot with timestamp and metadata
- **NodeData** ⭐ ENHANCED - Tree node data with:
  - `Path` - Full path to node
  - `NodeType` - Type (Semester, Subject, Repository, SubRepository, File)
  - `IsValidFile` - ⭐ NEW: Boolean flag indicating if file is valid (inside repo)
    - Only meaningful when NodeType is File
    - true = file inside repository (can be opened)
    - false = file directly under subject (cannot be opened)
- **RepoCreationInfo** - Input from repository creation dialog

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
| **Save Version** | `saveVersionButton_Click()` | VersionDomainService | New version snapshot |
| **Revert Version** | `revertButton_Click()` | VersionDomainService | File restored, newer versions deleted |

---

## ⭐ NEW: Invalid Files & UI Visibility Control

### Overview
Added validation system to prevent files created outside repositories from being edited or opened, while maintaining visibility in the tree view for reference. Right panel now intelligently shows/hides based on valid selection.

### Features

#### 1. File Validation During Tree Construction
- **When:** During `TreeViewDomainService.LoadChildNodes()`
- **Logic:** Files directly under Subject nodes are marked as INVALID
- **Indicator:** RED text in tree view
- **Storage:** `NodeData.IsValidFile` property

#### 2. Invalid File Prevention
- **Double-Click:** Does nothing (silent block)
- **Selection:** Shows error message "File not under a repository / unknown activity"
- **Editing:** Cannot be edited or interacted with
- **Purpose:** Guides users to organize files in repositories

#### 3. Right Panel Visibility Control
- **Message Panel:** Shows contextual messages when no valid selection
- **Content Panels:** Show only when valid repository or file is selected
- **Messages:**
  - "Please select a repository" - For semester/subject/no selection
  - "File not under a repository / unknown activity" - For invalid files

#### 4. User Experience Flow

**Valid Repository Selected:**
```
User clicks repository in tree
  ↓
repositoryTreeView_AfterSelect() fires
  ↓
Check: NodeType = Repository? YES
  ↓
SelectRepository(path)
  ├─ Load metadata
  ├─ Load file list
  └─ Load history
  ↓
HideMessage()
  ├─ Show metadata panel
  ├─ Show files list
  └─ Show history panel
  ↓
User sees repository details and files
```

**Invalid File Selected:**
```
User clicks invalid file (RED) in tree
  ↓
repositoryTreeView_AfterSelect() fires
  ↓
Check: NodeType = File? YES
Check: IsValidFile? NO
  ↓
ShowMessage("File not under a repository / unknown activity")
  ├─ Hide metadata panel
  ├─ Hide files list
  └─ Hide history panel
  ↓
User sees error message, cannot interact with file
```

**Invalid File Double-Click:**
```
User double-clicks invalid file
  ↓
repositoryTreeView_NodeMouseDoubleClick() fires
  ↓
Check: NodeType = File? YES
Check: IsValidFile? NO
  ↓
Return (do nothing)
  ↓
File cannot be opened (silent block)
```

### Technical Implementation

**Changes Made:**
1. `NodeData` - Added `IsValidFile` boolean property
2. `TreeViewDomainService.LoadChildNodes()` - Validates files during tree load
3. `TreeViewDomainService.IsValidFileNode()` - Determines file validity
4. `MainForm.repositoryTreeView_AfterSelect()` - Checks `IsValidFile`, shows/hides content
5. `MainForm.repositoryTreeView_NodeMouseDoubleClick()` - Prevents opening invalid files
6. `MainForm.ShowMessage()` / `HideMessage()` - Manages message panel visibility
7. `MainForm.Designer.cs` - Added message panel UI controls

### Validation Rules

```csharp
// File is INVALID if:
- Parent node type is Subject
- File is directly under subject directory
- Outside of any repository folder

// File is VALID if:
- Parent node type is Repository or SubRepository
- File is inside a repository folder hierarchy
```

### Benefits

✅ **User Guidance** - Messages prevent confusion  
✅ **Error Prevention** - Cannot edit orphaned files  
✅ **Visual Clarity** - Red text highlights invalid files  
✅ **Clean UI** - Content hides when not relevant  
✅ **Performance** - Validation during tree load, not on click  
✅ **Maintainability** - Simple boolean flag, minimal changes  

### Related Documentation

- See `IMPLEMENTATION_SUMMARY.md` for detailed changes
- See `VISUAL_GUIDE.md` for UI behavior and state diagrams
- See `IMPLEMENTATION_APPROACH.md` for design decisions
