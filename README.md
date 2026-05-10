# 📚 IskolRepo

> A local Windows desktop application for organizing academic files, repositories, coursework, and version history.

---

# 📖 Project Description and Purpose

## Overview

**IskolRepo** is a local Windows desktop application that helps students organize academic files and tasks by:

- Semester
- Subject
- Repository or Activity

The system works like a smarter version of ordinary folders on a computer. Students can create semester folders, organize coursework by subject, initialize repositories for activities, manage deadlines, track submission status, create files, organize subrepositories, and maintain local version history for supported files.

The application is built using:

- **C#**
- **.NET 10**
- **Windows Forms**

and is designed to work completely **offline**, storing all academic data locally on the user's computer.

---

## Purpose of the System

Students commonly organize files using folders such as:

```text
1st Semester/
Programming/
Assignment 1/
Final Project/
```

Although this works initially, managing deadlines, revisions, versions, and multiple subjects becomes difficult as coursework increases.

IskolRepo solves this problem by introducing a structured academic repository system:

```text
Semester → Subject → Repository/Activity → Files and Subrepositories
```

Each repository represents a specific academic activity such as:

- Assignments
- Laboratory Exercises
- Reports
- Presentations
- Projects

The system stores repository metadata including:

- Deadline
- Date Added
- Status
- Submitted Date

It also provides a **local version history feature** for supported file types, allowing students to save snapshots of their work and restore previous versions when necessary.

---

# 🧩 UML Diagram

The UML documentation is divided into focused diagrams so each one explains a specific architectural concern while preserving the original class and relationship details.

---

## 🏗️ High-Level Architecture Diagram

This diagram shows the application entry point, UI ownership, and service access path used by `MainForm`.

```mermaid
classDiagram
  direction TB

  class Program {
    +Main()
  }

  class ServiceFactory {
    +CreateServices() ServiceRegistry
  }

  class ServiceRegistry {
    +ISemesterService SemesterService
    +IRepositoryService RepositoryService
    +IFileService FileService
    +ISubjectService SubjectService
    +ITreeViewService TreeViewService
    +IVersionService VersionService
    +IValidationHelper ValidationService
    +IFileIdentityManager FileIdentityManager
    +IFileReconciliationService FileReconciliationService
  }

  class MainForm {
    -_semesterService ISemesterService
    -_repositoryService IRepositoryService
    -_fileService IFileService
    -_subjectService ISubjectService
    -_treeViewService ITreeViewService
    -_versionService IVersionService
    -_validationService IValidationHelper
    -currentSemesterPath string?
    -currentSubjectPath string?
    -selectedRepositoryPath string?
    -currentBrowsePath string?
    -selectedFilePath string?
  }

  class StartupView {
    +OpenSemesterRequested event
    +NewSemesterRequested event
  }

  class SubjectSelectionView {
    +AddSubjectRequested event
    +ChangeSemesterRequested event
    +SemesterName string
    +PopulateSubjects(loader)
  }

  Program ..> ServiceFactory : calls
  Program ..> MainForm : runs
  ServiceFactory ..> ServiceRegistry : builds

  MainForm "1" *-- "1" StartupView : owns
  MainForm "1" *-- "1" SubjectSelectionView : owns
  MainForm "1" --> "1" ServiceRegistry : receives

```

---

## ⚙️ Service Implementation Diagram

This diagram shows the service interfaces, concrete implementations, and interface realization relationships.

