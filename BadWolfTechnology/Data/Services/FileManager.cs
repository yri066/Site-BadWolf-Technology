using BadWolfTechnology.Data.Interfaces;

namespace BadWolfTechnology.Data.Services
{
    public class FileManager : IFileManager
    {
        /// <summary>
        /// Проверка что передаваемый файл является изображением.
        /// </summary>
        /// <param name="file">Передаваемый файл.</param>
        /// <returns>true - файл является изображением, false - файл другого типа.</returns>
        public bool CheckIsImage(IFormFile file)
        {
            string[] contentTypeImage = { "image/jpg", "image/jpeg", "image/png", "image/webp" };

            return contentTypeImage.Any(ext => file.ContentType.Contains(ext));
        }

        /// <summary>
        /// Получить расширение файла.
        /// </summary>
        /// <param name="file">Передаваемый файл.</param>
        /// <returns>Расширение файла.</returns>
        public string GetFileExtension(IFormFile file)
        {
            return Path.GetExtension(file.FileName);
        }

        /// <summary>
        /// Сохранить файл в указанную папку.
        /// </summary>
        /// <param name="path">Путь к папке.</param>
        /// <param name="fileName">Имя файла с расширением.</param>
        /// <param name="file">Передаваемый файл.</param>
        /// <returns></returns>
        public async Task SaveFile(string path, string fileName, IFormFile file)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            var filePath = Path.Combine(path, fileName);

            using (var stream = File.Create(filePath))
            {
                await file.CopyToAsync(stream);
            }
        }

        /// <summary>
        /// Загрузить изображение в корневой каталог изображений.
        /// </summary>
        /// <param name="folderName">Название папки.</param>
        /// <param name="fileName">Название файла.</param>
        /// <param name="image">Передаваемый файл.</param>
        /// <returns></returns>
        /// <exception cref="Exception">Не удалось сохранить файл.</exception>
        public async Task<string> UploadImageAsync(string folderName, string? fileName, IFormFile image)
        {
            if (image != null && !CheckIsImage(image))
            {
                throw new Exception("Данный тип файла не разрешен.");
            }

            var dirPath = Path.Combine(Environment.CurrentDirectory, "wwwroot", "image", folderName);


            var fileExtension = GetFileExtension(image);
            fileName ??= Guid.NewGuid() + fileExtension;

            //Загрузить файл.
            try
            {
                await SaveFile(dirPath, fileName, image);
            }
            catch
            {
                throw new Exception("Не удалось сохранить файл.");
            }

            return fileName;
        }

        /// <summary>
        /// Переместить файл между папками в корневой каталог изображений.
        /// </summary>
        /// <param name="sourceFolderName">Имя исходной папки.</param>
        /// <param name="destFolderName">Имя новой папки.</param>
        /// <param name="fileName">Имя файла с расширением.</param>
        /// <exception cref="Exception">Не удалось сохранить файл.</exception>
        public void MoveImage(string sourceFolderName, string destFolderName, string fileName)
        {
            var path = Path.Combine(Environment.CurrentDirectory, "wwwroot", "image", destFolderName);

            if(!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            var sourceFileName = Path.Combine(Environment.CurrentDirectory, "wwwroot", "image", sourceFolderName, fileName);
            var destFileName = Path.Combine(path, fileName);

            //Переместить файл.
            try
            {
                File.Move(sourceFileName, destFileName, true);
            }
            catch
            {
                throw new Exception("Не удалось сохранить файл.");
            }
        }
    }
}
