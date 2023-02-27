namespace Api.Services.Interfaces
{
    public interface IUserHandler
    {
        public bool TryLogin(string username);
        public void Logout(string username);
    }
}