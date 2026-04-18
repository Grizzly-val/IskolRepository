using System.Text.Json;
using IskolRepository.Core.Interfaces.Application;
using IskolRepository.Core.Interfaces.Domain;
using IskolRepository.Core.Interfaces.Infrastructure;
using IskolRepository.Core.Services.Application;
using IskolRepository.Core.Services.Domain;
using IskolRepository.Core.Services.Infrastructure;

namespace IskolRepository.Core;

/// <summary>
/// Factory class for creating and configuring application services.
/// </summary>
public static class ServiceFactory
{
    /// <summary>
    /// Creates and returns all configured application services.
    /// Use this to bootstrap the application with dependencies.
    /// </summary>
    public static ApplicationServices CreateServices()
    {
        // Configuration
        var jsonOptions = new JsonSerializerOptions { WriteIndented = true };
        var validStatuses = new[] { "in-progress", "completed", "late" };

        // Infrastructure services
        var fileSystemHelper = new FileSystemService();
        var pathProvider = new PathProviderService();
        var validationHelper = new ValidationHelperService(fileSystemHelper);

        
        var repositoryDomainService = new RepositoryDomainService(
            fileSystemHelper,
            validationHelper,
            pathProvider,
            jsonOptions,
            validStatuses);

        // Domain services
        var semesterDomainService = new SemesterDomainService(fileSystemHelper, pathProvider);
        var fileDomainService = new FileDomainService(fileSystemHelper, pathProvider, validationHelper);
        var subjectDomainService = new SubjectDomainService(fileSystemHelper, pathProvider);
        var treeViewDomainService = new TreeViewDomainService(fileSystemHelper, pathProvider, validationHelper);
        var versionDomainService = new VersionDomainService(jsonOptions);

        // Application services
        var semesterAppService = new SemesterApplicationService(semesterDomainService, treeViewDomainService);
        var repositoryAppService = new RepositoryApplicationService(repositoryDomainService);
        var fileAppService = new FileApplicationService(fileDomainService, versionDomainService);
        var subjectAppService = new SubjectApplicationService(subjectDomainService);

        return new ApplicationServices(
            semesterAppService,
            repositoryAppService,
            fileAppService,
            fileDomainService,
            subjectAppService,
            treeViewDomainService,
            versionDomainService,
            validationHelper);
    }
}

/// <summary>
/// Container for all application services.
/// </summary>
public sealed class ApplicationServices
{
    public ISemesterApplicationService SemesterService { get; }
    public IRepositoryApplicationService RepositoryService { get; }
    public IFileApplicationService FileService { get; }
    public IFileDomainService FileDomainService { get; }
    public ISubjectApplicationService SubjectService { get; }
    public ITreeViewDomainService TreeViewService { get; }
    public IVersionDomainService VersionService { get; }
    public IValidationHelper ValidationService { get; }

    public ApplicationServices(
        ISemesterApplicationService semesterService,
        IRepositoryApplicationService repositoryService,
        IFileApplicationService fileService,
        IFileDomainService fileDomainService,
        ISubjectApplicationService subjectService,
        ITreeViewDomainService treeViewService,
        IVersionDomainService versionService,
        IValidationHelper validationService)
    {
        SemesterService = semesterService ?? throw new ArgumentNullException(nameof(semesterService));
        RepositoryService = repositoryService ?? throw new ArgumentNullException(nameof(repositoryService));
        FileService = fileService ?? throw new ArgumentNullException(nameof(fileService));
        FileDomainService = fileDomainService ?? throw new ArgumentNullException(nameof(fileDomainService));
        SubjectService = subjectService ?? throw new ArgumentNullException(nameof(subjectService));
        TreeViewService = treeViewService ?? throw new ArgumentNullException(nameof(treeViewService));
        VersionService = versionService ?? throw new ArgumentNullException(nameof(versionService));
        ValidationService = validationService ?? throw new ArgumentNullException(nameof(validationService));
    }
}
