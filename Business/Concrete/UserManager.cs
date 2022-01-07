using Business.Abstract;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.Dtos.UserDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class UserManager : IUserService
    {
        IUserDal _userDal;
        public UserManager(IUserDal userDal)
        {
            _userDal = userDal;
        }
        public async Task<UserDto> AddAsync(UserAddDto userAddDto)
        {
            User user = new User()
            {
                UserName = userAddDto.UserName,
                FirstName = userAddDto.FirstName,
                LastName = userAddDto.LastName,
                Password = userAddDto.Password,
                Gender = userAddDto.Gender,
                Email = userAddDto.Email,
                DateOfBirth = userAddDto.DateOfBirth,
                CreatedUserId = 1,
                CreatedDate = DateTime.Now,
                Address = userAddDto.Address
            };
            var userAdd = await _userDal.AddAsync(user);
            UserDto userDto = new UserDto()
            {
                LastName = userAdd.LastName,
                Address = userAdd.Address,
                DateOfBirth = userAdd.DateOfBirth,
                Email = userAdd.Email,
                FirstName = userAdd.FirstName,
                Gender = userAdd.Gender,
                UserName = userAdd.UserName,
                Id = userAdd.Id
            };
            return userDto;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _userDal.DeleteAsync(id);
        }

        public async Task<UserDto> GetByIdAsync(int id)
        {
            var user = await _userDal.GetAsync(x => x.Id == id);
            UserDto userDto = new UserDto()
            {
                Id = user.Id,
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Password=user.Password,
                Gender = user.Gender,
                Address = user.Address,
                Email = user.Email,
                DateOfBirth = user.DateOfBirth
            };
            return userDto;
        }

        public async Task<IEnumerable<UserDetailDto>> GetListAsync()
        {
            List<UserDetailDto> userDetailDtos = new List<UserDetailDto>();
            var response = await _userDal.GetListAsync();
            foreach (var item in response.ToList())
            {
                userDetailDtos.Add(new UserDetailDto()
                {
                    Id=item.Id,
                    UserName= item.UserName,
                    FirstName= item.FirstName,
                    LastName= item.LastName,
                    Gender= item.Gender==true?"Erkek" : "Kadın",
                    DateOfBirth= item.DateOfBirth,
                    Address= item.Address,
                    Email= item.Email
                });
            }
            return userDetailDtos;
        }

        public async Task<UserUpdateDto> UpdateAsync(UserUpdateDto userUpdateDto)
        {
            var getUser = await _userDal.GetAsync(x => x.Id == userUpdateDto.Id);
            User user = new User()
            {
                LastName = userUpdateDto.LastName,
                Address = userUpdateDto.Address,
                DateOfBirth = userUpdateDto.DateOfBirth,
                Email = userUpdateDto.Email,
                FirstName = userUpdateDto.FirstName,
                Gender = userUpdateDto.Gender,
                UserName = userUpdateDto.UserName,
                Id = userUpdateDto.Id,
                Password = userUpdateDto.Password,
                CreatedDate = getUser.CreatedDate,
                CreatedUserId = getUser.CreatedUserId,
                UpdatedDate = DateTime.Now,
                UpdateUserId = 1
            };
            var userUpdate = await _userDal.UpdateAsync(user);
            UserUpdateDto newUserUpdateDto = new UserUpdateDto()
            {
                LastName = userUpdate.LastName,
                Address = userUpdate.Address,
                DateOfBirth = userUpdate.DateOfBirth,
                Email = userUpdate.Email,
                FirstName = userUpdate.FirstName,
                Gender = userUpdate.Gender,
                UserName = userUpdate.UserName,
                Id = userUpdate.Id,
                Password = userUpdate.Password
            };
            return newUserUpdateDto;
        }
    }
}
