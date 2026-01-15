using Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Logic.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepo _userRepo;
        private readonly PasswordHasher<object> _hasher = new();

        public UserService(IUserRepo userRepo)
        {
            _userRepo = userRepo;
        }

        public int RegisterUser(string name, string email, string password, byte[]? picture)
        {
            var existing = _userRepo.GetByEmail(email);
            if (existing != null) throw new ArgumentException("Email is already registered.", nameof(email));
            string hash = _hasher.HashPassword(null!, password);
            return _userRepo.AddUser(name, email, hash, picture);
        }

        public (int Id, string Name, string Email, bool Admin)? ValidateLogin(string email, string password)
        {
            var user = _userRepo.GetByEmail(email);
            if (user == null) return null;
            PasswordVerificationResult result = _hasher.VerifyHashedPassword(null!, user.Value.Password, password);
            if (result != PasswordVerificationResult.Success) return null;
            return (user.Value.Id, user.Value.Name, user.Value.Email, user.Value.Admin);
        }

        public byte[]? GetUserPicture(long id)
        {
            return _userRepo.GetPictureById(id);
        }

        public void ChangePassword(long userId, string oldPassword, string newPassword)
        {
            var currentHash = _userRepo.GetPasswordHashById(userId);
            if (currentHash == null) throw new Exception("User not found.");
            var verify = _hasher.VerifyHashedPassword(null!, currentHash, oldPassword);
            if (verify != PasswordVerificationResult.Success) throw new Exception("Old password is incorrect.");
            var newHash = _hasher.HashPassword(null!, newPassword);
            _userRepo.UpdatePasswordHash(userId, newHash);
        }

        public List<(int Id, string Name)> GetUsersByIds(IEnumerable<int> userIds)
        {
            return _userRepo.GetUsersByIds(userIds);
        }
    }
}
