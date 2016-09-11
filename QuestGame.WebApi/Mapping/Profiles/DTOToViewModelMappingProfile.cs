using AutoMapper;
using QuestGame.Domain.DTO;
using QuestGame.WebApi.Areas.Design.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuestGame.WebApi.Mapping.Profiles
{
    public class DTOToViewModelMappingProfile : Profile
    {
        public DTOToViewModelMappingProfile()
        {
            CreateMap<QuestDTO, QuestViewModel>();
            CreateMap<IEnumerable<QuestDTO>, IEnumerable<QuestViewModel>>();
            CreateMap<StageDTO, StageViewModel>();
            CreateMap<MotionDTO, MotionViewModel>();
        }

        public override string ProfileName
        {
            get { return "DTOToViewModelMappingProfile"; }
        }
    }
}