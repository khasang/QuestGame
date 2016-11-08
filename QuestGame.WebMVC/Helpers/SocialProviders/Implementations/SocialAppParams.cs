namespace QuestGame.WebMVC.Helpers.SocialProviders
{
    public class SocialAppParams
    {
        public string ClientId { get; set; }

        public string ClientSecret { get; set; }

        public string RedirectUri { get; set; }

        public string Code { get; set; }

        public string AccessToken { get; set; }

        public string Scope { get; set; }

        public string SocialID { get; set; }

        public string Provider { get; set; }

    }
}