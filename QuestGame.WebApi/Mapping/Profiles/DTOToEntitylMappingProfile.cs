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
            CreateMap<QuestDTO, Quest>()
                .ForMember(x => x.Owner, y => y.Ignore());
            CreateMap<StageDTO, Stage>();
            CreateMap<MotionDTO, Motion>()
                .ForMember(x => x.OwnerStage, y => y.Ignore());
        }

        public override string ProfileName
        {
            get { return "DTOToEntitylMappingProfile"; }
        }
    }
}