```mermaid
classDiagram
  direction TB

  class ISemesterService {
    <<interface>>
    +OpenSemester(selectedPath) string
    +CreateSemester(parentPath,semesterName) string
    +CreateSemesterMarker(semesterPath)
  }

  class ISubjectService {
    <<interface>>
    +CreateSubject(semesterPath,subjectName)
    +GetSubjectsForSemester(semesterPath) IEnumerable
    +LoadSubjectsUI(semesterPath,subjectCardsPanel,createSubjectCard,onEmpty)
  }

  class IRepositoryService {
    <<interface>>
    +CreateRepository(subjectPath,repositoryName,deadline) string
    +UpdateRepositoryMetadata(repositoryPath,deadline,status)
    +EnsureMetadata(repositoryPath) RepoMetadata
    +FindRepositoryRoot(startPath) string?
  }

  class IFileService {
    <<interface>>
    +LoadFiles(repositoryRootPath,browsePath,filesListView,semesterMarkerFileName)
    +CreateFolder(parentPath,name,folderType)
    +CreateFile(repositoryPath,fileName,extension) string
    +OpenFile(filePath,onFileExited)
  }

  class IVersionService {
    <<interface>>
    +LoadVersionHistory(filePath,versionsListBox,captionLabel,noVersionsMessageLabel)
    +SaveVersion(filePath,comment)
    +IsSupportedVersionFileType(filePath) bool
    +CanSaveVersion(filePath) bool
    +RevertToVersion(filePath,selectedVersion)
  }

  class ITreeViewService {
    <<interface>>
    +LoadSemesterTree(semesterPath,repositoryTreeView,semesterMarkerFileName)
    +LoadSubjectTree(currentSubjectPath,selectPath,repositoryTreeView,semesterMarkerFileName)
    +LoadChildNodes(parentNode,semesterMarkerFileName)
    +FindNodeByPath(nodes,path) TreeNode?
    +EnsureParentChainExpanded(node)
  }

  class IFileIdentityManager {
    <<interface>>
    +LoadManifest(repositoryPath) FileIdentityManifest
    +RegisterFile(repositoryPath,filePath) Guid
    +UpdateFilePath(repositoryPath,fileId,newPath)
    +GetFileIdByPath(repositoryPath,filePath) Guid?
    +GetFilePathById(repositoryPath,fileId) string?
    +FindOrphaned(repositoryPath) List
    +FindLost(repositoryPath) List
    +SaveManifest(repositoryPath,manifest)
  }

  class IFileReconciliationService {
    <<interface>>
    +ValidateManifestIntegrity(repositoryPath) FileReconciliationReport
    +MigrateHistoryFolders(repositoryPath)
    +ReconcileLostFiles(repositoryPath)
    +RegisterAllUnregisteredFiles(repositoryPath)
    +ComputeFileHash(filePath) string
  }

  class IValidationHelper {
    <<interface>>
    +IsRepositoryFolder(path) bool
    +IsInsideRepository(node) bool
    +ApplyNodeValidationColors(node)
    +IsValidStatus(status) bool
    +IsSystemManagedFile(filePath,semesterMarkerFileName) bool
    +IsValidName(name) bool
  }

  class IFileSystemHelper {
    <<interface>>
    +CreateDirectory(path)
    +DirectoryExists(path) bool
    +FileExists(path) bool
    +IsDirectoryEmpty(path) bool
    +IsValidName(name) bool
    +CreateRepositoryFile(repositoryPath,fileName,extension) string?
    +EnumerateFiles(path) IEnumerable
    +EnumerateDirectories(path) IEnumerable
    +EnumerateFileSystemEntries(path) IEnumerable
    +ReadAllText(filePath) string
    +WriteAllText(filePath,content)
    +SetFileAttributes(filePath,attributes)
    +GetFileAttributes(filePath) FileAttributes
    +MoveDirectory(sourcePath,destinationPath)
    +DeleteDirectory(path)
  }

  class IPathProvider {
    <<interface>>
    +CombinePaths(paths) string
    +GetFileName(path) string
    +GetDirectoryName(path) string?
    +GetFileNameWithoutExtension(path) string
    +GetExtension(path) string
    +GetFullPath(path) string
  }

  class SemesterService
  class SubjectService
  class RepositoryService
  class FileService
  class VersionService
  class TreeViewService
  class FileIdentityManager
  class FileReconciliationService
  class FileReconciliationReport
  class ValidationHelperService
  class FileSystemService
  class PathProviderService

  SemesterService ..|> ISemesterService
  SubjectService ..|> ISubjectService
  RepositoryService ..|> IRepositoryService
  FileService ..|> IFileService
  VersionService ..|> IVersionService
  TreeViewService ..|> ITreeViewService
  FileIdentityManager ..|> IFileIdentityManager
  FileReconciliationService ..|> IFileReconciliationService
  ValidationHelperService ..|> IValidationHelper
  FileSystemService ..|> IFileSystemHelper
  PathProviderService ..|> IPathProvider

```

---

## 🔌 Dependency Injection Diagram

