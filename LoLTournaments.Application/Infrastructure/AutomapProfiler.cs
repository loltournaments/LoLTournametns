using AutoMapper;
using LoLTournaments.Application.Services;
using LoLTournaments.Shared.Models;

namespace LoLTournaments.Application.Infrastructure
{

    public class AppMappingProfile : Profile
    {
        public AppMappingProfile()
        {			
            CreateMap<AppSettings, SharedConfig>().ReverseMap();
        }
    }

}