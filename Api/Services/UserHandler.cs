using Api.Services.Interfaces;

namespace Api.Services
{
    public class UserHandler : IUserHandler
    {
        private static readonly List<string> usernames = new();
        public bool UsernameIsAlreadyInUse(string username)
        {
            return usernames.Contains(username);
        }
        public bool TryAddUser(string username)
        {
            if (usernames.Contains(username))
            {
                return false;
            }

            usernames.Add(username);
            return true;
        }

        public void TryRemoveUser(string username)
        {
            if (username.Contains(username))
            {
                _ = usernames.Remove(username);
            }
        }
    }
}