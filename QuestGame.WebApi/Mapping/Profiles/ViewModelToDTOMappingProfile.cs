using AutoMapper;
using QuestGame.Domain.DTO;
using QuestGame.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using QuestGame.WebApi.Areas.Game.Models;

namespace QuestGame.WebApi.Mapping.Profiles
{
    public class ViewModelToDTOMappingProfile : Profile
    {
        public ViewModelToDTOMappingProfile()
        {
            CreateMap<QuestViewModel, QuestDTO>();
            CreateMap<NewQuestViewModel, QuestDTO>();
            CreateMap<StageViewModel, StageDTO>();
            CreateMap<MotionViewModel, MotionDTO>();
        }

        public override string ProfileName
        {
            get { return "ViewModelToDTOMappingProfile"; }
        }
    }
}