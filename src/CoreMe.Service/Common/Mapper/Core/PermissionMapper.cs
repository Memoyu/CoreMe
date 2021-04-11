using AutoMapper;
using CoreMe.Core.Domains.Entities.Core;
using CoreMe.Service.Core.Permission.Input;

namespace CoreMe.Service.Common.Mapper.Core
{
    public class PermissionMapper : Profile
    {
        public PermissionMapper()
        {
            CreateMap<PermissionEntity, PermissionDto>();
            CreateMap<ModifyPermissionDto, PermissionEntity>();
        }
    }
}
