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

        var repositoryService = new RepositoryService(
            fileSystemHelper,
            validationHelper,
            pathProvider,
            jsonOptions);

        var semesterService = new SemesterService(fileSystemHelper, pathProvider);
        var fileService = new FileService(fileSystemHelper, pathProvider, validationHelper);
        var subjectService = new SubjectService(fileSystemHelper, pathProvider);
        var treeViewService = new TreeViewService(fileSystemHelper, pathProvider, validationHelper);
        var versionService = new VersionService(jsonOptions);

        return new ServiceRegistry(
            semesterService,
            repositoryService,
            fileService,
            subjectService,
            treeViewService,
            versionService,
            validationHelper);
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

    public ServiceRegistry(
        ISemesterService semesterService,
        IRepositoryService repositoryService,
        IFileService fileService,
        ISubjectService subjectService,
        ITreeViewService treeViewService,
        IVersionService versionService,
        IValidationHelper validationService)
    {
        SemesterService = semesterService ?? throw new ArgumentNullException(nameof(semesterService));
        RepositoryService = repositoryService ?? throw new ArgumentNullException(nameof(repositoryService));
        FileService = fileService ?? throw new ArgumentNullException(nameof(fileService));
        SubjectService = subjectService ?? throw new ArgumentNullException(nameof(subjectService));
        TreeViewService = treeViewService ?? throw new ArgumentNullException(nameof(treeViewService));
        VersionService = versionService ?? throw new ArgumentNullException(nameof(versionService));
        ValidationService = validationService ?? throw new ArgumentNullException(nameof(validationService));
    }
}
