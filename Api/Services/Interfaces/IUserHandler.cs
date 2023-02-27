namespace Api.Services.Interfaces
{
    public interface IUserHandler
    {
        public bool UsernameIsAlreadyInUse(string username);
        public bool TryAddUser(string username);
        public void TryRemoveUser(string username);
    }
}