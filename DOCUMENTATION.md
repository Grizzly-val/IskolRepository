# IskolRepository Codebase Documentation

This document provides a comprehensive overview of the **IskolRepository** project, a Windows Forms application built with .NET 10.0. It is designed to help students organize their academic files using a structured hierarchy and integrated version control.

---

## 1. Project Overview

**IskolRepository** is a local file management system that organizes school-related documents into a specific hierarchy:
`Semester` -> `Subject` -> `Repository`.

Each "Repository" represents a specific assignment or project and includes metadata (deadlines, status) and automated versioning for files contained within it. Unlike standard file explorers, it enforces a specific structure to ensure academic materials remain organized.

### Core Technologies
- **Framework:** .NET 10.0 (Windows Forms)
- **Data Persistence:** Local file system with JSON serialization for metadata and history logs.
- **Serialization:** `System.Text.Json` with custom converters for specific date formats.

---

## 2. Directory Structure & Hierarchy

The application allows users to select or create a "Semester" folder anywhere on their local machine. Once a semester is activated, the application manages the following structure:

### Hierarchy Rules:
1. **Root (Semester):** The base folder representing an academic term (e.g., "Fall 2025").
2. **Level 1 (Subject):** Subfolders within the semester folder representing specific courses.
3. **Level 2 (Repository):** Folders within a subject representing specific tasks or assignments. This is the unit of versioning and metadata tracking.

### Managed Files:
- `metadata.json`: Located in each Repository folder. Stores the deadline, creation date, and current status.
- `.history/`: A hidden folder within each Repository that contains versioned snapshots of files and a `log.json` file for each tracked file.

---

## 3. Key Components & Classes

### 3.1 UI Layer
- **`Program.cs`**: The entry point that initializes the application and runs `MainForm`.
- **`MainForm.cs`**: The primary interface, featuring a dynamic view system:
    - **Startup View:** Allows opening an existing semester folder or creating a new one.
    - **Subject Selection View:** Displays subjects as interactive cards for quick navigation.
    - **Workspace View:** The main project dashboard.
        - **Repository Tree:** A `TreeView` showing repositories within the current subject.
        - **File Manager:** A `ListView` displaying files in the selected repository.
        - **Metadata Panel:** Displays and allows editing of project deadlines and status (`in-progress`, `completed`, `late`).
        - **History Panel:** Displays version history for the selected file.
- **`RepoCreationDialog.cs`**: A specialized dialog for creating a new repository, requiring a name and a deadline.
- **`FileTypeDialog.cs`**: A selection dialog used when creating new files (currently supports `.txt` and `.docx`).
- **`PromptDialog.cs`**: A generic input dialog used for names and version comments.

### 3.2 Data Models
- **`RepoMetadata.cs`**:
    - `Deadline`: The due date for the project.
    - `DateAdded`: The date the repository was initialized.
    - `Status`: Current state (`in-progress`, `completed`, or `late`).
- **`FileVersion.cs`**:
    - `Version`: Sequential version number.
    - `Timestamp`: When the snapshot was captured.
    - `Comment`: User-provided description of changes.
- **`RepoCreationInfo.cs`**: A DTO used to pass data from `RepoCreationDialog` back to the main form.

### 3.3 Utilities
- **`DateOnlyDateTimeConverter.cs`**: A custom `JsonConverter<DateTime>` that ensures dates are stored in the `yyyy-MM-dd` format in JSON files, omitting time components for metadata consistency.

---

## 4. Key Workflows

### 4.1 Semester & Subject Management
1. **Activation:** Users select a folder to act as a "Semester". If new, it must be empty.
2. **Subject Creation:** Subjects are created as folders directly under the Semester root.
3. **Repository Initialization:** When creating a repository:
    - A folder is created under the selected Subject.
    - A `metadata.json` is generated with the selected deadline.

### 4.2 File Management
- **Creation:** Users can create new files directly within a repository using the "Create File" action, which prompts for a name and extension via `FileTypeDialog`.
- **Versioning (Opt-in):**
    1. **Opening:** Double-clicking a file opens it with the system's default handler.
    2. **Tracking:** The application monitors the external process.
    3. **Snapshot:** When the process exits, the application prompts the user for a comment and creates a new version in the `.history` folder.
- **Reverting:** Users can select a previous version from the history list and click "Revert". This replaces the current file with the selected snapshot and removes any "newer" versions to maintain a linear history.

### 4.3 Validation Logic
- **`IsValidName`**: Ensures folder/file names don't contain invalid characters or system-reserved sequences.
- **`IsSystemManagedFile`**: Filters out `metadata.json` and internal folders from the file list to prevent accidental corruption.

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
