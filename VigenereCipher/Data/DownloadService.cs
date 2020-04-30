using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace VigenereCipher.Data
{
    public static class DownloadService
    {
        /// <summary>
        ///     Download a DOCX file by the client asynchronously.
        /// </summary>
        /// <param name="jsRuntime"></param>
        /// <param name="savedFileName">Name of the file to be saved.</param>
        /// <param name="text">Text that will be saved inside the file.</param>
        /// <returns></returns>
        /// <exception cref="Exception">Error related to an empty value inside the arguments.</exception>
        public static async Task DownloadDocxFileAsync(this IJSRuntime jsRuntime, string savedFileName,
            string text)
        {
            if (string.IsNullOrWhiteSpace(savedFileName) || string.IsNullOrWhiteSpace(text))
                throw new Exception("There is no file name or result to save.");

            var newFileName = Path.GetInvalidFileNameChars().Aggregate(savedFileName,
                (current, invalidChar) => current.Replace(invalidChar.ToString(), "_"));
            var content = DocumentService.GetDocxBytesFromText(text);
            await jsRuntime.InvokeAsync<object>(
                "DocxFileSaveAs",
                newFileName + ".docx",
                Convert.ToBase64String(content)
            );
        }

        /// <summary>
        ///     Download a TXT file by the client asynchronously.
        /// </summary>
        /// <param name="jsRuntime"></param>
        /// <param name="savedFileName">Name of the file to be saved.</param>
        /// <param name="text">Text that will be saved inside the file.</param>
        /// <returns></returns>
        /// <exception cref="Exception">Error related to an empty value inside the arguments.</exception>
        public static async Task DownloadTxtFileAsync(this IJSRuntime jsRuntime, string savedFileName,
            string text)
        {
            if (string.IsNullOrWhiteSpace(savedFileName) || string.IsNullOrWhiteSpace(text))
                throw new Exception("There is no file name or result to save.");

            var newFileName = Path.GetInvalidFileNameChars().Aggregate(savedFileName,
                (current, invalidChar) => current.Replace(invalidChar.ToString(), "_"));

            await jsRuntime.InvokeAsync<object>(
                "FileSaveAs",
                newFileName + ".txt",
                text
            );
        }
    }
}