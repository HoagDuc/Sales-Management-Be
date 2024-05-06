namespace ptdn_net.Data.Dto;

public class FileUploadInfo
{
    public string Extension { get; set; } = null!;

    public string FileName { get; set; } = null!;

    public int ContentSize { get; set; }

    public string ContentType { get; set; } = null!;

    public string UniqueFileName { get; set; } = null!;
}