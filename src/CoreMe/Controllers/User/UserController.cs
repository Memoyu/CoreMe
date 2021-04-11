using AutoMapper;
using CoreMe.Core.AOP.Attributes;
using CoreMe.Core.Domains.Common;
using CoreMe.Core.Domains.Common.Consts;
using CoreMe.Core.Domains.Entities.Core;
using CoreMe.Core.Domains.Entities.User;
using CoreMe.Service.Core.User;
using CoreMe.Service.Core.User.Input;
using CoreMe.Service.Core.User.Output;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CoreMe.Controllers.Core
{
    /// <summary>
    /// 用户管理
    /// </summary>
    [Route("api/admin/user")]
    public class UserController : ApiControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public UserController(IMapper mapper, IUserService userService)
        {
            _mapper = mapper;
            _userService = userService;
        }

        /// <summary>
        /// 超级管理员新增用户
        /// </summary>
        /// <param name="userInput"></param>
        [Logger("超级管理员新建了一个用户")]
        [HttpPost("register")]
        [Authorize(Roles = RoleEntity.Administrator)]
        [ApiExplorerSettings(GroupName = SystemConst.Grouping.GroupName_v2)]
        public async Task<ServiceResult> CreateAsync([FromBody] ModifyUserDto userInput)
        {
            await _userService.CreateAsync(_mapper.Map<UserEntity>(userInput), userInput.RoleIds, userInput.Password);
            return ServiceResult.Successed("用户创建成功");
        }

        /// <summary>
        /// 获取用户信息（id为空则通过token获取）
        /// </summary>
        [HttpGet("get")]
        [Authorize]
        [ApiExplorerSettings(GroupName = SystemConst.Grouping.GroupName_v3)]
        public async Task<ServiceResult<UserDto>> GetByAsync([FromQuery] long? id)
        {
            return ServiceResult<UserDto>.Successed(await _userService.GetAsync(id));
        }
    }
}
