using AutoMapper;
using LoLTournaments.Application.Runtime;
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
            CreateMap<UserEntity, Account>().ReverseMap();
            CreateMap<RuntimeRoom, Room>().ReverseMap();
            CreateMap<RuntimeSession, Session>().ReverseMap();
            CreateMap<RuntimeStage, Stage>().ReverseMap();
            CreateMap<RuntimeGroup, Group>().ReverseMap();
            CreateMap<RuntimeGame, Game>().ReverseMap();
            CreateMap<RuntimeMember, Member>().ReverseMap();
            CreateMap<RuntimeWinner, Winner>().ReverseMap();
            CreateMap<LeagueOfLegendsPlayerInfo, AccountInfo>().ReverseMap();
            CreateMap<Member, Winner>().ReverseMap();
        }
    }

}