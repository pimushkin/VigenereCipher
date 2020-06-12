using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Messenger.Data.Messenger;

namespace VigenereMessenger.Data
{
    public class UserService
    {
        private readonly MessengerContext _context;

        public UserService(MessengerContext context)
        {
            _context = context;
        }

        /// <summary>
        ///     Function for checking the existence of the entered username in the database.
        /// </summary>
        /// <param name="username">Name of the user to check for existence.</param>
        /// <returns>Returns true if the user exists and returns false if the user does not exist.</returns>
        public async Task<bool> CheckUserExistence(string username)
        {
            return (await _context.AspNetUsers.Where(x => x.UserName == username).AsNoTracking().ToListAsync())
                   .Count == 1;
        }
    }
}