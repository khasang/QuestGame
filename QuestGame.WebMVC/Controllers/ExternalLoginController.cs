using QuestGame.WebMVC.Helpers.SocialProviders;
using System.Web.Mvc;


namespace QuestGame.WebMVC.Controllers
{
    public class ExternalLoginController : Controller
    {
        // GET: ExternalLogin
        public ActionResult Index()
        {
            //SocialProvider provider = new GoogleAuth();
            SocialProvider provider = new FacebookAuth();


            return Redirect(provider.RequestCodeUrl);
        }

        [HttpGet]
        public ActionResult GoogleAuthCallback(string code)
        {
            SocialProvider gogole = new GoogleAuth();
            gogole.Code = code;

            var userInfo = gogole.GetUserInfo();

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult FaceBookAuthCallback(string code)
        {
            SocialProvider provider = new FacebookAuth();
            provider.Code = code;

            var userInfo = provider.GetUserInfo();

            return RedirectToAction("Index", "Home");
        }
    }
}