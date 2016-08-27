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
            CreateMap<Quest, QuestDTO>().ForMember(x => x.Owner, y => y.MapFrom(pr => pr.Owner.UserName));
            CreateMap<Stage, StageDTO>();
            CreateMap<Motion, MotionDTO>();//.ForMember(x => x.NextStage, y => y.MapFrom(pr => pr.NextStage.Tag));
        }
    }
}