using DTOs;
using ViewModels;

namespace VmMapper
{
    public class RegisteredUserVmMapper
    {
        public RegisteredUserVM Mappper(UserDTO user)
        {
            return new RegisteredUserVM
            {
                Name = user.Name,
                Email = user.Email,
                Password = user.password,
            };
        }

        public UserDTO Mappper(RegisteredUserVM user)
        {
            return new UserDTO
            {
                Name = user.Name,
                Email = user.Email,
                password = user.Password,
            };
        }
    }
}
