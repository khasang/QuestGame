using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using QuestGame.WebApi.Models;
using QuestGame.WebApi.Providers;
using QuestGame.WebApi.Results;
using QuestGame.Domain;
using QuestGame.Domain.Entities;
using System.Net;
using System.Web.Configuration;
using System.Net.Http.Headers;
using QuestGame.Common.Interfaces;
using QuestGame.Common;
using System.Diagnostics;
using QuestGame.Common.Helpers;
using AutoMapper;
using QuestGame.Domain.DTO;
using QuestGame.Domain.Interfaces;
using System.Net.Mail;
using System.Threading;
using QuestGame.WebApi.Constants;

namespace QuestGame.WebApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/Account")]
    public class AccountController : BaseController
    {
        private const string LocalLoginProvider = "Local";
        private ApplicationUserManager _userManager;
        private ILoggerService logger;
        private IMapper mapper;

        public AccountController(IMapper mapper, ILoggerService logger)
        {
            this.mapper = mapper;
            this.logger = logger;
        }

        public AccountController(ApplicationUserManager userManager,
            ISecureDataFormat<AuthenticationTicket> accessTokenFormat)
        {
            UserManager = userManager;
            AccessTokenFormat = accessTokenFormat;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public ISecureDataFormat<AuthenticationTicket> AccessTokenFormat { get; private set; }


        [Route("GetUserById")]
        [AllowAnonymous]
        [HttpGet]
        public async Task<IHttpActionResult> GetUserById(string id)
        {
            var user = await UserManager.FindByIdAsync(id);

            if (user == null) { return NotFound(); }

            var result = mapper.Map<ApplicationUser, ApplicationUserDTO>(user);

            return Ok(result);
        }

        [Route("GetUserByEmail")]
        [AllowAnonymous]
        [HttpGet]
        public async Task<IHttpActionResult> GetUserByEmail(string email)
        {
            var user = await UserManager.FindByEmailAsync(email);
            if (user == null)
                return BadRequest();

            var result = mapper.Map<ApplicationUser, ApplicationUserDTO>(user);
            return Ok(result);
        }


        [Route("EditUser")]
        [HttpPost]
        public async Task<IHttpActionResult> EditUser(ApplicationUserDTO model)
        {
            var user = await UserManager.FindByIdAsync(model.Id);
            var userResult = mapper.Map<ApplicationUserDTO, ApplicationUser>(model, user);

            try
            {
                var result = await UserManager.UpdateAsync(userResult);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                logger.Error("Account | EditUser | ", ex.ToString());
                return InternalServerError();
            }

            return Ok();
        }


        // GET api/Account/UserInfo
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("UserInfo")]
        public UserInfoViewModel GetUserInfo()
        {
            var externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);

            return new UserInfoViewModel
            {
                Email = User.Identity.GetUserName(),
                HasRegistered = externalLogin == null,
                LoginProvider = externalLogin != null ? externalLogin.LoginProvider : null
            };
        }

        // POST api/Account/Logout
        [Route("Logout")]
        public IHttpActionResult Logout()
        {
            Authentication.SignOut(CookieAuthenticationDefaults.AuthenticationType);
            return Ok();
        }

        // GET api/Account/ManageInfo?returnUrl=%2F&generateState=true
        [Route("ManageInfo")]
        public async Task<ManageInfoViewModel> GetManageInfo(string returnUrl, bool generateState = false)
        {
            IdentityUser user = await UserManager.FindByIdAsync(User.Identity.GetUserId());

            if (user == null)
            {
                return null;
            }

            List<UserLoginInfoViewModel> logins = new List<UserLoginInfoViewModel>();

            foreach (IdentityUserLogin linkedAccount in user.Logins)
            {
                logins.Add(new UserLoginInfoViewModel
                {
                    LoginProvider = linkedAccount.LoginProvider,
                    ProviderKey = linkedAccount.ProviderKey
                });
            }

            if (user.PasswordHash != null)
            {
                logins.Add(new UserLoginInfoViewModel
                {
                    LoginProvider = LocalLoginProvider,
                    ProviderKey = user.UserName,
                });
            }

            return new ManageInfoViewModel
            {
                LocalLoginProvider = LocalLoginProvider,
                Email = user.UserName,
                Logins = logins,
                ExternalLoginProviders = GetExternalLogins(returnUrl, generateState)
            };
        }

        // POST api/Account/ChangePassword
        [HttpPost]
        [Route("ChangePassword")]
        public async Task<IHttpActionResult> ChangePassword(ChangePasswordDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var principal = Thread.CurrentPrincipal;
            var identity = principal.Identity;
            var userId = identity.GetUserId();

            var result = await UserManager.ChangePasswordAsync(userId, model.OldPassword, model.NewPassword);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }



        // POST api/Account/SetPassword
        [Route("SetPassword")]
        public async Task<IHttpActionResult> SetPassword(SetPasswordBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }


        // POST api/Account/ResetPassword
        [AllowAnonymous]
        [Route("ResetPassword")]
        public async Task<IHttpActionResult> ResetPassword(ResetPasswordDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await UserManager.ResetPasswordAsync(model.Id, model.ResetToken, model.NewPassword);

            if (!result.Succeeded)
            {
                return BadRequest();
            }

            return Ok();
        }

        // POST api/Account/AddExternalLogin
        [Route("AddExternalLogin")]
        public async Task<IHttpActionResult> AddExternalLogin(AddExternalLoginBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);

            AuthenticationTicket ticket = AccessTokenFormat.Unprotect(model.ExternalAccessToken);

            if (ticket == null || ticket.Identity == null || (ticket.Properties != null
                && ticket.Properties.ExpiresUtc.HasValue
                && ticket.Properties.ExpiresUtc.Value < DateTimeOffset.UtcNow))
            {
                return BadRequest("Сбой внешнего входа.");
            }

            ExternalLoginData externalData = ExternalLoginData.FromIdentity(ticket.Identity);

            if (externalData == null)
            {
                return BadRequest("Внешнее имя входа уже связано с учетной записью.");
            }

            IdentityResult result = await UserManager.AddLoginAsync(User.Identity.GetUserId(),
                new UserLoginInfo(externalData.LoginProvider, externalData.ProviderKey));

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        // POST api/Account/RemoveLogin
        [Route("RemoveLogin")]
        public async Task<IHttpActionResult> RemoveLogin(RemoveLoginBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result;

            if (model.LoginProvider == LocalLoginProvider)
            {
                result = await UserManager.RemovePasswordAsync(User.Identity.GetUserId());
            }
            else
            {
                result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(),
                    new UserLoginInfo(model.LoginProvider, model.ProviderKey));
            }

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        // GET api/Account/ExternalLogin
        [OverrideAuthentication]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalCookie)]
        [AllowAnonymous]
        [Route("ExternalLogin", Name = "ExternalLogin")]
        public async Task<IHttpActionResult> GetExternalLogin(string provider, string error = null)
        {
            if (error != null)
            {
                return Redirect(Url.Content("~/") + "#error=" + Uri.EscapeDataString(error));
            }

            if (!User.Identity.IsAuthenticated)
            {
                return new ChallengeResult(provider, this);
            }

            ExternalLoginData externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);

            if (externalLogin == null)
            {
                return InternalServerError();
            }

            if (externalLogin.LoginProvider != provider)
            {
                Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);
                return new ChallengeResult(provider, this);
            }

            ApplicationUser user = await UserManager.FindAsync(new UserLoginInfo(externalLogin.LoginProvider,
                externalLogin.ProviderKey));

            var hasRegistered = user != null;

            if (hasRegistered)
            {
                Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);

                ClaimsIdentity oAuthIdentity = await user.GenerateUserIdentityAsync(UserManager,
                   OAuthDefaults.AuthenticationType);
                ClaimsIdentity cookieIdentity = await user.GenerateUserIdentityAsync(UserManager,
                    CookieAuthenticationDefaults.AuthenticationType);

                AuthenticationProperties properties = ApplicationOAuthProvider.CreateProperties(user.UserName);
                Authentication.SignIn(properties, oAuthIdentity, cookieIdentity);
            }
            else
            {
                IEnumerable<Claim> claims = externalLogin.GetClaims();
                ClaimsIdentity identity = new ClaimsIdentity(claims, OAuthDefaults.AuthenticationType);
                Authentication.SignIn(identity);
            }

            return Ok();
        }

        // GET api/Account/ExternalLogins?returnUrl=%2F&generateState=true
        [AllowAnonymous]
        [Route("ExternalLogins")]
        public IEnumerable<ExternalLoginViewModel> GetExternalLogins(string returnUrl, bool generateState = false)
        {
            IEnumerable<AuthenticationDescription> descriptions = Authentication.GetExternalAuthenticationTypes();
            List<ExternalLoginViewModel> logins = new List<ExternalLoginViewModel>();

            string state;

            if (generateState)
            {
                const int strengthInBits = 256;
                state = RandomOAuthStateGenerator.Generate(strengthInBits);
            }
            else
            {
                state = null;
            }

            foreach (var description in descriptions)
            {
                var login = new ExternalLoginViewModel
                {
                    Name = description.Caption,
                    Url = Url.Route("ExternalLogin", new
                    {
                        provider = description.AuthenticationType,
                        response_type = "token",
                        client_id = Startup.PublicClientId,
                        redirect_uri = new Uri(Request.RequestUri, returnUrl).AbsoluteUri,
                        state = state
                    }),
                    State = state
                };
                logins.Add(login);
            }

            return logins;
        }

        // POST api/Account/Register
        [AllowAnonymous]
        [Route("Register")]
        public async Task<IHttpActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = mapper.Map<RegisterViewModel, ApplicationUser>(model);
            var profile = mapper.Map<RegisterViewModel, UserProfile>(model);
            user.UserProfile = profile;

            var result = await UserManager.CreateAsync(user, model.Password);
            var toRole = UserManager.AddToRole(user.Id, "user");

            var response = new RegisterResponse
            {
                Success = result.Succeeded,
                Status = result.Succeeded.ToString(),
                Body = user.Id,
                ErrorMessage = result.Errors.ToString()
            };

            logger.Information("| Registration | {@user}", model);
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("GetEmailToken")]
        public async Task<HttpResponseMessage> GetEmailToken(string id)
        {
            try
            {
                var emailToken = await UserManager.GenerateEmailConfirmationTokenAsync(id);
                return Request.CreateResponse<string>(HttpStatusCode.OK, emailToken, new MediaTypeHeaderValue("application/json"));
            }
            catch (Exception)
            {
                return new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.BadRequest,
                };
            }
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("GetResetToken")]
        public async Task<HttpResponseMessage> GetResetToken(string id)
        {
            try
            {
                var resetToken = await UserManager.GeneratePasswordResetTokenAsync(id);
                return Request.CreateResponse<string>(HttpStatusCode.OK, resetToken, new MediaTypeHeaderValue("application/json"));
            }
            catch (Exception)
            {
                return new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.BadRequest,
                };
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("SendEmailToken")]
        public IHttpActionResult SendEmailToken(Dictionary<string, string> parameters)
        {
            var userid = parameters["userId"];
            var subject = parameters["subject"];
            var body = parameters["body"];

            try
            {
                UserManager.SendEmail(userid, subject, body);
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("SendResetToken")]
        public IHttpActionResult SendResetToken(Dictionary<string, string> parameters)
        {
            var userid = parameters["userId"];
            var subject = parameters["subject"];
            var body = parameters["body"];

            try
            {
                UserManager.SendEmail(userid, subject, body);
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("ConfirmEmail")]
        public async Task<HttpResponseMessage> ConfirmEmail(string id, string code)
        {

            using (var client = RestHelper.Create())
            {
                var result = await UserManager.ConfirmEmailAsync(id, code);

                if (!result.Succeeded)
                {
                    return new HttpResponseMessage
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                    };
                }

                return new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("Email подтвержден")
                };
            }
        }

        [AllowAnonymous]
        [Route("LoginUser")]
        public async Task<HttpResponseMessage> LoginUserNew(LoginBindingModel model)
        {
            if (model == null)
            {
                return new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Content = new StringContent("Invalid user data")
                };
            }

            using (var client = RestHelper.Create())
            {
                var requestParams = new Dictionary<string, string>
                {
                    { "grant_type", "password" },
                    { "username", model.Email },
                    { "password", model.Password }
                };

                var content = new FormUrlEncodedContent(requestParams);
                var response = await client.PostAsync("Token", content);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return new HttpResponseMessage
                    {
                        StatusCode = HttpStatusCode.BadRequest
                    };
                }

                var responseData = await response.Content.ReadAsAsync<Dictionary<string, string>>();
                var authToken = responseData["access_token"];

                logger.Information("| Login | {@user}", model);

                var user = UserManager.FindByName(model.Email);
                var userResult = mapper.Map<ApplicationUser, ApplicationUserDTO>(user);
                userResult.Token = authToken;

                var responseResult = Request.CreateResponse<ApplicationUserDTO>(HttpStatusCode.OK, userResult);
                responseResult.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                return responseResult;
            }
        }

        // POST api/Account/RegisterExternal
        [OverrideAuthentication]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("RegisterExternal")]
        public async Task<IHttpActionResult> RegisterExternal(RegisterExternalBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var info = await Authentication.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return InternalServerError();
            }

            var user = new ApplicationUser() { UserName = model.Email, Email = model.Email };

            IdentityResult result = await UserManager.CreateAsync(user);

            var response = new RegisterResponse
            {
                Success = result.Succeeded,
                Status = result.Succeeded.ToString(),
                Body = string.Empty,
                ErrorMessage = result.Errors.ToString()
            };

            return Ok(response);
        }

        /// <summary>
        /// Загрузка аватара
        /// </summary>
        [HttpPost]
        [Route("UploadFile")]
        public async Task<string> UploadFile()
        {
            try
            {
                var result = await Upload(ConfigSettings.AvatarPrefixFile);
                return result;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                logger.Error("Account | UploadFile | ", ex.ToString());
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
            }

            base.Dispose(disposing);
        }

        #region Вспомогательные приложения

        private IAuthenticationManager Authentication => Request.GetOwinContext().Authentication;

        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (result.Succeeded)
                return null;

            if (result.Errors != null)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error);
                }
            }
            if (ModelState.IsValid)
            {
                // Ошибки ModelState для отправки отсутствуют, поэтому просто возвращается пустой BadRequest.
                return BadRequest();
            }

            return BadRequest(ModelState);
        }

        private class ExternalLoginData
        {
            public string LoginProvider { get; set; }
            public string ProviderKey { get; set; }
            public string UserName { get; set; }

            public IList<Claim> GetClaims()
            {
                IList<Claim> claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.NameIdentifier, ProviderKey, null, LoginProvider));

                if (UserName != null)
                {
                    claims.Add(new Claim(ClaimTypes.Name, UserName, null, LoginProvider));
                }

                return claims;
            }

            public static ExternalLoginData FromIdentity(ClaimsIdentity identity)
            {
                var providerKeyClaim = identity?.FindFirst(ClaimTypes.NameIdentifier);

                if (string.IsNullOrEmpty(providerKeyClaim?.Issuer) ||
                    string.IsNullOrEmpty(providerKeyClaim.Value) ||
                    providerKeyClaim.Issuer == ClaimsIdentity.DefaultIssuer)
                {
                    return null;
                }
                    
                return new ExternalLoginData
                {
                    LoginProvider = providerKeyClaim.Issuer,
                    ProviderKey = providerKeyClaim.Value,
                    UserName = identity.FindFirstValue(ClaimTypes.Name)
                };
            }
        }

        private static class RandomOAuthStateGenerator
        {
            private static RandomNumberGenerator _random = new RNGCryptoServiceProvider();

            public static string Generate(int strengthInBits)
            {
                const int bitsPerByte = 8;

                if (strengthInBits % bitsPerByte != 0)
                {
                    throw new ArgumentException("Значение strengthInBits должно нацело делиться на 8.", "strengthInBits");
                }

                int strengthInBytes = strengthInBits / bitsPerByte;

                byte[] data = new byte[strengthInBytes];
                _random.GetBytes(data);
                return HttpServerUtility.UrlTokenEncode(data);
            }
        }

        #endregion
    }
}
