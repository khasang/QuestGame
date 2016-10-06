using AutoMapper;
using QuestGame.WebMVC.Mapping.Profiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuestGame.WebMVC.Mapping
{
    public class AutoMapperConfiguration
    {
        public static IMapper CreateMappings()
        {
            return new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<DTOToViewModelMappingProfile>();
                cfg.AddProfile<ViewModelToDTOMappingProfile>();
            }).CreateMapper();
        }
    }
}