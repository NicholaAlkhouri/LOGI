using LOGI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LOGI.Services
{
    public class AuthorService
    {
         private ApplicationDbContext DbContext;

         public AuthorService(ApplicationDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public List<Author> GetAuthors(out int total_count, int page = 0, int page_size = 10, string search_key = "", string order_by = "ID", string order_dir = "desc")
         {
             IQueryable<Author> result;
             if (order_by == "FullName")
             {
                 if (order_dir == "desc")
                     result = DbContext.Authors.OrderByDescending(a => a.FullName);
                 else
                     result = DbContext.Authors.OrderBy(a => a.FullName);
             }
             else if (order_by == "FullNameAr")
             {
                 if (order_dir == "desc")
                     result = DbContext.Authors.OrderByDescending(a => a.FullNameAr);
                 else
                     result = DbContext.Authors.OrderBy(a => a.FullNameAr);
             }
             else
             {
                 if (order_dir == "desc")
                     result = DbContext.Authors.OrderByDescending(a => a.ID);
                 else
                     result = DbContext.Authors.OrderBy(a => a.ID);
             }

             if (search_key != "")
                 result = result.Where(a => a.FullName.Contains(search_key) || a.FullNameAr.Contains(search_key));


             total_count = result.Count();
             result = result.Skip(page).Take(page_size);

             return result.ToList();
         }

        public int AddAuthor ( Author author)
        {
            DbContext.Authors.Add(author);
            try
            {
                DbContext.SaveChanges();
                return author.ID;
            }
            catch(Exception ex)
            {
                return -1;
            }
        }

        public Author GetAuthor(int id)
        {
            Author A = DbContext.Authors.Where(a => a.ID == id).FirstOrDefault();

            return A;
        }

        public int UpdateAuthor ( Author author)
        {
            Author A = DbContext.Authors.Where(a => a.ID == author.ID).FirstOrDefault();

            if (A == null)
                return -1;

            A.FullName = author.FullName;
            A.FullNameAr = author.FullNameAr;

            try
            {
                DbContext.SaveChanges();
                return A.ID;
            }
            catch(Exception ex)
            {
                return -1;
            }
           
        }

        public bool DeleteAuthor(int id)
        {
            Author a = DbContext.Authors.Where(aa => aa.ID == id).FirstOrDefault();

            if (a == null)
                return false;

            DbContext.Authors.Remove(a);

            try
            {
                DbContext.SaveChanges();
                return true;
            }catch(Exception ex)
            {
                return false;
            }
        }

        public SelectList GetSelectListAuthors(int? selected_value)
        {

            return new SelectList((from s in DbContext.Authors.OrderBy(a => a.FullName).ToList()
                                   select new
                                   {
                                       ID_Dipendente = s.ID,
                                       AuthorName = s.FullName + " (" + s.FullNameAr + ")"
                                   }),
                                "ID_Dipendente",
                                "AuthorName",
                                selected_value);
        }

        public SelectList GetSelectListFrontAuthors(int? selected_value, string language = "en")
        {
            if (language == "en")
                return new SelectList((from s in DbContext.Authors.ToList()
                                       select new
                                       {
                                           ID_Dipendente = s.ID,
                                           AuthorName = s.FullName
                                       }),
                                    "ID_Dipendente",
                                    "AuthorName",
                                    selected_value);
            else
                return new SelectList((from s in DbContext.Authors.ToList()
                                       select new
                                       {
                                           ID_Dipendente = s.ID,
                                           AuthorName = s.FullNameAr
                                       }),
                                    "ID_Dipendente",
                                    "AuthorName",
                                    selected_value);
        }
    }
}