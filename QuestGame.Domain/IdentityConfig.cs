using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using QuestGame.Domain.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace QuestGame.Domain
{
    public class EmailService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Подключите здесь службу электронной почты для отправки сообщения электронной почты.

            using (var client = new SmtpClient())
            {
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.Timeout = 5000;

                var mail = new MailMessage("kloder@yandex.ru", message.Destination);
                    mail.Subject = message.Subject;
                    mail.Body = message.Body;
                    mail.IsBodyHtml = true;

                try
                {
                    client.Send(mail);
                }
                 catch (Exception)
                {
                    throw;
                }
            }


            return Task.FromResult(0);
        }
    }

    public class EmailToFileService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            //using (FileStream fstream = new FileStream(@"e:\Temp\QGemails.html", FileMode.OpenOrCreate))
            //{
            //    byte[] array = System.Text.Encoding.Default.GetBytes(message.Body);
            //    fstream.Write(array, 0, array.Length);
            //}

            using (var client = new SmtpClient())
            {
                client.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
                client.PickupDirectoryLocation = @"d:\Temp\";
                client.EnableSsl = false;

                var mail = new MailMessage("robot@questgame.ru", message.Destination);
                mail.Subject = message.Subject;
                mail.Body = message.Body;
                mail.IsBodyHtml = true;

                try
                {
                    client.Send(mail);
                }
                catch (SmtpException)
                {
                    throw;
                }
            }

                return Task.FromResult(0);
        }
    }

    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser> store)
            : base(store)
        {
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
        {
            var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(context.Get<ApplicationDbContext>()));
            // Настройка логики проверки имен пользователей
            manager.UserValidator = new UserValidator<ApplicationUser>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };
            // Настройка логики проверки паролей
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = false,
                RequireDigit = false,
                RequireLowercase = false,
                RequireUppercase = false,
            };

            manager.RegisterTwoFactorProvider("Код из сообщения", new EmailTokenProvider<ApplicationUser>
            {
                Subject = "Код безопасности",
                BodyFormat = "Ваш код безопасности: {0}"
            });

            manager.EmailService = new EmailToFileService(); 
            //manager.EmailService = new EmailService();

             var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider = new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }
    }
}
