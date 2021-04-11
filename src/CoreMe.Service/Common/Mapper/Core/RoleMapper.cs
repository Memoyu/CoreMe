using AutoMapper;
using CoreMe.Core.Domains.Entities.Core;
using CoreMe.Service.Core.Permission.Input;

namespace CoreMe.Service.Common.Mapper.Core
{
    public class RoleMapper : Profile
    {
        public RoleMapper()
        {
            CreateMap<RoleEntity, RoleDto>();
            CreateMap<ModifyRoleDto, RoleEntity>();
        }
    }

}
