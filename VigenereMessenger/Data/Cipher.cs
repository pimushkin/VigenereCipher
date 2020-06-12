using System;
using System.Linq;
using System.Text;

namespace VigenereMessenger.Data
{
    public class Cipher
    {
        public enum CryptMode
        {
            Encrypt,
            Decrypt
        }

        public enum LanguageMode
        {
            Russian,
            English
        }

        private string _inputText;
        private string _key;

        private LanguageMode _language;

        public Cipher(string inputText, LanguageMode language, string key, CryptMode cryptCrypt)
        {
            InputText = inputText;
            Language = language;
            Key = key;
            Crypt = cryptCrypt;
        }

        private CryptMode Crypt { get; }

        private string Alphabet { get; set; }

        private LanguageMode Language
        {
            get => _language;
            set
            {
                _language = value;
                Alphabet = value switch
                {
                    LanguageMode.Russian => "абвгдеёжзийклмнопрстуфхцчшщъыьэюя",
                    LanguageMode.English => "abcdefghijklmnopqrstuvwxyz",
                    _ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
                };
            }
        }

        private string InputText
        {
            get => _inputText;
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                    _inputText = value;
                else
                    throw new Exception("The input text cannot be empty.");
            }
        }

        private string Key
        {
            get => _key;
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    _key = value.ToLower();
                    switch (Language)
                    {
                        case LanguageMode.Russian:
                            if (_key.Any(symbol => !Alphabet.Contains(symbol)))
                                throw new Exception("The key must contain only characters of the Russian alphabet.");
                            break;
                        case LanguageMode.English:
                            if (_key.Any(symbol => !Alphabet.Contains(symbol)))
                                throw new Exception("The key must contain only characters of the English alphabet.");
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
                else
                {
                    throw new Exception("The key cannot be empty.");
                }
            }
        }

        /// <summary>
        ///     Get the result of encrypting or decrypting the text.
        /// </summary>
        /// <returns>Text as a result of encryption or decryption.</returns>
        /// <exception cref="Exception"></exception>
        public string GetResultText()
        {
            try
            {
                var stringBuilder = new StringBuilder(InputText);
                var inputTextIndex = 0;
                var outputTextIndex = 0;
                foreach (var character in InputText)
                {
                    if (!Alphabet.Contains(char.ToLower(character)))
                    {
                        inputTextIndex++;
                        continue;
                    }

                    var alphabetIndex = Alphabet.IndexOf(char.ToLower(character));
                    var keyIndex = Alphabet.IndexOf(Key[outputTextIndex % Key.Length]);
                    if (Crypt == CryptMode.Encrypt) alphabetIndex = (keyIndex + alphabetIndex) % Alphabet.Length;
                    else alphabetIndex = (alphabetIndex - keyIndex + Alphabet.Length) % Alphabet.Length;
                    stringBuilder.Replace(character,
                        char.IsUpper(character)
                            ? char.ToUpper(Alphabet[alphabetIndex])
                            : char.ToLower(Alphabet[alphabetIndex]),
                        inputTextIndex, 1);
                    outputTextIndex++;
                    inputTextIndex++;
                }

                return stringBuilder.ToString();
            }
            catch (Exception)
            {
                throw new Exception("Error during encryption/decryption of the text.");
            }
        }
    }
}