using DTOs;
using Entities;


namespace Mapper
{
    public class UserDTOMapper
    {
        public User Mapper(UserDTO userDto)
        {
            if (userDto == null) throw new ArgumentNullException(nameof(userDto));
            return new User
            {
                Id = userDto.Id,
                Name = userDto.Name,
                Email = userDto.Email,
                password = userDto.password,
                Picture = userDto.Picture
            };
        }
        public UserDTO Mapper(User user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            return new UserDTO
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                password = user.password,
                Picture = user.Picture
            };
        }
    }
}
