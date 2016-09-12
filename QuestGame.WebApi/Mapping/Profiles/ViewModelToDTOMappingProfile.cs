using AutoMapper;
using QuestGame.Domain.DTO;
using QuestGame.Domain.Entities;
using QuestGame.WebApi.Areas.Design.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuestGame.WebApi.Mapping.Profiles
{
    public class ViewModelToDTOMappingProfile : Profile
    {
        public ViewModelToDTOMappingProfile()
        {
            CreateMap<QuestViewModel, QuestDTO>();
            //CreateMap<NewQuestViewModel, QuestDTO>();
            CreateMap<StageViewModel, StageDTO>();
            //CreateMap<MotionViewModel, MotionDTO>();
        }

        public override string ProfileName
        {
            get { return "ViewModelToDTOMappingProfile"; }
        }
    }
}