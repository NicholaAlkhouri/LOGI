using System.Collections.Generic;
using Mvc.Mailer;
using LOGI.Models;

namespace LOGI.Mailers
{ 
    public interface IUserMailer
    {
			MvcMailMessage ContactUs(string to_email, ContactMessage contactMessage);
            MvcMailMessage JoinLogi(string to_email, Vacancy vacancy, VacancyApplication vacancyApplication);
            MvcMailMessage JoinLogiAsExpert(string to_email, Expert expert);
	}
}