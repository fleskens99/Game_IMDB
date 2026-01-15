using DTOs;
using Presentation.ViewModels.Account;

namespace ViewModels
{
    public class LoggedUserVmMapper
    {
        public LogedInVm Mappper(UserDTO user)
        {
            return new LogedInVm
            {
                Email = user.Email,
                Password = user.password,
                Admin = user.Admin,
            };
        }

        public UserDTO Mappper(LogedInVm user)
        {
            return new UserDTO
            {
                Email = user.Email,
                password = user.Password,
                Admin = user.Admin,
            };
        }
    }
}
