using System.Collections.Generic;

namespace QuestGame.WebMVC.Helpers.SocialProviders
{
    public interface IGetSocialUserInfo
    {
        Dictionary<string, string> GetSocialUserInfo();
    }
}