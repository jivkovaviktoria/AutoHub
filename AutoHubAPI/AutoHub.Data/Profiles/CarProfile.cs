using AutoHub.Data.Models;
using AutoHub.Data.ViewModels;
using AutoMapper;

namespace AutoHub.Data.Profiles;

public class CarProfile : Profile
{
    public CarProfile()
    {
        CreateMap<Car, CarInfoViewModel>();
        CreateMap<CarInfoViewModel, Car>().ForMember(c => c.Images, opt => opt.Ignore());
    }
}