# IskolRepository Codebase Documentation

This document provides a comprehensive overview of the **IskolRepository** project, a Windows Forms application built with .NET 10.0. It is designed to help students organize their academic files using a structured hierarchy and integrated version control.

---

## 1. Project Overview

**IskolRepository** is a local file management system that organizes school-related documents into a specific hierarchy:
`Semester` -> `Subject` -> `Repository`.

Each "Repository" represents a specific assignment or project and includes metadata (deadlines, status) and automated versioning for files contained within it.

### Core Technologies
- **Framework:** .NET 10.0 (Windows Forms)
- **Data Persistence:** Local file system with JSON serialization for metadata and logs.
- **Serialization:** `System.Text.Json` with custom converters.

---

## 2. Directory Structure & Hierarchy

The application roots itself in the user's `My Documents/IskolRepository` folder.

### Hierarchy Rules:
1. **Root:** `IskolRepository/`
2. **Level 1 (Semester):** Folders representing academic terms (e.g., "Fall 2025").
3. **Level 2 (Subject):** Folders representing specific courses within a semester.
4. **Level 3 (Repository):** Folders representing specific tasks. This is where metadata and history are tracked.

### Managed Files:
- `metadata.json`: Located in each Repository folder. Stores project status and deadlines.
- `.history/`: A hidden folder within each Repository that contains version snapshots and a `log.json` file.

---

## 3. Key Components & Classes

### 3.1 UI Layer
- **`Program.cs`**: The entry point that initializes the application and runs `Form1`.
- **`Form1.cs`**: The main dashboard.
    - **Navigation:** Uses a `TreeView` to display the semester/subject/repository hierarchy.
    - **File Management:** Uses a `ListView` to show files within a selected repository.
    - **Metadata View:** Displays and allows editing of deadlines and status for the selected repository.
    - **History View:** Displays a list of previous versions for a selected file.
- **`RepoCreationDialog.cs`**: A specialized dialog for creating a new repository, requiring a name and a deadline.
- **`PromptDialog.cs`**: A generic input dialog used for names and version comments.

### 3.2 Data Models
- **`RepoMetadata.cs`**:
    - `Deadline`: The due date for the project.
    - `DateAdded`: When the repository was created.
    - `Status`: Current state (`in-progress`, `completed`, or `late`).
- **`FileVersion.cs`**:
    - `Version`: Integer version number.
    - `Timestamp`: When the snapshot was taken.
    - `Comment`: User-provided description of changes.
- **`RepoCreationInfo.cs`**: A simple DTO used to pass data back from the creation dialog.

### 3.3 Utilities
- **`DateOnlyDateTimeConverter.cs`**: A custom `JsonConverter<DateTime>` that ensures dates are stored in the `yyyy-MM-dd` format in JSON files, ignoring time components.

---

## 4. Key Workflows

### 4.1 Repository Management
When a user creates a repository:
1. A folder is created under the selected Subject.
2. A `metadata.json` file is initialized with the chosen deadline.
3. The UI refreshes to show the new node in the `TreeView`.

### 4.2 File Versioning System
The versioning system is "opt-in" triggered by file interaction:
1. **Opening a File:** Double-clicking a file in the `ListView` opens it with the default system handler (e.g., Notepad, Word).
2. **Detection:** The application waits for the external process to exit.
3. **Snapshot:** Upon exit, the user is prompted for a comment.
4. **Storage:**
    - The file is copied to `.history/{FileName}/v{N}.{ext}`.
    - An entry is added to `.history/{FileName}/log.json`.
5. **Reverting:** Users can select a version from the history list and click "Revert". The current file is then overwritten by the selected snapshot.

### 4.3 Validation Logic
- **`IsValidName`**: Ensures folder/file names don't contain invalid characters or trailing dots/spaces.
- **`IsValidStatus`**: Restricts repository status to `in-progress`, `completed`, or `late`.
- **`IsSystemManagedFile`**: Prevents users from seeing or directly editing `metadata.json` within the file list.

---

## 5. Persistence Format Examples

### `metadata.json`
```json
{
  "Deadline": "2026-05-20",
  "DateAdded": "2026-04-16",
  "Status": "in-progress"
}
```

### `log.json` (inside .history/{filename}/)
```json
[
  {
    "Version": 1,
    "Timestamp": "2026-04-16T10:30:00",
    "Comment": "Initial draft"
  },
  {
    "Version": 2,
    "Timestamp": "2026-04-16T14:45:00",
    "Comment": "Added introduction section"
  }
]
```
