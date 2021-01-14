﻿/**************************************************************************  
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：Memoyu.Core.Domain.Shared.Security
*   文件名称 ：ICurrentUser.cs
*   =================================
*   创 建 者 ：Memoyu
*   创建日期 ：2021-01-03 16:05:44
*   邮箱     ：mmy6076@outlook.com
*   功能描述 ：
***************************************************************************/

using System.Security.Claims;

namespace Memoyu.Core.Domain.Shared.Security
{
    public interface ICurrentUser
    {
        long? Id { get; }

        string UserName { get; }
        long[] Roles { get; }


        Claim FindClaim(string claimType);

        Claim[] FindClaims(string claimType);

        Claim[] GetAllClaims();


        bool IsInGroup(long groupId);
    }
}
