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

            CreateMap<ApplicationUser, ApplicationUserDTO>()
                .ForMember(x => x.EmailConfirmed, y => y.MapFrom((pr => pr.Logins.Count > 0 || pr.EmailConfirmed == true)))
                .ForMember(x => x.Logins, y => y.MapFrom(pr => pr.Logins.Select(d=> d.LoginProvider)));


            CreateMap<Quest, QuestFullDTO>()
                .ForMember(x => x.Owner, y => y.MapFrom(pr => pr.Owner.UserName));

            CreateMap<Quest, QuestDTO>()
                .ForMember(x => x.Owner, y => y.MapFrom(pr => pr.Owner.UserName))
                .ForMember(x => x.Cover, y => y.MapFrom(pr => pr.Cover.Name));

            CreateMap<Stage, StageDTO>()
                .ForMember(x => x.Cover, y => y.MapFrom(pr => pr.Cover.Name));

            CreateMap<Stage, StageFullDTO>();

            CreateMap<Motion, MotionDTO>();

            CreateMap<UserProfile, UserProfileDTO>()
                .ForMember(x => x.AvatarUrl, y => y.MapFrom(pr => pr.Avatar.Name));
        }

        public override string ProfileName => "EntityToDTOMappingProfile";
    }
}