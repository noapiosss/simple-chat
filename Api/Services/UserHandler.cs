using Api.Services.Interfaces;

namespace Api.Services
{
    public class UserHandler : IUserHandler
    {
        private static readonly List<string> usernames = new();
        public bool TryLogin(string username)
        {
            if (usernames.Contains(username))
            {
                return false;
            }

            usernames.Add(username);
            return true;
        }

        public void Logout(string username)
        {
            _ = usernames.Remove(username);
        }
    }
}