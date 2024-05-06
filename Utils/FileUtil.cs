using ptdn_net.Data.Dto;

namespace ptdn_net.Utils;

public static class FileUtil
{
    public static readonly string UploadFilesPath = Path.Combine("Upload", "Files");
    public static readonly string UploadImagePath = Path.Combine("wwwroot", "images");

    public static FileUploadInfo GetFileUploadInfo(IFormFile file)
    {
        var fileExtension = Path.GetExtension(file.FileName);
        var uniqueFileName = $"{Guid.NewGuid()}.{fileExtension}";

        return new FileUploadInfo
        {
            FileName = file.FileName,
            ContentType = file.ContentType,
            ContentSize = (int)file.Length,
            Extension = fileExtension,
            UniqueFileName = uniqueFileName
        };
    }

    public static async Task WriteFile(IFormFile file, string absoluteFilePath)
    {
        EnsureDirectoryExists(absoluteFilePath);

        await using var fileStream = new FileStream(absoluteFilePath, FileMode.Create);
        await file.CopyToAsync(fileStream);
    }

    public static void DeleteFile(string filePath)
    {
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
    }

    private static void EnsureDirectoryExists(string filePath)
    {
        var directory = Path.GetDirectoryName(filePath);
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory!);
        }
    }
}