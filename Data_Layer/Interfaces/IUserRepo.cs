using DTOs;

namespace Interfaces
{
    public interface IUserRepo
    {
        public int AddUser(string name, string email, string password, byte[]? picture);
        public (long Id, string Name, string Email, string Password)? GetByEmail(string email);
        byte[]? GetPictureById(long id);
    }
}
