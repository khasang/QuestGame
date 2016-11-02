
namespace QuestGame.WebMVC.Helpers.SocialProviderFactory
{
    public class FaceBookAuth : SocialProvider
    {
        public FaceBookAuth()
        {
            this.RedirectUri = "https://localhost:44366/ExternalLogin/GoogleAuthCallback";
            this.ClientId = "803183701728-q1ktbmuhces4vdj9udkmatn0gota8he8.apps.googleusercontent.com";
            this.ClientSecret = "yXNNeyD0OlL7pS-yfSzGL4bv";
            this.ApplicationAuthPath = "https://accounts.google.com/o/oauth2/auth";
            this.ApplicationAuthTokenPath = "https://accounts.google.com/o/oauth2/token";
            this.RequestUserInfoPath = "https://www.googleapis.com/oauth2/v1/userinfo";
        }

        public override string ProviderName()
        {
            return "Google";
        }
    }
}