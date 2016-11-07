using AutoMapper;
using QuestGame.Domain.DTO;
using QuestGame.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuestGame.WebApi.Mapping.Profiles
{
    public class EntityToDTOMappingProfile : Profile
    {
        public EntityToDTOMappingProfile()
        {
            CreateMap<ApplicationUser, UserDTO>();
            CreateMap<ApplicationUser, ApplicationUserDTO>();

            CreateMap<Quest, QuestFullDTO>().ForMember(x => x.Owner, y => y.MapFrom(pr => pr.Owner.UserName));
            CreateMap<Quest, QuestDTO>().ForMember(x => x.Owner, y => y.MapFrom(pr => pr.Owner.UserName))
                                        .ForMember(x => x.Cover, y => y.MapFrom(pr => pr.Cover.GetPath()));
            CreateMap<Stage, StageDTO>().ForMember(x => x.Cover, y => y.MapFrom(pr => pr.Cover.GetPath()));
            CreateMap<Stage, StageFullDTO>();
            CreateMap<Motion, MotionDTO>();
            CreateMap<UserProfile, UserProfileDTO>();
        }

        public override string ProfileName
        {
            get { return "EntityToDTOMappingProfile"; }
        }
    }
}