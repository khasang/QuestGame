using QuestGame.WebMVC.Models;
using System.Collections.Generic;

namespace QuestGame.WebMVC.Helpers.SocialProviders
{
    public interface IGetSocialUserInfo
    {
        SocialUserModel GetSocialUserInfo();
    }
}