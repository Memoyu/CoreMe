/**************************************************************************  
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：Memoyu.Core.Application.User
*   文件名称 ：IUserService.cs
*   =================================
*   创 建 者 ：Memoyu
*   创建日期 ：2021-01-06 21:16:28
*   邮箱     ：mmy6076@outlook.com
*   功能描述 ：
***************************************************************************/
using Memoyu.Core.Application.Contracts.Dtos.User;
using Memoyu.Core.Domain.Entities.User;
using Memoyu.Core.ToolKits.Base.Page;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Memoyu.Core.Application.User
{
    public interface IUserService
    {
        Task<PagedDto<UserDto>> GetListAsync(PagingDto pageDto);

        /// <summary>
        /// 通过token获取用户信息
        /// </summary>
        /// <returns></returns>
        Task<UserDto> GetAsync();

        Task<UserDto> GetAsync(Guid id);

        /// <summary>
        /// 注册-新增一个用户
        /// </summary>
        /// <param name="user">用户</param>
        /// <param name="roleIds">分组Id集合</param>
        /// <param name="password">密码</param>
        Task CreateAsync(UserEntity user, List<long> roleIds, string password);

        Task UpdateAsync(Guid id, ModifyUserDto inputDto);

        Task DeleteAsync(Guid id);
    }
}
