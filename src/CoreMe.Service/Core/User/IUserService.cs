﻿using CoreMe.Core.Domains.Common;
using CoreMe.Core.Domains.Entities.User;
using CoreMe.Service.Core.User.Input;
using CoreMe.Service.Core.User.Output;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreMe.Service.Core.User
{
    public interface IUserService
    {
        /// <summary>
        /// 注册-新增一个用户
        /// </summary>
        /// <param name="user">用户</param>
        /// <param name="roleIds">分组Id集合</param>
        /// <param name="password">密码</param>
        Task CreateAsync(UserEntity user, List<long> roleIds, string password);

        /// <summary>
        /// 获取用户信息，id为空时，通过Token获取
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<UserDto> GetAsync(long? id);

        /// <summary>
        /// 获取用户分页信息
        /// </summary>
        /// 
        /// <returns></returns>
        Task<PagedDto<UserDto>> GetPagesAsync(UserPagingDto pagingDto);

        /// <summary>
        /// 更新用户信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task UpdateAsync(long id, UserEntity entity);

        /// <summary>
        /// 删除用户（软删除）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteAsync(long id);
    }
}
