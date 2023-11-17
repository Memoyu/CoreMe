using CoreMe.Core.AOP.Attributes;
using CoreMe.Core.Domains.Common;
using CoreMe.Core.Domains.Common.Enums.Base;
using CoreMe.Core.Domains.Entities.Core;
using CoreMe.Core.Domains.Entities.User;
using CoreMe.Core.Exceptions;
using CoreMe.Core.Extensions;
using CoreMe.Core.Interface.IRepositories.Core;
using CoreMe.Service.Base;
using CoreMe.Service.Core.User;
using CoreMe.Service.Core.User.Input;
using CoreMe.Service.Core.User.Output;
using CoreMe.ToolKits.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreMe.Service.Core.User;

public class UserService : ApplicationSvc, IUserService
{
    private readonly IUserRepo _userRepo;
    private readonly IFileRepo _fileRepo;

    public UserService(IUserRepo userRepo, IFileRepo fileRepo)
    {
        _userRepo = userRepo;
        _fileRepo = fileRepo;
    }

    [Transactional]
    public async Task CreateAsync(UserEntity user, List<long> roleIds, string password)
    {
        if (!string.IsNullOrEmpty(user.Username))
        {
            bool isRepeatName = await _userRepo.Select.AnyAsync(r => r.Username == user.Username);
            if (isRepeatName)//用户名重复
            {
                throw new KnownException("用户名重复，请重新输入", ServiceResultCode.RepeatField);
            }
        }

        if (!string.IsNullOrEmpty(user.Email.Trim()))
        {
            var isRepeatEmail = await _userRepo.Select.AnyAsync(r => r.Email == user.Email.Trim());
            if (isRepeatEmail)//邮箱重复
            {
                throw new KnownException("注册邮箱重复，请重新输入", ServiceResultCode.RepeatField);
            }
        }

        user.UserRoles = new List<UserRoleEntity>();
        roleIds?.ForEach(roleId =>//遍历构建赋值角色
        {
            user.UserRoles.Add(new UserRoleEntity()
            {
                RoleId = roleId
            });
        });

        user.UserIdentitys = new List<UserIdentityEntity>()//构建赋值用户身份认证登录信息
        {
            new UserIdentityEntity(UserIdentityEntity.Password,user.Username,EncryptUtil.Encrypt(password),DateTime.Now)
        };
        await _userRepo.InsertAsync(user);
    }

    public async Task<UserDto> GetAsync(long? id)
    {
        var userId = id ?? CurrentUser.Id;
        var user = await _userRepo
            .Select
            .IncludeMany(u => u.Roles)
            .Where(r => r.Id == userId).FirstAsync();
        user.AvatarUrl = _fileRepo.GetFileUrl(user.AvatarUrl);
        return Mapper.Map<UserDto>(user);
    }

    public async Task<PagedDto<UserDto>> GetPagesAsync(UserPagingDto pageDto)
    {
        pageDto.Sort = pageDto.Sort.IsNullOrEmpty() ? "id ASC" : pageDto.Sort.Replace("-", " ");
        bool? isEnable = pageDto.IsEnable switch
        {
            1 => true,
            0 => false,
            _ => null
        };
        var users = await _userRepo
            .Select
            .IncludeMany(u => u.Roles)
            .WhereIf(!string.IsNullOrWhiteSpace(pageDto.Username), u => u.Username.Contains(pageDto.Username))
            .WhereIf(!string.IsNullOrWhiteSpace(pageDto.Nickname), u => u.Nickname.Contains(pageDto.Nickname))
            .WhereIf(isEnable != null, u => u.IsEnable == isEnable)
            .WhereIf(pageDto.CreateTime != null, u => u.CreateTime == pageDto.CreateTime)
            .WhereIf(pageDto.RoleId > 0, u => u.Roles.AsSelect().Any(r => r.Id == pageDto.RoleId))
            .OrderBy(pageDto.Sort)
            .ToPageListAsync(pageDto, out long totalCount);
        var userDtos = users.Select(u =>
         {
             var dto = Mapper.Map<UserDto>(u);
             dto.AvatarUrl = _fileRepo.GetFileUrl(dto.AvatarUrl);
             return dto;
         }).ToList();

        return new PagedDto<UserDto>(userDtos, totalCount);
    }

    public Task UpdateAsync(long id, UserEntity inputDto)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(long id)
    {
        throw new NotImplementedException();
    }
}
