using System;
using System.IO;
using System.Linq;
using System.Text;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

namespace VigenereMessenger.Data
{
    /// <summary>
    ///     Class for working with DOCX and TXT files.
    /// </summary>
    public class DocumentService
    {
        /// <summary>
        ///     Method for getting encrypted or decrypted text from a file.
        /// </summary>
        /// <param name="file">A DOCX or TXT file.</param>
        /// <param name="language">The language in which the key was entered.</param>
        /// <param name="key">Key for encryption or decryption.</param>
        /// <param name="crypt">Mode of text conversion(encryption or decryption).</param>
        /// <returns>Text as a result of encryption or decryption.</returns>
        /// <exception cref="Exception">Error reading the file.</exception>
        public static string GetResultFile(Document file, Cipher.LanguageMode language, string key,
            Cipher.CryptMode crypt)
        {
            if (file.GetContent() is byte[] bytesOfDocx)
            {
                using var stream = new MemoryStream();
                var result = new StringBuilder();
                stream.Write(bytesOfDocx, 0, bytesOfDocx.Length);
                using var wordDocument = WordprocessingDocument.Open(stream, true);
                var paragraphs = wordDocument.MainDocumentPart.Document.Body.Elements<Paragraph>();
                foreach (var paragraph in paragraphs)
                foreach (var run in paragraph.Elements<Run>())
                foreach (var text in run.Elements<Text>())
                    result.AppendLine(new Cipher(text.Text, language, key, crypt).GetResultText());

                return result.ToString();
            }

            if (!(file.GetContent() is string content)) throw new Exception("The file content was not detected.");
            return new Cipher(content, language, key, crypt).GetResultText();
        }

        /// <summary>
        ///     Method for getting DOCX file as bytes from text.
        /// </summary>
        /// <param name="text">Text that will be placed inside the file.</param>
        /// <returns>Array of bytes of the DOCX file.</returns>
        /// <exception cref="Exception">The input text is empty.</exception>
        public static byte[] GetDocxBytesFromText(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) throw new Exception("The file content was not detected.");
            using var ms = new MemoryStream();
            using var wordDocument =
                WordprocessingDocument.Create(ms, WordprocessingDocumentType.Document, true);

            var mainPart = wordDocument.AddMainDocumentPart();
            mainPart.Document = new DocumentFormat.OpenXml.Wordprocessing.Document();
            var body = mainPart.Document.AppendChild(new Body());
            var para = body.AppendChild(new Paragraph());
            var run = para.AppendChild(new Run());
            var texts = text.Split(new[] {"\r\n", "\n"}, StringSplitOptions.None).ToList();
            if (texts.Last() == "") texts.RemoveAt(texts.Count - 1);
            for (var i = 0; i < texts.Count; i++)
            {
                if (i > 0)
                    run.AppendChild(new Break());

                var newText = new Text {Text = texts[i]};
                run.AppendChild(newText);
            }

            wordDocument.Close();
            return ms.ToArray();
        }
    }
}