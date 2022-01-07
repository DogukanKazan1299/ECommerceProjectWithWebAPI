using Business.Abstract;
using Business.Constants;
using Core.Utilities.Responses;
using Core.Utilities.Security.Token;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.Dtos.UserDtos;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class UserManager : IUserService
    {
        IUserDal _userDal;
        AppSettings _appSettings;
        public UserManager(IUserDal userDal,IOptions<AppSettings> appSettings)
        {
            _userDal = userDal;
            _appSettings = appSettings.Value;
        }
        public async Task<ApiDataResponse<UserDto>> AddAsync(UserAddDto userAddDto)
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
            return new SuccessApiDataResponse<UserDto>(userDto, Messages.Added);
        }

        public async Task<AccessToken> Authenticate(UserForLoginDto userForLoginDto)
        {
            var user = await _userDal.GetAsync(x => x.UserName == userForLoginDto.UserName && x.Password == userForLoginDto.Password);
            if (user == null)
            {
                return null;
            }
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.SecurityKey);
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new[]
               {
                    new Claim(ClaimTypes.Name,user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            AccessToken accessToken = new AccessToken()
            {
                Token = tokenHandler.WriteToken(token),
                Expiration = (DateTime)tokenDescriptor.Expires,
                UserName = user.UserName,
                UserID = user.Id
            };
            return await Task.Run(() => accessToken);
        }

        public async Task<ApiDataResponse<bool>> DeleteAsync(int id)
        {
            var isDelete = await _userDal.DeleteAsync(id);
            return new SuccessApiDataResponse<bool>(isDelete, Messages.Deleted);
        }

        public async Task<ApiDataResponse<UserDto>> GetByIdAsync(int id)
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
            return new SuccessApiDataResponse<UserDto>(userDto, Messages.Listed);
        }

        public async Task<ApiDataResponse<IEnumerable<UserDetailDto>>> GetListAsync()
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
            return new SuccessApiDataResponse<IEnumerable<UserDetailDto>>(userDetailDtos, Messages.Listed);
        }

        public async Task<ApiDataResponse<UserUpdateDto>> UpdateAsync(UserUpdateDto userUpdateDto)
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
            return new SuccessApiDataResponse<UserUpdateDto>(newUserUpdateDto, Messages.Updated);
        }
    }
}
