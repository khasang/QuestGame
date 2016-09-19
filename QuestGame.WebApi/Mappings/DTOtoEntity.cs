using AutoMapper;
using QuestGame.Domain.DTO;
using QuestGame.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuestGame.WebApi.Mappings
{
    public class DTOtoEntity : Profile
    {
        public DTOtoEntity()
        {
            CreateMap<StageDTO, Stage>();
        }
    }
}