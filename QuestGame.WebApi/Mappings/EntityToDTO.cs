using AutoMapper;
using QuestGame.Domain.DTO;
using QuestGame.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuestGame.WebApi.Mappings
{
    public class EntityToDTO : Profile
    {
        public EntityToDTO()
        {
            CreateMap<Quest, QuestDTO>();
            CreateMap<Stage, StageDTO>();
            CreateMap<Operation, OperationDTO>().ForMember("StageTitle", x => x.MapFrom(title => title.Stage.Title)); ;
            CreateMap<ContentQuest, ContentDTO>();
            CreateMap<ContentStage, ContentDTO>();
            CreateMap<QuestRoute, QuestRouteDTO>().ForMember("User", x => x.MapFrom( owner => owner.User.Name + " " + owner.User.LastName));
        }
    }
}