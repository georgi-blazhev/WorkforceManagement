using AutoMapper;
using System.Diagnostics.CodeAnalysis;
using WorkforceManagement.DAL.Entities;
using WorkforceManagement.Models.TimeOff;

namespace WorkforceManagement.WEB.Profiles
{
    [ExcludeFromCodeCoverage]
    public class TimeOffProfile : Profile
    {
        public TimeOffProfile()
        {
            CreateMap<TimeOffRequest, ViewTimeOffModel>()
                .ForMember(t => t.StartDate, o => o.MapFrom(m => m.StartDate.ToString("dd/MM/yyyy")))
                .ForMember(t => t.EndDate, o => o.MapFrom(m => m.EndDate.ToString("dd/MM/yyyy")))
                .ForMember(t => t.Created, o => o.MapFrom(m => m.CreatedAt.ToString("dd/MM/yyyy")))
                .ForMember(t => t.Modified, o => o.MapFrom(m => m.LastChange.ToString("dd/MM/yyyy")))
                .ReverseMap();

            CreateMap<DayOff, ViewDayOffModel>()
                .ForMember(t => t.DateOfDayOff, o => o.MapFrom(m => m.DateOfDayOff.ToString("dd/MM/yyyy")))
                .ReverseMap();

            CreateMap<TimeOffRequest, CreateTimeOffModel>()
                .ReverseMap();

            CreateMap<TimeOffRequest, EditTimeOffModel>()
                    .ReverseMap();
        }
    }
}
