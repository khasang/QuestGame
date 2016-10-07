using AutoMapper;
using QuestGame.WebMVC.Mapping.Profiles;

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