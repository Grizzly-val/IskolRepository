# IskolRepository

**Your academic file organizer** — A Windows desktop application designed to help students and educators organize, manage, and track academic files, subjects, and semesters.

# Members
- Donatos, Trixter Lanz C.
- Ilao, Kent Patrick M.
- Laganzon, Adrian G.
- Villanueva, Franz Daniel

## Overview

IskolRepository is a comprehensive file management system built specifically for academic environments. It provides an intuitive interface for organizing educational materials, managing repositories, and tracking file versions with semester-based categorization.

## Features

- **Repository Management** — Create and manage repositories for organizing academic materials
- **Subject Organization** — Organize files by subject with hierarchical structure
- **Semester Tracking** — Track files across different semesters
- **File Management** — Create, organize, and manage various file types
- **File Versioning** — Track file versions and history
- **Tree View Navigation** — Intuitive hierarchical navigation of repositories
- **Dark Theme UI** — Modern, user-friendly interface with accessibility improvements

## Project Structure

```
IskolRepository/
├── Core/                          # Business logic and service interfaces
│   ├── Interfaces/                # Service contracts
│   │   ├── IFileService.cs
│   │   ├── IRepositoryService.cs
│   │   ├── ISemesterService.cs
│   │   ├── ISubjectService.cs
│   │   ├── ITreeViewService.cs
│   │   ├── IVersionService.cs
│   │   └── Infrastructure/        # Infrastructure contracts
│   ├── Services/                  # Service implementations
│   │   ├── FileService.cs
│   │   ├── RepositoryService.cs
│   │   ├── SemesterService.cs
│   │   ├── SubjectService.cs
│   │   ├── TreeViewService.cs
│   │   ├── VersionService.cs
│   │   └── Infrastructure/        # Infrastructure services
│   └── ...
├── Forms/                         # Windows Forms UI
│   ├── MainForm.cs                # Main application window
│   ├── StartupView.cs             # Startup/splash screen
│   ├── RepoCreationDialog.cs      # Repository creation dialog
│   ├── FileTypeDialog.cs          # File type selection
│   ├── SubjectSelectionView.cs    # Subject selection interface
│   └── ...
├── Models/                        # Data models
│   ├── FileIdentity.cs
│   ├── FileVersion.cs
│   ├── RepoCreationInfo.cs
│   └── RepoMetadata.cs
├── Utilities/                     # Helper utilities
│   ├── AnimationHelper.cs
│   ├── ThemeManager.cs
│   └── DateOnlyDateTimeConverter.cs
└── Program.cs                     # Application entry point
```

## Prerequisites

- **.NET 10.0** or later (Windows-specific)
- **Windows Forms** support
- Visual Studio 2022 or later (recommended) or any C# compatible IDE

## Getting Started

### Building the Project

```bash
dotnet build IskolRepository.sln
```

### Running the Application

```bash
dotnet run --project IskolRepository.csproj
```

Or directly run the executable from the build output:
```bash
.\bin\Debug\net10.0-windows\IskolRepository.exe
```

## Architecture

IskolRepository follows a **service-oriented architecture** with clean separation of concerns:

- **Presentation Layer** (`Forms/`) — Windows Forms UI components
- **Business Logic Layer** (`Core/Services/`) — Service implementations handling business operations
- **Interface Layer** (`Core/Interfaces/`) — Contracts defining service boundaries
- **Data Layer** (`Models/`) — Data transfer objects and entity models
- **Infrastructure** (`Core/Interfaces/Infrastructure/`, `Core/Services/Infrastructure/`) — File system and validation helpers

### Key Components

- **ServiceCollectionExtensions** — Dependency injection setup and service registration
- **FileService** — Handles file operations and management
- **RepositoryService** — Manages academic repositories
- **TreeViewService** — Handles hierarchical navigation and tree view rendering
- **VersionService** — Manages file versioning and history
- **ThemeManager** — Manages application theming and visual consistency

## Development

### Testing

The project uses **NUnit** for unit testing and **Moq** for mocking:

```bash
dotnet test
```

### Error Handling

The application includes comprehensive exception handling at multiple levels:
- UI thread exception handling
- Application domain exception handling
- Startup error handling with user-friendly messaging

## UI Design

The application features a modern dark theme with:
- Dark navy background (RGB 12, 14, 24)
- Light text for contrast (RGB 255, 255, 255)
- Muted secondary text (RGB 180, 190, 205)
- Accessible color scheme with proper contrast ratios

---

**IskolRepository** — Making academic organization simple and efficient.
