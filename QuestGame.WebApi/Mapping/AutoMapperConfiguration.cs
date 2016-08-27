using AutoMapper;
using QuestGame.WebApi.Mapping.Profiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuestGame.WebApi.Mapping
{
    public class AutoMapperConfiguration
    {
        public static IMapper CreateMappings()
        {
            return new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<EntityToDTOMappingProfile>();
                cfg.AddProfile<DTOToEntitylMappingProfile>();
            }).CreateMapper();
        }
    }
}