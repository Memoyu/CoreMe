using Autofac;
using CoreMe.Core.Common.Configs;
using CoreMe.Core.Domains.Common.Consts;
using CoreMe.Core.Domains.Common.Enums.Base;
using CoreMe.Core.Exceptions;
using CoreMe.Service.Core.Auth;
using CoreMe.Service.Core.Auth.Input;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CoreMe.Controllers.Core
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
            bool isIdentityServer4 = Appsettings.IdentityServer4Enable;
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
        public async Task<TokenDto> Login(LoginDto loginDto)
        {
            return await _tokenService.LoginAsync(loginDto);
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
