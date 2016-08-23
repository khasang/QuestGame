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
            CreateMap<Quest, QuestDTO>().ForMember("Owner", x => x.MapFrom(pr => pr.Owner.UserName));
            CreateMap<Stage, StageDTO>();
            CreateMap<Motion, MotionDTO>();
        }
    }
}