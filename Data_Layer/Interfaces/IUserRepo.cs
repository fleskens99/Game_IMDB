using DTOs;

namespace Interfaces
{
    public interface IUserRepo
    {
        public int AddUser(string name, string email, string password, byte[]? picture);
        public (int Id, string Name, string Email, string Password, bool Admin)? GetByEmail(string email);
        byte[]? GetPictureById(long id);
        public void UpdatePasswordHash(long userId, string newPasswordHash);
        public string? GetPasswordHashById(long userId);
        public List<(int Id, string Name)> GetUsersByIds(IEnumerable<int> userIds);
    }
}
