﻿using FreeSql;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System;
using CoreMe.Core.Interface.IRepositories.Core;
using CoreMe.Core.Domains.Entities.User;
using CoreMe.Infrastructure.Repository.Base;
using CoreMe.Core.Security;

namespace CoreMe.Infrastructure.Repository.Core
{
    public class UserRepo : AuditBaseRepo<UserEntity>, IUserRepo
    {
        private readonly ICurrentUser _currentUser;
        public UserRepo(UnitOfWorkManager unitOfWorkManager, ICurrentUser currentUser) : base(unitOfWorkManager, currentUser)
        {
            _currentUser = currentUser;
        }

        /// <summary>
        /// 根据条件得到用户信息
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public Task<UserEntity> GetUserAsync(Expression<Func<UserEntity, bool>> expression)
        {
            return Select.Where(expression).IncludeMany(r => r.Roles).ToOneAsync();
        }

        /// <summary>
        /// 根据用户Id更新用户的最后登录时间
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Task UpdateLastLoginTimeAsync(long userId)
        {
            return UpdateDiy.Set(r => new UserEntity()
            {
                LastLoginTime = DateTime.Now
            }).Where(r => r.Id == userId).ExecuteAffrowsAsync();
        }
    }
}
