using System.Collections.Generic;
using Mvc.Mailer;
using LOGI.Models;
using System.Net.Mail;
using System.Configuration;

namespace LOGI.Mailers
{ 
    public class UserMailer : MailerBase, IUserMailer 	
	{
		public UserMailer()
		{
			MasterName="_EmailLayout";
		}
		
		public virtual MvcMailMessage ContactUs(string to_email, ContactMessage contactMessage)
		{
            MasterName = "_EmailLayout";
            ViewBag.Data = contactMessage;

            string from_email = ConfigurationManager.AppSettings["FromEmail"];
             
			return Populate(x =>
			{
                x.Subject = contactMessage.FullName + " Wants to contact with you.";
				x.ViewName = "ContactUs";
                x.ReplyToList.Add(contactMessage.Email);
                x.To.Add(to_email);
                x.From = new MailAddress(from_email);

			});
		}
 
		public virtual MvcMailMessage JoinLogi(string to_email, Vacancy vacancy, VacancyApplication application)
		{
            MasterName = "_EmailLayout";
            ViewBag.vacancy = vacancy;
            ViewBag.vacancyApplication = application;

            string from_email = ConfigurationManager.AppSettings["FromEmail"];

			return Populate(x =>
			{
                x.Subject = application.FullName + " Wants to join LOGI team.";
                x.ViewName = "JoinLogi";
                x.ReplyToList.Add(application.Email);
                x.To.Add(to_email);
                x.From = new MailAddress(from_email);
                
			});
		}

        public virtual MvcMailMessage JoinLogiAsExpert(string to_email, Expert expert)
        {
            MasterName = "_EmailLayout";
            ViewBag.expert = expert;

            string from_email = ConfigurationManager.AppSettings["FromEmail"];

            return Populate(x =>
            {
                x.Subject = expert.FullName + " Wants to join LOGI as an expert.";
                x.ViewName = "JoinLogiAsExpert";
                x.ReplyToList.Add(expert.Email);
                x.To.Add(to_email);
                x.From = new MailAddress(from_email);

            });
        }

       
 	}
}