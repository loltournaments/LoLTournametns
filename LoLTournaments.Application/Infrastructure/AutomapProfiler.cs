using AutoMapper;
using LoLTournaments.Application.Models;
using LoLTournaments.Application.Services;
using LoLTournaments.Domain.Entities;
using LoLTournaments.Shared.Models;

namespace LoLTournaments.Application.Infrastructure
{

    public class AppMappingProfile : Profile
    {
        public AppMappingProfile()
        {			
            CreateMap<AppSettings, SharedConfig>().ReverseMap();
            CreateMap<UserEntity, UserDto>().ReverseMap();
            CreateMap<RuntimeRoom, Room>().ReverseMap();
            CreateMap<RuntimeSession, Session>().ReverseMap();
            CreateMap<RuntimeStage, Stage>().ReverseMap();
            CreateMap<RuntimeGroup, Group>().ReverseMap();
            CreateMap<RuntimeGame, Game>().ReverseMap();
            CreateMap<RuntimeMember, Member>().ReverseMap();
            CreateMap<RuntimeWinner, Winner>().ReverseMap();
            CreateMap<Member, Winner>().ReverseMap();
        }
    }

}