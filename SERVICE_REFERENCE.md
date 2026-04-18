# 🚀 Service-Oriented Architecture - Quick Reference

## How to Use Services in MainForm

### 1. Semester Management
```csharp
// Open existing semester
var semesterPath = _semesterService.OpenSemester(selectedPath);

// Create new semester
var newPath = _semesterService.CreateAndActivateSemester(parentPath, name);

// Create semester marker
_semesterService.CreateSemesterMarker(semesterPath);
```

### 2. Repository Management
```csharp
// Create repository
var repoPath = _repositoryService.CreateRepository(subjectPath, name, deadline);

// Update metadata
_repositoryService.UpdateRepositoryMetadata(repoPath, newDeadline, status);

// Get metadata
var metadata = _repositoryService.EnsureMetadata(repoPath);

// Find repository root
var root = _repositoryService.FindRepositoryRoot(somePath);
```

### 3. Subject Management
```csharp
// Create subject
_subjectService.CreateSubject(semesterPath, subjectName);

// Get all subjects
var subjects = _subjectService.GetSubjectsForSemester(semesterPath);

// Load UI
_subjectService.LoadSubjectsUI(semesterPath, panel, cardFactory, onEmpty);
```

### 4. File Management
```csharp
// Create file
var filePath = _fileService.CreateFile(repoPath, fileName, extension);

// Load files to ListView
_fileService.LoadFiles(repoPath, filesListView, markerFileName);

// Revert file version
_fileService.RevertFileVersion(filePath, selectedVersion);
```

### 5. TreeView Management
```csharp
// Load semester tree
_treeViewService.LoadSemesterTree(semesterPath, treeView, markerFile);

// Load subject tree
_treeViewService.LoadSubjectTree(subjectPath, selectPath, treeView, markerFile);

// Load child nodes
_treeViewService.LoadChildNodes(parentNode, markerFile);
```

### 6. Version History
```csharp
// Load version history
_versionService.LoadVersionHistory(filePath, listBox, captionLabel);

// Prompt and save version
_versionService.PromptAndSaveVersion(filePath, selectedPath, listBox);

// Revert to version
_versionService.RevertToVersion(filePath, selectedVersion);
```

---

## Architecture Layers

### Application Services (ISemesterApplicationService, etc.)
- High-level business workflows
- Coordinate multiple domain services
- Direct contact point for UI
- Handle application-specific logic

### Domain Services (SemesterDomainService, etc.)
- Core business rules
- Orchestrate infrastructure services
- Reusable across applications
- No UI awareness

### Infrastructure Services (FileSystemHelper, etc.)
- Low-level operations
- File I/O, path operations, validation
- Reusable utilities
- No business logic

---

## Adding New Features

### Step 1: Define Interface
```csharp
public interface INewService
{
    void DoSomething(string param);
}
```

### Step 2: Implement Service
```csharp
public class NewService : INewService
{
    private readonly IFileSystemHelper _fileSystem;
    
    public NewService(IFileSystemHelper fileSystem)
    {
        _fileSystem = fileSystem;
    }
    
    public void DoSomething(string param)
    {
        // Implementation
    }
}
```

### Step 3: Register in ServiceFactory
```csharp
var newService = new NewService(fileSystemHelper);

return new ApplicationServices(
    // ... existing services ...
    newService  // Add your service
);
```

### Step 4: Use in MainForm
```csharp
// In MainForm constructor
private readonly INewService _newService;

public MainForm(ApplicationServices services)
{
    _newService = services.NewService;
    // ...
}
```

---

## Service Dependencies

