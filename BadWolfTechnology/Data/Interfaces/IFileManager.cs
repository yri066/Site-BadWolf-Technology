namespace BadWolfTechnology.Data.Interfaces
{
    public interface IFileManager
    {
        bool CheckIsImage(IFormFile file);
        string GetFileExtension(IFormFile file);
        Task SaveFile(string path, string fileName, IFormFile file);
        Task<string> UploadImageAsync(string folderName, string? fileName, IFormFile image);
        void MoveImage(string sourceFolderName, string destFolderName, string fileName);
    }
}
