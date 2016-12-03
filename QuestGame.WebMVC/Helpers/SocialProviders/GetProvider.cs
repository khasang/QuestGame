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
                case "FaceBook":
                    return new FacebookAuth();
                case "VK":
                    return new VKontakteAuth();
                case "Yandex":
                    return new YandexAuth();
            }
            return null;
        }
    }
}