This diagram preserves the constructor dependency relationships from the original design.

```mermaid
classDiagram
  direction LR

  class ServiceFactory {
    +CreateServices() ServiceRegistry
  }

  class ServiceRegistry
  class RepositoryService
  class FileService
  class VersionService
  class TreeViewService
  class FileReconciliationService
  class FileIdentityManager
  class ValidationHelperService
  class JsonSerializerOptions

  class IRepositoryService { <<interface>> }
  class IFileIdentityManager { <<interface>> }
  class IValidationHelper { <<interface>> }
  class IFileReconciliationService { <<interface>> }
  class IFileSystemHelper { <<interface>> }
  class IPathProvider { <<interface>> }

  ServiceFactory ..> ServiceRegistry : returns
  ServiceFactory ..> ValidationHelperService : creates
  ServiceFactory ..> FileIdentityManager : creates
  ServiceFactory ..> FileReconciliationService : creates
  ServiceFactory ..> RepositoryService : creates
  ServiceFactory ..> FileService : creates
  ServiceFactory ..> VersionService : creates
  ServiceFactory ..> TreeViewService : creates

  ServiceRegistry "1" *-- "1" IRepositoryService
  ServiceRegistry "1" *-- "1" IFileIdentityManager
  ServiceRegistry "1" *-- "1" IValidationHelper
  ServiceRegistry "1" *-- "1" IFileReconciliationService

  RepositoryService ..> IFileSystemHelper : injected
  RepositoryService ..> IValidationHelper : injected
  RepositoryService ..> IPathProvider : injected
  RepositoryService ..> JsonSerializerOptions : injected
  RepositoryService ..> IFileReconciliationService : injected

  FileService ..> IFileSystemHelper : injected
  FileService ..> IPathProvider : injected
  FileService ..> IValidationHelper : injected
  FileService ..> IFileIdentityManager : injected
  FileService ..> IRepositoryService : injected

  VersionService ..> JsonSerializerOptions : injected
  VersionService ..> IFileIdentityManager : injected
  VersionService ..> IRepositoryService : injected

  TreeViewService ..> IFileSystemHelper : injected
  TreeViewService ..> IPathProvider : injected
  TreeViewService ..> IValidationHelper : injected
  TreeViewService ..> IRepositoryService : injected

  FileReconciliationService ..> IFileSystemHelper : injected
  FileReconciliationService ..> IPathProvider : injected
  FileReconciliationService ..> IFileIdentityManager : injected

  FileIdentityManager ..> IFileSystemHelper : injected
  FileIdentityManager ..> IPathProvider : injected

  ValidationHelperService ..> IFileSystemHelper : injected

```

---

## 🗂️ Domain Model Diagram

This diagram isolates persisted repository, identity, and versioning data.

```mermaid
classDiagram
  direction LR

  class RepoMetadata {
    +Deadline DateTime
    +DateAdded DateTime
    +Status string
    +Submitted DateTime?
  }

  class FileIdentityManifest {
    +Files Dictionary~string,FileIdentity~
    +ManifestVersion int
    +LastUpdated DateTime
  }

  class FileIdentity {
    +FileId Guid
    +CurrentPath string
    +OriginalFileName string
    +CreatedDate DateTime
    +Status string
  }

  class FileVersion {
    +FileId Guid
    +Version int
    +Timestamp DateTime
    +Comment string
    +SnapshotPath string
  }

  class RepoCreationInfo {
    +RepositoryName string
    +Deadline DateTime
  }

  class RepositoryBrowseEntry {
    +Kind RepositoryBrowseEntryKind
    +Path string
    +DisplayName string
  }

  class RepositoryBrowseEntryKind {
    <<enumeration>>
    Parent
    Directory
    File
  }

  class NodeData {
    +Path string
    +FileName string
    +NodeType NodeType
    +IsValidFile bool
  }

  class NodeType {
    <<enumeration>>
    Semester
    Subject
    Repository
    SubRepository
    File
  }

  FileIdentityManifest "1" *-- "0..*" FileIdentity : contains
  FileVersion ..> FileIdentity : uses FileId
  RepositoryBrowseEntry ..> RepositoryBrowseEntryKind : typedBy
  NodeData ..> NodeType : typedBy

```

---

# ✨ Features and Functionalities of the System

