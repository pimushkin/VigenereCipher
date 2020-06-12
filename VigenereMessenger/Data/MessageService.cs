using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Messenger.Data.Messenger;
using Microsoft.EntityFrameworkCore;

namespace VigenereMessenger.Data
{
    public class MessageService
    {
        private readonly MessengerContext _messageContext;

        public MessageService(MessengerContext messageContext)
        {
            _messageContext = messageContext;
        }

        /// <summary>
        ///     Get a list of all messages sent to the current user.
        /// </summary>
        /// <param name="currentUser">Name of the user who was logged in.</param>
        /// <returns>Message list.</returns>
        public async Task<List<Messages>> GetIncomingMessageAsync(string currentUser)
        {
            return await _messageContext.Messages.Where(x => x.Receiver == currentUser).AsNoTracking().ToListAsync();
        }

        /// <summary>
        ///     Get a list of all messages sent by the current user.
        /// </summary>
        /// <param name="currentUser">Name of the user who was logged in.</param>
        /// <returns>Message list.</returns>
        public async Task<List<Messages>> GetSentMessageAsync(string currentUser)
        {
            return await _messageContext.Messages.Where(x => x.Sender == currentUser).AsNoTracking().ToListAsync();
        }

        /// <summary>
        ///     Adds the sent message to the database.
        /// </summary>
        /// <param name="message">Message to send.</param>
        /// <returns></returns>
        public void
            CreateMessageAsync(Messages message)
        {
            if (string.IsNullOrWhiteSpace(message.Topic) ||
                string.IsNullOrWhiteSpace(message.Message) ||
                string.IsNullOrWhiteSpace(message.Receiver) ||
                string.IsNullOrWhiteSpace(message.Sender))
                throw new Exception("One or more fields are empty for sending a message.");

            if (message.Topic.Length > 30)
                throw new Exception("The topic must not be longer than 30 characters.");
            _messageContext.Messages.Add(message);
            _messageContext.SaveChanges();
        }
    }
}