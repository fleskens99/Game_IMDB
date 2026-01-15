using DTOs;
using ViewModels;

namespace VmMapper
{
    public class UserVmMapper
    {
        public static UserAccountViewModel ToUserAccountViewModel(UserDTO userDto)
        {
            return new UserAccountViewModel
            {
                Name = userDto.Name,
                Email = userDto.Email
            };
        }
    }
}
