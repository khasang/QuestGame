using AutoMapper;
using QuestGame.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using QuestGame.WebApi.Areas.Game.Models;

namespace QuestGame.WebApi.Mapping.Profiles
{
    public class DTOToViewModelMappingProfile : Profile
    {
        public DTOToViewModelMappingProfile()
        {
            CreateMap<QuestDTO, QuestViewModel>();
            //CreateMap<StageDTO, StageViewModel>();
            //CreateMap<MotionDTO, MotionViewModel>();
        }

        public override string ProfileName
        {
            get { return "DTOToViewModelMappingProfile"; }
        }
    }
}