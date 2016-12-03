using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuestGame.WebMVC.Helpers.SocialProviders
{
    public class GetProvider
    {
        public static SocialProvider CreateProvider(string nameProvider)
        {
            switch (nameProvider)
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