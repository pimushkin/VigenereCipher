using System;
using MatBlazor;

namespace VigenereCipher.Data
{
    public static class MatBlazorExtension
    {
        /// <summary>
        ///     Displays a notification with information about successful encryption or decryption.
        /// </summary>
        /// <param name="notification"></param>
        /// <param name="crypt">Mode of text conversion(encryption or decryption).</param>
        /// <exception cref="ArgumentOutOfRangeException">Unknown conversion mode.</exception>
        public static void ShowSuccessCrypt(this IMatToaster notification, Cipher.CryptMode crypt)
        {
            switch (crypt)
            {
                case Cipher.CryptMode.Decrypt:
                    notification.Add("The text has been successfully decrypted.", MatToastType.Success, "Successfully");
                    break;
                case Cipher.CryptMode.Encrypt:
                    notification.Add("The text has been successfully encrypted.", MatToastType.Success, "Successfully");
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(crypt), crypt, null);
            }
        }

        /// <summary>
        ///     Displays a notification with information about successful file upload.
        /// </summary>
        /// <param name="notification"></param>
        public static void ShowSuccessUploading(this IMatToaster notification)
        {
            notification.Add("The file was uploaded successfully.", MatToastType.Success, "Successfully");
        }

        /// <summary>
        ///     Display a notification with information about the error.
        /// </summary>
        /// <param name="notification"></param>
        /// <param name="errorMessage">Text of the error that will be displayed inside the notification.</param>
        public static void ShowError(this IMatToaster notification, string errorMessage)
        {
            notification.Add(errorMessage, MatToastType.Danger, "Error");
        }
    }
}