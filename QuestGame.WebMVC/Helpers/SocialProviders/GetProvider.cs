using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuestGame.WebMVC.Helpers.SocialProviders
{
    public static class GetProvider
    {
        public static SocialProvider Provider(string name)
        {
            switch (name)
            {
                case "Google":
                    return new GoogleAuth();
                    break;
                case "FaceBook":
                    return new FacebookAuth();
                    break;
                case "VK":
                    return new VKontakteAuth();
                    break;
                case "Yandex":
                    return new YandexAuth();
                    break;
            }
            return null;
        }
    }
}