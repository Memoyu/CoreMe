/**************************************************************************  
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：Memoyu.Core.Application.Contracts.Mapper.User
*   文件名称 ：TestMapper.cs
*   =================================
*   创 建 者 ：Memoyu
*   创建日期 ：2021-01-03 22:22:33
*   邮箱     ：mmy6076@outlook.com
*   功能描述 ：
***************************************************************************/
using AutoMapper;
using Memoyu.Core.Application.Contracts.Dtos.User;
using Memoyu.Core.Application.Contracts.Mapper.Converter;
using Memoyu.Core.Domain.Entities.User;

namespace Memoyu.Mbill.Application.Contracts.Mapper.User
{
    public class UserMapper : Profile
    {
        public UserMapper()
        {
            CreateMap<ModifyUserDto, UserEntity>();
            CreateMap<UserEntity, UserDto>().ForMember(d => d.Gender, opt => opt.ConvertUsing<GenderFormatter, int>());
        }
    }
}
