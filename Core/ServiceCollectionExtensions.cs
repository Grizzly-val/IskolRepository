using System.Text.Json;
using IskolRepository.Core.Interfaces;
using IskolRepository.Core.Interfaces.Infrastructure;
using IskolRepository.Core.Services;
using IskolRepository.Core.Services.Infrastructure;

namespace IskolRepository.Core;

/// <summary>
/// Factory class for creating and configuring application services.
/// </summary>
public static class ServiceFactory
{
    public static ServiceRegistry CreateServices()
    {
        var jsonOptions = new JsonSerializerOptions { WriteIndented = true };

        var fileSystemHelper = new FileSystemService();
        var pathProvider = new PathProviderService();
        var validationHelper = new ValidationHelperService(fileSystemHelper);

        // Create file identity manager first (no dependencies on other services)
        var fileIdentityManager = new FileIdentityManager(fileSystemHelper, pathProvider);

        // Create reconciliation service (depends on fileIdentityManager)
        var fileReconciliationService = new FileReconciliationService(fileSystemHelper, pathProvider, fileIdentityManager);

        // Create repository service (depends on fileReconciliationService)
        var repositoryService = new RepositoryService(
            fileSystemHelper,
            validationHelper,
            pathProvider,
            jsonOptions,
            fileReconciliationService);

        var semesterService = new SemesterService(fileSystemHelper, pathProvider);

        // Pass new services to FileService
        var fileService = new FileService(
            fileSystemHelper,
            pathProvider,
            validationHelper,
            fileIdentityManager,
            repositoryService);

        var subjectService = new SubjectService(fileSystemHelper, pathProvider);
        var treeViewService = new TreeViewService(fileSystemHelper, pathProvider, validationHelper, repositoryService);
        
        // Pass new services to VersionService
        var versionService = new VersionService(jsonOptions, fileIdentityManager, repositoryService);

        return new ServiceRegistry(
            semesterService,
            repositoryService,
            fileService,
            subjectService,
            treeViewService,
            versionService,
            validationHelper,
            fileIdentityManager,
            fileReconciliationService);
    }
}

public sealed class ServiceRegistry
{
    public ISemesterService SemesterService { get; }
    public IRepositoryService RepositoryService { get; }
    public IFileService FileService { get; }
    public ISubjectService SubjectService { get; }
    public ITreeViewService TreeViewService { get; }
    public IVersionService VersionService { get; }
    public IValidationHelper ValidationService { get; }
    public IFileIdentityManager FileIdentityManager { get; }
    public IFileReconciliationService FileReconciliationService { get; }

    public ServiceRegistry(
        ISemesterService semesterService,
        IRepositoryService repositoryService,
        IFileService fileService,
        ISubjectService subjectService,
        ITreeViewService treeViewService,
        IVersionService versionService,
        IValidationHelper validationService,
        IFileIdentityManager fileIdentityManager,
        IFileReconciliationService fileReconciliationService)
    {
        SemesterService = semesterService ?? throw new ArgumentNullException(nameof(semesterService));
        RepositoryService = repositoryService ?? throw new ArgumentNullException(nameof(repositoryService));
        FileService = fileService ?? throw new ArgumentNullException(nameof(fileService));
        SubjectService = subjectService ?? throw new ArgumentNullException(nameof(subjectService));
        TreeViewService = treeViewService ?? throw new ArgumentNullException(nameof(treeViewService));
        VersionService = versionService ?? throw new ArgumentNullException(nameof(versionService));
        ValidationService = validationService ?? throw new ArgumentNullException(nameof(validationService));
        FileIdentityManager = fileIdentityManager ?? throw new ArgumentNullException(nameof(fileIdentityManager));
        FileReconciliationService = fileReconciliationService ?? throw new ArgumentNullException(nameof(fileReconciliationService));
    }
}
