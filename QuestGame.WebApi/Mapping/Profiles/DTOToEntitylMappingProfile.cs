using AutoMapper;
using QuestGame.Domain.DTO;
using QuestGame.Domain.Entities;
using QuestGame.Domain.Interfaces;
using QuestGame.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuestGame.WebApi.Mapping.Profiles
{
    public class DTOToEntitylMappingProfile : Profile
    {
        //IDataManager dataManager;

        //public DTOToEntitylMappingProfile(IDataManager dataManager)
        //{
        //    this.dataManager = dataManager;
        //}

        public DTOToEntitylMappingProfile()
        {
            CreateMap<QuestDTO, Quest>()
                .ForMember(x => x.Owner, y => y.Ignore());
            CreateMap<StageDTO, Stage>();
            CreateMap<MotionDTO, Motion>()
                .ForMember(x => x.OwnerStage, y => y.Ignore());

            //CreateMap<StageDTO, Stage>()
            //    .ConstructUsing((StageDTO detailDTO) =>
            //    {
            //        if (detailDTO.Id == 0)
            //        {
            //            var detail = new Stage();
            //            return detail;
            //        }

            //        return this.dataManager.Stages.GetById(detailDTO.Id);
            //    });
        }

        public override string ProfileName
        {
            get { return "DTOToEntitylMappingProfile"; }
        }
    }
}