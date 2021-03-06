﻿using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MatBlazor;

namespace VigenereMessenger.Data
{
    public class Document
    {
        private byte[] _bytesOfDocx;
        private string _txtContent;

        private byte[] BytesOfFile
        {
            get => _bytesOfDocx;
            set
            {
                _txtContent = null;
                _bytesOfDocx = value;
            }
        }

        private string TxtContent
        {
            get => _txtContent;
            set
            {
                _bytesOfDocx = null;
                _txtContent = value;
            }
        }

        /// <summary>
        ///     Method for uploading the file.
        /// </summary>
        /// <param name="file">DOCX ot TXT file.</param>
        /// <returns>Object of the class that the file is stored inside.</returns>
        /// <exception cref="Exception">Error in the file format that is being uploaded.</exception>
        public async Task<Document> UploadFile(IMatFileUploadEntry file)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            await using var stream = new MemoryStream();
            await file.WriteToStreamAsync(stream);
            stream.Seek(0, SeekOrigin.Begin);
            var fileFormat = file.Name.Split('.').Last();
            switch (fileFormat)
            {
                case "docx":
                {
                    using var reader = new StreamReader(stream, Encoding.GetEncoding("Windows-1252"));
                    var inputText = await reader.ReadToEndAsync();
                    BytesOfFile = Encoding.GetEncoding("Windows-1252").GetBytes(inputText);
                    return new Document {BytesOfFile = BytesOfFile};
                }
                case "txt":
                {
                    using var reader = new StreamReader(stream, Encoding.GetEncoding("Windows-1251"));
                    TxtContent = await reader.ReadToEndAsync();
                    return new Document {TxtContent = TxtContent};
                }
                default:
                    throw new Exception("The file format is not supported. Upload DOCX or TXT.");
            }
        }

        /// <summary>
        ///     Method for getting a file as a text or byte array.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception">The file is empty.</exception>
        public dynamic GetContent()
        {
            if (BytesOfFile != null) return BytesOfFile;

            if (TxtContent != null) return TxtContent;

            throw new Exception("Content is missing. You need to download the file first.");
        }
    }
}