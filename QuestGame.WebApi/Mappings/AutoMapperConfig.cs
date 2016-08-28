using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuestGame.WebApi.Mappings
{
    public class AutoMapperConfig
    {
        public static IMapper CreateMappings()
        {
            return new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<EntityToDTO>();
            }).CreateMapper();
        }
    }
}