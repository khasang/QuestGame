
namespace QuestGame.WebMVC.Helpers.SocialProviderFactory
{
    public class FaceBookAuth : SocialProvider
    {
        public FaceBookAuth()
        {
            this.RedirectUri = "https://localhost:44366/ExternalLogin/FaceBookAuthCallback";
            this.ClientId = "1601644850130436";
            this.ClientSecret = "5ca60a2235c69ed57cb4aa43685e84cc";
            this.ApplicationAuthPath = "https://www.facebook.com/dialog/oauth";
            this.ApplicationAuthTokenPath = "https://graph.facebook.com/oauth/access_token";
            this.RequestUserInfoPath = "https://graph.facebook.com/me";
        }

        public override string ProviderName()
        {
            return "FaceBook";
        }
    }
}