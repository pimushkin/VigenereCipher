using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MatBlazor;

namespace VigenereMessenger.Data
{
    public static class UploadService
    {
        /// <summary>
        ///     Upload a file sent by the user.
        /// </summary>
        /// <param name="files">Downloadable files from which the first file will be downloaded.</param>
        /// <returns>The object that the downloaded file is stored inside.</returns>
        /// <exception cref="Exception">The file format or file size does not meet the upload condition.</exception>
        public static async Task<Document> UploadFileAsync(IEnumerable<IMatFileUploadEntry> files)
        {
            var file = files.FirstOrDefault();
            if (file == null) return null;
            var fileFormat = file.Name.Split('.').Last();
            if (!(fileFormat == "docx" || fileFormat == "txt"))
                throw new Exception("The file format is not supported. Upload DOCX or TXT.");

            if (file.Size > 1024 * 1024 * 10) throw new Exception("A file larger than 10 MB.");

            var uploadedFile = await new Document().UploadFile(file);
            return uploadedFile;
        }
    }
}