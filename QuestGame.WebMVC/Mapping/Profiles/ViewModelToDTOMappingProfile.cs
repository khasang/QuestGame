using AutoMapper;
using QuestGame.Domain.DTO;
using QuestGame.Domain.Entities;
using QuestGame.WebMVC.Areas.Design.Models;
using QuestGame.WebMVC.Areas.Game.Models;
using QuestGame.WebMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuestGame.WebMVC.Mapping.Profiles
{
    public class ViewModelToDTOMappingProfile : Profile
    {
        public ViewModelToDTOMappingProfile()
        {
            CreateMap<QuestViewModel, QuestFullDTO>();
            CreateMap<QuestViewModel, QuestDTO>();
            CreateMap<NewItemViewModel, QuestFullDTO>();
            CreateMap<NewItemViewModel, QuestDTO>();
            CreateMap<NewItemViewModel, StageDTO>();
            CreateMap<NewItemViewModel, MotionDTO>().ForMember(x => x.Description, y => y.MapFrom(v => v.Title));
            CreateMap<StageViewModel, StageFullDTO>();
            CreateMap<StageViewModel, StageDTO>();
            CreateMap<MotionViewModel, MotionDTO>(); 
            CreateMap<SocialUserModel, SocialUserDTO>();

            CreateMap<RegisterViewModel, RegisterUserDTO>();

            CreateMap<ResetPasswordRequestModel, ResetPasswordDTO>();

            CreateMap<UserViewModel, ApplicationUserDTO>();
                //.ForMember(x => x.Id, y => y.Ignore())
                //.ForMember(x => x.UserName, y => y.Ignore())
                //.ForMember(x => x.Email, y => y.Ignore())
                //.ForMember(x => x.Token, y => y.Ignore());

            CreateMap<UserProfileViewModel, UserProfileDTO>();
            //.ForMember(x => x.UserId, y => y.Ignore());

            CreateMap<ChangePasswordViewModel, ChangePasswordDTO>();
        }

        public override string ProfileName
        {
            get { return "ViewModelToDTOMappingProfile"; }
        }
    }
}