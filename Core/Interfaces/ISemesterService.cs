namespace IskolRepository.Core.Interfaces;

public interface ISemesterService
{
    string OpenSemester(string selectedPath);

    string CreateSemester(string parentPath, string semesterName);

    void CreateSemesterMarker(string semesterPath);
}
