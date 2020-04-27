using System.Threading.Tasks;

namespace VigenereCipher.Data
{
    public class CipherService
    {
        public static Task<string> GetResultTextAsync(string inputText,  Cipher.LanguageMode language, string key, Cipher.CryptMode crypt)
        {
            return Task.FromResult(new Cipher(inputText, language, key, crypt).GetResultText());
        } 
    }
}