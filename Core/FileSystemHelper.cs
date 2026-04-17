namespace IskolRepository.Core;

public static class FileSystemHelper
{
    public static bool IsValidName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return false;
        }

        var trimmedName = name.Trim();
        return trimmedName.IndexOfAny(Path.GetInvalidFileNameChars()) < 0
            && !trimmedName.EndsWith('.')
            && !trimmedName.EndsWith(' ');
    }

    public static void CreateDirectory(string path)
    {
        Directory.CreateDirectory(path);
    }

    public static string? CreateRepositoryFile(string repositoryPath, string fileName, string extension)
    {
        var filePath = Path.Combine(repositoryPath, fileName + extension);

        if (extension == ".txt")
        {
            File.WriteAllText(filePath, string.Empty);
            return filePath;
        }

        if (extension == ".docx")
        {
            using var stream = File.Create(filePath);
            return filePath;
        }

        return null;
    }
}