```
ApplicationServices
├── ISemesterApplicationService
│   ├── ISemesterDomainService
│   │   ├── IFileSystemHelper
│   │   └── IPathProvider
│   └── ITreeViewDomainService
│       ├── IFileSystemHelper
│       ├── IPathProvider
│       └── IValidationHelper
├── IRepositoryApplicationService
│   └── IRepositoryDomainService
│       ├── IFileSystemHelper
│       ├── IValidationHelper
│       ├── IPathProvider
│       └── JsonSerializerOptions
├── IFileApplicationService
│   ├── IFileDomainService
│   │   ├── IFileSystemHelper
│   │   ├── IPathProvider
│   │   └── IValidationHelper
│   └── IVersionDomainService
│       └── JsonSerializerOptions
├── ISubjectApplicationService
│   └── ISubjectDomainService
│       ├── IFileSystemHelper
│       └── IPathProvider
├── ITreeViewDomainService
│   ├── IFileSystemHelper
│   ├── IPathProvider
│   └── IValidationHelper
└── IVersionDomainService
    └── JsonSerializerOptions
```

---

## Testing Guide

### Unit Test Example
```csharp
[TestClass]
public class SemesterApplicationServiceTests
{
    [TestMethod]
    public void OpenSemester_WithValidPath_ReturnsPath()
    {
        // Arrange
        var mockDomain = new Mock<ISemesterDomainService>();
        mockDomain.Setup(s => s.IsSemesterFolder(It.IsAny<string>()))
            .Returns(true);
        
        var service = new SemesterApplicationService(mockDomain.Object, null);
        
        // Act
        var result = service.OpenSemester("/valid/path");
        
        // Assert
        Assert.AreEqual("/valid/path", result);
    }
}
```

### Integration Test Example
```csharp
[TestClass]
public class SemesterApplicationServiceIntegrationTests
{
    [TestMethod]
    public void CreateAndActivateSemester_CreatesActualSemester()
    {
        // Arrange
        var services = ServiceFactory.CreateServices();
        var tempPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(tempPath);
        
        // Act
        var result = services.SemesterService.CreateAndActivateSemester(tempPath, "Test Semester");
        
        // Assert
        Assert.IsTrue(Directory.Exists(result));
    }
}
```

---

## Common Patterns

### Error Handling
```csharp
try
{
    var result = _semesterService.CreateAndActivateSemester(path, name);
    // Success handling
}
catch (InvalidOperationException ex)
{
    MessageBox.Show($"Operation failed: {ex.Message}", "Error", 
        MessageBoxButtons.OK, MessageBoxIcon.Error);
}
```

### Loading UI Components
```csharp
private void LoadSubjectsUI()
{
    try
    {
        _subjectService.LoadSubjectsUI(
            currentSemesterPath,
            subjectCardsPanel,
            CreateSubjectCard,
            () => subjectCardsPanel.Controls.Add(CreateEmptyLabel())
        );
    }
    catch (Exception ex)
    {
        ShowError($"Failed to load subjects: {ex.Message}");
    }
}
```

### Cascading Operations
```csharp
// Create repository -> Load tree -> Select repository
var repoPath = _repositoryService.CreateRepository(subjectPath, name, deadline);
LoadSubjectTree(repoPath);
SelectRepository(repoPath);
```

---

## FAQ

**Q: Why inject services instead of using static classes?**
A: Injection enables testing, loose coupling, and easy swapping of implementations.

**Q: Can I use different implementations of services?**
A: Yes! Create a new class implementing the interface and register it in ServiceFactory.

**Q: How do I add logging to services?**
A: Add a logging interface, inject it into services, and use it for logging without changing the UI.

**Q: What if a service method throws an exception?**
A: Handle it in MainForm's event handlers with try-catch blocks and user-friendly messages.

**Q: Can services call other services?**
A: Yes! Application services call domain services, domain services call infrastructure services. No cyclic dependencies.

---

## Troubleshooting

### "Service is null"
Check that ServiceFactory.CreateServices() is called in Program.cs before creating MainForm.

### "Interface method not found"
Make sure the method is defined in the interface AND implemented in the service class.

### "Parameter mismatch"
Check the exact signature of the interface method - parameter names and types must match.

### "Build errors after changes"
Run a clean build: Delete bin/obj folders and rebuild.

---

**Version:** 1.0
**Last Updated:** 2024
**Status:** ✅ Production Ready
