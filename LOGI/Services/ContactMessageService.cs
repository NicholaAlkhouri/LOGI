using LOGI.Models;
using LOGI.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LOGI.Services
{
    public class ContactMessageService
    {
        private ApplicationDbContext DbContext;

        public ContactMessageService(ApplicationDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public int AddContactMessage(ContactMessage contactMessage)
        {
            DbContext.ContactMessages.Add(contactMessage);
            contactMessage.ContactDate = DateTime.Now;

            try
            {
                DbContext.SaveChanges();
                return contactMessage.ID;
            }
            catch(Exception ex)
            {
                return -1;
            }
        }

        public List<ContactMessageViewModel> GetMessages(out int total_count, int page = 0, int page_size = 10, string search_key = "", string order_by = "ID", string order_dir = "desc")
        {
            IQueryable<ContactMessage> result;
            if (order_by == "Email")
            {
                if (order_dir == "desc")
                    result = DbContext.ContactMessages.OrderByDescending(a => a.Email);
                else
                    result = DbContext.ContactMessages.OrderBy(a => a.Email);
            }
            else if (order_by == "FullName")
            {
                if (order_dir == "desc")
                    result = DbContext.ContactMessages.OrderByDescending(a => a.FullName);
                else
                    result = DbContext.ContactMessages.OrderBy(a => a.FullName);
            }
            else if (order_by == "Country")
            {
                if (order_dir == "desc")
                    result = DbContext.ContactMessages.OrderByDescending(a => a.Country);
                else
                    result = DbContext.ContactMessages.OrderBy(a => a.Country);
            }
            else
            {
                if (order_dir == "desc")
                    result = DbContext.ContactMessages.OrderByDescending(a => a.ContactDate);
                else
                    result = DbContext.ContactMessages.OrderBy(a => a.ContactDate);
            }

            total_count = result.Count();
            result = result.Skip(page).Take(page_size);

            List<ContactMessage> res = result.ToList();

            List<ContactMessageViewModel> final_res = new List<ContactMessageViewModel>();

            foreach(ContactMessage c in res)
            {
                final_res.Add(new ContactMessageViewModel()
                {
                    ContactDate = c.ContactDate.ToString("dd/MMMM/yyyy"),
                    Country = c.Country,
                    Email = c.Email,
                    FullName = c.FullName,
                    Message = c.Message,
                    Phone= c.Phone
                });
            }

            return final_res;
        }
    }
}