## 📁 Semester Management

- Create a new semester folder
- Open existing semester folders
- Validate semester folders using `.semester.json`
- Switch between semesters

---

## 📚 Subject Management

- Add subjects inside semesters
- Display subjects as selectable cards
- Open subject workspaces

---

## 🗃️ Repository and Activity Management

- Create repositories for academic activities
- Assign deadlines
- Store metadata using hidden files
- Track repository status:
  - `in-progress`
  - `completed`
  - `submitted`
- Automatically record submitted dates
- Show due date warnings and overdue indicators

---

## 📄 File and Folder Organization

Supported file creation:

- `.txt`
- `.docx`
- `.pptx`
- `.xlsx`
- `.pub`
- `.one`
- `.accdb`

Additional features:

- Create nested folders
- Browse through tree view and list view
- Open files using default Windows applications
- Prevent invalid workflow usage

---

## 🕓 Local Version History

Supported version-controlled files:

- `.txt`
- `.docx`

Features include:

- Save snapshots with comments
- View version history
- Restore previous versions
- Maintain version timelines
- Track files using GUID identities

---

## 🔍 File Identity and Reconciliation

- Maintain hidden `.metadata/files.json`
- Assign unique `Guid` identifiers
- Detect orphaned and lost files
- Reconcile histories using content hashes
- Migrate older filename-based histories

---

## 🎨 User Interface

- Windows Forms desktop interface
- Startup screen
- Subject selection view with animations
- Repository tree view and file explorer
- Metadata and version history panels
- Dark and light mode support
- Icons and warning indicators

---

# ⚙️ Explanation of How the Program Works

When the application starts:

1. `Program.cs` initializes Windows Forms.
2. Global exception handlers are registered.
3. Services are created through `ServiceFactory.CreateServices()`.
4. `MainForm` is launched.

---

## 🔄 Main Workflow

### 1. Semester Creation or Opening

The user creates or opens a semester folder.

The system validates the semester using:

```text
.semester.json
```

---

### 2. Subject Selection

The user creates or selects a subject.

---

### 3. Repository Creation

Repositories are created for academic activities such as:

- Assignments
- Projects
- Research
- Reports
- Laboratory Exercises

Each repository stores hidden metadata including:

- Deadline
- Date Added
- Status
- Submitted Date

---

### 4. File Management

Users can:

- Create files
- Create subrepositories
- Organize folders
- Browse repository contents

System-managed metadata files remain hidden.

---

### 5. Version History

When a supported file is selected:

- The version panel displays saved snapshots
- Users may save new versions with comments
- Users may restore earlier versions

---

## 🗂️ Local Folder Structure

```text
Selected Parent Folder/
└── Semester Name/
    ├── .semester.json
    └── Subject Name/
        └── Repository or Activity Name/
            ├── .metadata/
            │   ├── metadata.json
            │   ├── files.json
            │   └── .history/
            │       └── {file-guid}/
            │           ├── v1.docx
            │           ├── v2.docx
            │           └── log.json
            ├── Research.docx
            ├── Notes.txt
            └── Subrepository/
                └── Draft.txt
```

The system works completely offline and does not require an internet connection.

---

# ▶️ Instructions on How to Run the Application

## ✅ Prerequisites

Required software:

- Windows Operating System
- .NET SDK `10.0.203` or compatible .NET 10 SDK
- Visual Studio 2022 or later

---

## 🖥️ Run Using Visual Studio

1. Clone or download the repository
2. Open:

```text
IskolRepository.sln
```

3. Restore NuGet packages if necessary
4. Set `IskolRepository` as the startup project
5. Press `F5` or click **Start**

---

## 💻 Run Using the Command Line

From the repository root, run:

```bash
dotnet restore
dotnet build IskolRepository.sln
dotnet run --project IskolRepository.csproj
```

---

## 📦 Build Output

After building, the executable is typically located at:

```text
bin/Debug/net10.0-windows/IskolRepository.exe
```

The exact folder may vary depending on the build configuration.

---

# 👨‍💻 Names of the Developers or Team Members

- **Donatos, Trixter Lanz C.**
- **Ilao, Kent Patrick M.**
- **Laganzon, Adrian G.**
- **Villanueva, Franz Daniel**