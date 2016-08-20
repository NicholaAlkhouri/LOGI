using LOGI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LOGI.Services
{
    public class TeamService
    {
        private ApplicationDbContext DbContext;

        public TeamService(ApplicationDbContext dbContext)
        {
            DbContext = dbContext;
        }

       public List<TeamMember> GetTeamMembers()
       {
           return DbContext.TeamMembers.ToList();
       }
        
        public TeamMember GetTeamMember(int member_id)
       {
           return DbContext.TeamMembers.Where(t => t.ID == member_id).FirstOrDefault();
       }

       public List<TeamMember> GetTeamMembers(out int total_count, int page = 0, int page_size = 10, string search_key = "", string order_by = "ID", string order_dir = "desc")
       {
           IQueryable<TeamMember> result;
           if(order_by == "Type")
           { 
               if (order_dir == "desc")
                   result = DbContext.TeamMembers.OrderByDescending(a=> a.Type);
               else
                   result = DbContext.TeamMembers.OrderBy(a => a.Type);
           }
           else if (order_by == "FullName")
           {
               if (order_dir == "desc")
                   result = DbContext.TeamMembers.OrderByDescending(a => a.FullName);
               else
                   result = DbContext.TeamMembers.OrderBy(a => a.FullName);
           }
           else if (order_by == "FullNameAr")
           {
               if (order_dir == "desc")
                   result = DbContext.TeamMembers.OrderByDescending(a => a.FullNameAr);
               else
                   result = DbContext.TeamMembers.OrderBy(a => a.FullNameAr);
           }
           else 
           {
               if (order_dir == "desc")
                   result = DbContext.TeamMembers.OrderByDescending(a => a.ID);
               else
                   result = DbContext.TeamMembers.OrderBy(a => a.ID);
           }
           
           if (search_key != "")
               result = result.Where(cat => cat.FullName.Contains(search_key) || cat.FullNameAr.Contains(search_key));


           total_count = result.Count();
           result = result.Skip(page).Take(page_size);

           return result.ToList();
       }

       public int AddNewMember(TeamMember member, IEnumerable<HttpPostedFileBase> Images = null)
       {

           DbContext.TeamMembers.Add(member);

           try
           {
               DbContext.SaveChanges();

           }
           catch (Exception ex)
           {
              // logService.WriteError(ex.Message, ex.Message, ex.StackTrace, ex.Source);
               return -1;
           }

           if (member.ID > 0 && Images != null && Images.Count() > 0 && Images.Where(i => i != null).Count() > 0)
           {
               AddMemberImages(member.ID, Images);
           }

           return member.ID;
       }
       
        public int UpdateMember(TeamMember member, IEnumerable<HttpPostedFileBase> Images = null)
        {
            TeamMember old_member = DbContext.TeamMembers.Where(m => m.ID == member.ID).FirstOrDefault();

            if (old_member != null)
            {
                old_member.Career = member.Career;
                old_member.CareerAr = member.CareerAr;
                old_member.CurrentRole = member.CurrentRole;
                old_member.CurrentRoleAr = member.CurrentRoleAr;
                old_member.Education = member.Education;
                old_member.EducationAr = member.EducationAr;
                old_member.Email = member.Email;
                old_member.Expertise = member.Expertise;
                old_member.ExpertiseAr = member.ExpertiseAr;
                old_member.FullName = member.FullName;
                old_member.FullNameAr = member.FullNameAr;
                old_member.IsOnline = member.IsOnline;
                old_member.Linkedin = member.Linkedin;
                old_member.Twitter = member.Twitter;
                old_member.Type = member.Type;


                DbContext.SaveChanges();
            }
            else
                return -1;

            if (member.ID > 0 && Images != null && Images.Where(i=> i != null).Count() > 0)
            {
                if(old_member.ImageURL != null && old_member.ImageURL != "")
                    ImageService.DeleteImage(HttpContext.Current.Server.MapPath("~" + old_member.ImageURL));
                AddMemberImages(member.ID, Images);
            }

            try
            {
                return old_member.ID;
            }
            catch (Exception ex)
            {
                // logService.WriteError(ex.Message, ex.Message, ex.StackTrace, ex.Source);
                return -1;
            }
        }

        public bool DeleteMember(int member_id)
        {
            TeamMember member = DbContext.TeamMembers.Where(m => m.ID == member_id).FirstOrDefault();

            if (member == null)
                return false;

            try
            {
                if (member.ImageURL != null && member.ImageURL != "")
                    ImageService.DeleteImage(HttpContext.Current.Server.MapPath("~" + member.ImageURL));
                DbContext.TeamMembers.Remove(member);
                DbContext.SaveChanges();
                return true;
            }catch(Exception ex)
            {
                return false;
            }
        }

       public bool AddMemberImages(int member_id, IEnumerable<HttpPostedFileBase> Images, string article_title = "")
       {
           TeamMember member = DbContext.TeamMembers.Where(m => m.ID == member_id).FirstOrDefault();

           if (member == null)
               return false;

           if (Images != null)
               foreach (HttpPostedFileBase image in Images)
               {
                   if (image != null)
                   {
                       try
                       {
                           //Original
                           string file_name = "";
                           
                          file_name = (member_id + "-team-" + image.FileName).Replace(" ", "-");
                          

                           System.Drawing.Image web_image = System.Drawing.Image.FromStream(image.InputStream);

                           //save Original Image
                           ImageService.SaveImage((System.Drawing.Image)web_image.Clone(), file_name);

                           //Save Thumnails
                           //ImageService.SaveImage((System.Drawing.Image)web_image.Clone(), file_name, width, height);

                           //Update the DB value
                           member.ImageURL = ImageService.GetImagesDirectory() + file_name;
                           DbContext.SaveChanges();
                       }
                       catch (Exception ex)
                       {
                           return false;
                       }
                   }
               }
           return true;
       }
    }
}