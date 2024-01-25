using AutoMapper;
using ZATO.DTOs;
using ZATO.Models;

namespace ZATO.Configurations;

public class MapperConfig:Profile
{
    public MapperConfig()
    {
        CreateMap<User, UserDetailsDTO>().ReverseMap();
        CreateMap<Orders, OrderDTO>().ReverseMap();
        CreateMap<MenuItems, MenuItemsDTO>().ReverseMap();
        CreateMap<DeliveryPersonnel, DeliveryPersonnelDTO>().ReverseMap();
        CreateMap<DeliveryAssignments, DeliveryAssignmentDTO>().ReverseMap();
    }
}