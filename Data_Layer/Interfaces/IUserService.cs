

namespace Interfaces
{
    public interface IUserService
    {
        public int RegisterUser(string name, string email, string password, byte[]? picture);
        (long Id, string Name, string Email)? ValidateLogin(string email, string password);
        byte[]? GetUserPicture(long id);
    }
}
