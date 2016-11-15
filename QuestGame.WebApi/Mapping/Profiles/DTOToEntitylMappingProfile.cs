using AutoMapper;
using QuestGame.Domain.DTO;
using QuestGame.Domain.Entities;
using QuestGame.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuestGame.WebApi.Mapping.Profiles
{
    public class DTOToEntitylMappingProfile : Profile
    {
        public DTOToEntitylMappingProfile()
        {
            CreateMap<ApplicationUserDTO, ApplicationUser>()
                .ForMember(x => x.Id, y => y.Ignore())
                .ForMember(x => x.Email, y => y.Ignore())
                .ForMember(x => x.EmailConfirmed, y => y.Ignore())
                .ForMember(x => x.UserName, y => y.Ignore())
                .ForMember(x => x.Quests, y => y.Ignore());

            CreateMap<UserDTO, ApplicationUser>()
                .ForMember(x => x.Id, y => y.Ignore())
                .ForMember(x => x.Email, y => y.Ignore())
                .ForMember(x => x.UserName, y => y.Ignore())
                .ForMember(x => x.Quests, y => y.Ignore());

            CreateMap<UserProfileDTO, UserProfile>()
                .ForMember(x => x.UserId, y => y.Ignore());

            CreateMap<QuestFullDTO, Quest>()
                .ForMember(x => x.Owner, y => y.Ignore())
                .ForMember(x => x.Cover, y => y.Ignore());

            CreateMap<QuestDTO, Quest>()
                .ForMember(x => x.Owner, y => y.Ignore())
                .ForMember(x => x.Cover, y => y.Ignore());

            CreateMap<StageFullDTO, Stage>()
                .ForMember(x => x.Cover, y => y.Ignore());

            CreateMap<StageDTO, Stage>()
                .ForMember(x => x.Cover, y => y.Ignore());

            CreateMap<MotionDTO, Motion>();
        }

        public override string ProfileName => "DTOToEntitylMappingProfile";
    }
}