using AutoMapper;

using Garage_2._0.Models;

namespace Garage_2._0.Automapper
{
    public class GarageMappings : Profile
    {
        public GarageMappings()
        {
            CreateMap<Member, MemberCreateViewModel>().ReverseMap();
            
            CreateMap<Vehicle, VehicleCreateViewModel>().ReverseMap();
        }
    }
}
