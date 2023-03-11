using AutoHub.Data.Models;
using AutoHub.Data.ViewModels;
using AutoMapper;

namespace AutoHub.Data.Profiles;

public class CarProfile : Profile
{
    public CarProfile()
    {
        CreateMap<Car, CarInfoViewModel>();
    }
}