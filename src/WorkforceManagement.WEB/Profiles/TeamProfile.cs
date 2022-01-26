using AutoMapper;
using System.Diagnostics.CodeAnalysis;
using WorkforceManagement.DAL.Entities;
using WorkforceManagement.Models.Team;

namespace WorkforceManagement.WEB.Profiles
{
    [ExcludeFromCodeCoverage]
    public class TeamProfile : Profile
    {
        public TeamProfile()
        {
            CreateMap<Team, ViewTeamDetailModel>()
                .ForMember(t => t.Created, o => o.MapFrom(m => m.CreatedAt.ToString("dd/MM/yyyy")))
                .ForMember(t => t.Modified, o => o.MapFrom(m => m.LastChange.ToString("dd/MM/yyyy")))
                .ForMember(t => t.Members, o => o.MapFrom(m => m.Members))
                .ReverseMap();

            CreateMap<Team, ViewTeamModel>()
                .ReverseMap();

            CreateMap<Team, CreateTeamModel>()
                    .ReverseMap();

            CreateMap<Team, EditTeamModel>()
                    .ReverseMap();
        }
    }
}
