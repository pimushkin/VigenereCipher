using System;
using System.IO;
using System.Text;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

namespace VigenereCipher.Data
{
    public class DocumentService
    {
        public static string GetResultDocumentAsync(Document file, Cipher.LanguageMode language, string key,
            Cipher.CryptMode crypt)
        {
            if (file.GetContent() is byte[])
            {
                var stream = new MemoryStream();
                var result = new StringBuilder();
                if (!(file.GetContent() is byte[] bytesOfDocx)) throw new Exception("The file content was not detected.");
                stream.Write(bytesOfDocx, 0, bytesOfDocx.Length);
                using var wordDocument = WordprocessingDocument.Open(stream, true);
                var paragraphs = wordDocument.MainDocumentPart.Document.Body.Elements<Paragraph>();
                foreach (var paragraph in paragraphs)
                foreach (var run in paragraph.Elements<Run>())
                foreach (var text in run.Elements<Text>())
                    result.AppendLine(new Cipher(text.Text, language, key, crypt).GetResultText());

                return result.ToString();
            }

            if (!(file.GetContent() is string)) throw new Exception("The file content was not detected.");
            var content = file.GetContent() as string;
            return new Cipher(content, language, key, crypt).GetResultText();
        }
        
        public static byte[] DownloadDocxFromText(string outputText)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            if (string.IsNullOrWhiteSpace(outputText)) throw new Exception("The file content was not detected.");
            var ms = new MemoryStream();
            using var wordDocument =
                WordprocessingDocument.Create(ms, WordprocessingDocumentType.Document, true);

            var mainPart = wordDocument.AddMainDocumentPart();
            var body = new Body(new Paragraph(new Run(new Text(outputText))));
            mainPart.Document = new DocumentFormat.OpenXml.Wordprocessing.Document(body);
            wordDocument.Save();
            wordDocument.Close();
            return ms.ToArray();
        }
    }
}