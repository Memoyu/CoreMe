using AutoMapper;
using CoreMe.Core.Domains.Entities.User;
using CoreMe.Service.Common.Common.Converter;
using CoreMe.Service.Core.User.Input;
using CoreMe.Service.Core.User.Output;

namespace CoreMe.Service.Common.Mapper.Core
{
    public class UserMapper : Profile
    {
        public UserMapper()
        {
            CreateMap<ModifyUserDto, UserEntity>();
            CreateMap<UserEntity, UserDto>()
                .ForMember(d => d.Address, opt => opt.MapFrom(s => $"{s.Province}{s.City}{s.District}{s.Street}"))
                .ForMember(d => d.Gender, opt => opt.ConvertUsing<GenderFormatter, int>());
        }
    }
}
