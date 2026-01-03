using DTOs;
using Presentation.ViewModels;
using ViewModels;

namespace VmMapper
{
    public class LoggedUserVmMapper
    {
        public LogedInVm Mappper(UserDTO user)
        {
            return new LogedInVm
            {
                Email = user.Email,
                Password = user.password,
            };
        }

        public UserDTO Mappper(LogedInVm user)
        {
            return new UserDTO
            {
                Email = user.Email,
                password = user.Password,
            };
        }
    }
}
