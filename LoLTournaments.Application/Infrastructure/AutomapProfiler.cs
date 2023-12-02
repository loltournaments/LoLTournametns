using AutoMapper;
using LoLTournaments.Application.Runtime;
using LoLTournaments.Application.Services;
using LoLTournaments.Domain.Entities;
using LoLTournaments.Shared.Models;
using Newtonsoft.Json;

namespace LoLTournaments.Application.Infrastructure
{

    public class AppMappingProfile : Profile
    {
        public AppMappingProfile()
        {
            CreateMap<AppSettings, SharedConfig>().ReverseMap();
            CreateMap<UserEntity, Account>().ReverseMap();
            
            CreateMap<RuntimeSession, SessionEntity>()
                    .ForMember(x => x.Data, o => o.MapFrom(x => JsonConvert.SerializeObject(x)))
                    .ForMember(x => x.Id, o => o.MapFrom(x => x.Id))
                    .ReverseMap()
                    .ConvertUsing((x, _) => string.IsNullOrEmpty(x.Data) ? new RuntimeSession() : JsonConvert.DeserializeObject<RuntimeSession>(x.Data));
            
            CreateMap<RuntimeRoom, RoomEntity>()
                    .ForMember(x => x.Data, o => o.MapFrom(x => JsonConvert.SerializeObject(x)))
                    .ForMember(x => x.Id, o => o.MapFrom(x => x.Id))
                    .ReverseMap()
                    .ConvertUsing((x, _) => string.IsNullOrEmpty(x.Data) ? new RuntimeRoom() : JsonConvert.DeserializeObject<RuntimeRoom>(x.Data));
            
            CreateMap<RuntimeRoom, Room>().ReverseMap();
            CreateMap<RuntimeSession, Session>().ReverseMap();
            CreateMap<RuntimeStage, Stage>().ReverseMap();
            CreateMap<RuntimeGroup, Group>().ReverseMap();
            CreateMap<RuntimeGame, Game>().ReverseMap();
            CreateMap<RuntimeMember, Member>().ReverseMap();
            CreateMap<RuntimeWinner, Winner>().ReverseMap();
            CreateMap<RuntimeMember, RuntimeWinner>().ReverseMap();
            CreateMap<LeagueOfLegendsPlayerInfo, AccountInfo>().ReverseMap();
            CreateMap<Member, Winner>().ReverseMap();
        }
    }

}