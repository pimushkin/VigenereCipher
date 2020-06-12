using NUnit.Framework;
using VigenereMessenger.Data;

namespace VigenereMessengerTest
{
    [TestFixture]
    public class Tests
    {
        private readonly Cipher RussianEncryptedText =
            new Cipher("Пример шифра Виженера на русском. And some text in English.", Cipher.LanguageMode.Russian,
                "скорпион", Cipher.CryptMode.Decrypt);
        private readonly Cipher EnglishEncryptedText =
            new Cipher("Пример шифра Виженера на русском. And some text in English.", Cipher.LanguageMode.English,
                "scorpion", Cipher.CryptMode.Decrypt);
        private readonly Cipher RussianDecryptedText =
            new Cipher("Юёъьхз йыгёс Сщюцауёс эр зедаааь. And some text in English.", Cipher.LanguageMode.Russian,
                "скорпион", Cipher.CryptMode.Encrypt);
        private readonly Cipher EnglishDecryptedText =
            new Cipher("Пример шифра Виженера на русском. Ilp bzeq gmvf ry Wzttgeq.", Cipher.LanguageMode.English,
                "scorpion", Cipher.CryptMode.Encrypt);
        [Test]
        public void DecodingTest()
        {

            var firstExpectedText = RussianEncryptedText.GetResultText();
            const string firstActualText = "Юёъьхз йыгёс Сщюцауёс эр зедаааь. And some text in English.";
            Assert.AreEqual(firstExpectedText, firstActualText);

            var secondExpectedText = EnglishEncryptedText.GetResultText();
            const string secondActualText = "Пример шифра Виженера на русском. Ilp bzeq gmvf ry Wzttgeq.";
            Assert.AreEqual(secondExpectedText, secondActualText);
        }
        [Test]
        public void EncryptingTest()
        {
            var firstExpectedText = RussianDecryptedText.GetResultText();
            const string actualText = "Пример шифра Виженера на русском. And some text in English.";
            Assert.AreEqual(firstExpectedText, actualText);

            var secondExpectedText = EnglishDecryptedText.GetResultText();
            Assert.AreEqual(secondExpectedText, actualText);
        }
    }
}