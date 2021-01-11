﻿/**************************************************************************  
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：Memoyu.Core.WebApi.Controllers.Core
*   文件名称 ：AccountController.cs
*   =================================
*   创 建 者 ：Memoyu
*   创建日期 ：2021-01-10 11:57:14
*   邮箱     ：mmy6076@outlook.com
*   功能描述 ：
***************************************************************************/
using Autofac;
using Memoyu.Core.Application.Contracts.Dtos.Core;
using Memoyu.Core.Application.Contracts.Exceptions;
using Memoyu.Core.Application.Core.Account;
using Memoyu.Core.Application.Core.Account.Impl;
using Memoyu.Core.Domain.Shared.Configurations;
using Memoyu.Core.Domain.Shared.Const;
using Memoyu.Core.ToolKits.Base.Enum.Base;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Memoyu.Core.WebApi.Controllers.Core
{
    /// <summary>
    /// 账户相关
    /// </summary>
    [Route("api/account")]
    [ApiExplorerSettings(GroupName = SystemConst.Grouping.GroupName_v3)]
    public class AccountController : ApiControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly IAccountService _accountService;
        public AccountController(IComponentContext componentContext, IAccountService accountService)
        {
            bool isIdentityServer4 = AppSettings.IdentityServer4Enable;
            _tokenService = componentContext.ResolveNamed<ITokenService>(isIdentityServer4 ? typeof(IdentityServer4Service).Name : typeof(JwtTokenService).Name);
            _accountService = accountService;
        }

        /// <summary>
        /// 登录接口
        /// </summary>
        ///<example>
        /// 用户名：admin，密码：123456
        /// </example>
        [HttpPost("login")]
        public async Task<TokenDto> Login(LoginInputDto loginInputDto)
        {
            return await _tokenService.LoginAsync(loginInputDto);
        }

        /// <summary>
        /// 刷新用户的token
        /// </summary>
        /// <returns></returns>
        [HttpGet("refresh")]
        public async Task<TokenDto> GetRefreshToken()
        {
            string refreshToken;
            string authorization = Request.Headers["Authorization"];

            if (authorization != null && authorization.StartsWith(JwtBearerDefaults.AuthenticationScheme))//判断请求是否带Token
            {
                refreshToken = authorization.Substring(JwtBearerDefaults.AuthenticationScheme.Length + 1).Trim();//获取refreshToken
            }
            else
            {
                throw new KnownException(" 请先登录.", ServiceResultCode.RefreshTokenError);
            }
            return await _tokenService.GetTokenByRefreshAsync(refreshToken);
        }

    }
}
