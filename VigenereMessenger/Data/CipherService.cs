using System.Threading.Tasks;

namespace VigenereMessenger.Data
{
    /// <summary>
    ///     Class for working with cipher.
    /// </summary>
    public class CipherService
    {
        /// <summary>
        ///     Asynchronous method that returns the result of encrypting or decrypting text.
        /// </summary>
        /// <param name="text">Text that will be encrypted or decrypted.</param>
        /// <param name="language">The language in which the key was entered.</param>
        /// <param name="key">Key for encryption or decryption.</param>
        /// <param name="crypt">Mode of text conversion(encryption or decryption).</param>
        /// <returns>Text as a result of encryption or decryption.</returns>
        public static Task<string> GetResultTextAsync(string text, Cipher.LanguageMode language, string key,
            Cipher.CryptMode crypt)
        {
            return Task.FromResult(new Cipher(text, language, key, crypt).GetResultText());
        }
    }
}