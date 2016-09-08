using AutoMapper;
using QuestGame.Domain.DTO;
using QuestGame.Domain.Entities;
using QuestGame.WebApi.Models;
using QuestGame.WebApi.Models.UserViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuestGame.WebApi.Mappings
{ 
    public class ViewModelsToEntity : Profile
    {
        public ViewModelsToEntity()
        {
            CreateMap<QuestVM, Quest>();
            CreateMap<QuestVM, ContentQuest>();
            CreateMap<StageVM, Stage>();
            CreateMap<StageVM, ContentStage>();
            CreateMap<OperationsVM, Operation>();
            CreateMap<UserRegisterVM, ApplicationUser>();
            CreateMap<ApplicationUser, UserProfileVM>();
        }
    }
}