using LOGI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LOGI.Services
{
    public class SourceService
    {
        private ApplicationDbContext DbContext;

        public SourceService(ApplicationDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public List<Source> GetSources(out int total_count, int page = 0, int page_size = 10, string search_key = "", string order_by = "ID", string order_dir = "desc")
         {
             IQueryable<Source> result;
             if (order_by == "Name")
             {
                 if (order_dir == "desc")
                     result = DbContext.Sources.OrderByDescending(a => a.Name);
                 else
                     result = DbContext.Sources.OrderBy(a => a.Name);
             }
             else if (order_by == "NameAr")
             {
                 if (order_dir == "desc")
                     result = DbContext.Sources.OrderByDescending(a => a.NameAr);
                 else
                     result = DbContext.Sources.OrderBy(a => a.NameAr);
             }
             else
             {
                 if (order_dir == "desc")
                     result = DbContext.Sources.OrderByDescending(a => a.ID);
                 else
                     result = DbContext.Sources.OrderBy(a => a.ID);
             }

             if (search_key != "")
                 result = result.Where(a => a.Name.Contains(search_key) || a.NameAr.Contains(search_key));


             total_count = result.Count();
             result = result.Skip(page).Take(page_size);
             
            int img_width = LogiConfig.KeyIssuesImageSizes.Where(a=> a.Name == "SmallThumb").FirstOrDefault().Width;
            int img_height = LogiConfig.KeyIssuesImageSizes.Where(a=> a.Name == "SmallThumb").FirstOrDefault().Height;
            foreach ( Source s in result)
            {
                s.ImageURL = ImageService.GenerateImageFullPath(s.ImageURL, img_width.ToString(),img_height.ToString());
            }

             return result.ToList();
         }

        public int AddSource(Source source, IEnumerable<HttpPostedFileBase> Images = null, IEnumerable<HttpPostedFileBase> Logo = null)
        {
            DbContext.Sources.Add(source);
            try
            {
                DbContext.SaveChanges();

                if (source.ID > 0 && Images != null && Images.Count() > 0 && Images.Where(i => i != null).Count() > 0)
                {
                    AddSourceImages(source.ID, Images);
                }

                if (source.ID > 0 && Logo != null && Logo.Count() > 0 && Logo.Where(i => i != null).Count() > 0)
                {
                    AddSourceLogo(source.ID, Logo);
                }

                return source.ID;
            }
            catch(Exception ex)
            {
                return -1;
            }
        }

        public bool AddSourceImages(int ID, IEnumerable<HttpPostedFileBase> Images)
        {
            Source source = DbContext.Sources.Where(m => m.ID == ID).FirstOrDefault();

            if (source == null)
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

                            file_name = (ID + "-source-" + image.FileName).Replace(" ", "-");


                            System.Drawing.Image web_image = System.Drawing.Image.FromStream(image.InputStream);

                            //save Original Image
                            ImageService.SaveImage((System.Drawing.Image)web_image.Clone(), file_name);

                            //Save Thumnails
                            foreach (ImageSize IS in LogiConfig.KeyIssuesImageSizes)
                                ImageService.SaveImage((System.Drawing.Image)web_image.Clone(), file_name, IS.Width, IS.Height);

                            //Update the DB value
                            source.ImageURL = ImageService.GetImagesDirectory() + file_name;
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

        public bool AddSourceLogo(int ID, IEnumerable<HttpPostedFileBase> Logo)
        {
            Source source = DbContext.Sources.Where(m => m.ID == ID).FirstOrDefault();
          
            if (source == null)
                return false;

            if (Logo != null)
                foreach (HttpPostedFileBase image in Logo)
                {
                    if (image != null)
                    {
                        try
                        {
                            //Original
                            string file_name = "";

                            file_name = (ID + "-sourcelogo-" + image.FileName).Replace(" ", "-");


                            System.Drawing.Image web_image = System.Drawing.Image.FromStream(image.InputStream);

                            //save Original Image
                            ImageService.SaveImage((System.Drawing.Image)web_image.Clone(), file_name);

                            //Save Thumnails
                            ImageService.SaveImage((System.Drawing.Image)web_image.Clone(), file_name, 150, 80);

                            //Update the DB value
                            source.LogoURL = ImageService.GetImagesDirectory() + file_name;
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

        public Source GetSource(int id)
        {
            Source A = DbContext.Sources.Where(a => a.ID == id).FirstOrDefault();

            int img_width = LogiConfig.KeyIssuesImageSizes.Where(a => a.Name == "SmallThumb").FirstOrDefault().Width;
            int img_height = LogiConfig.KeyIssuesImageSizes.Where(a => a.Name == "SmallThumb").FirstOrDefault().Height;
            if (A.ImageURL != null && A.ImageURL != "")
                A.ImageURL = ImageService.GenerateImageFullPath(A.ImageURL, img_width.ToString(), img_height.ToString());

            return A;
        }

        public int UpdateSource(Source source, IEnumerable<HttpPostedFileBase> Images = null, IEnumerable<HttpPostedFileBase> Logo = null)
        {
            Source A = DbContext.Sources.Where(a => a.ID == source.ID).FirstOrDefault();

            if (A == null)
                return -1;

            A.Name = source.Name;
            A.NameAr = source.NameAr;

            try
            {
                DbContext.SaveChanges();

                if (A.ID > 0 && Images != null && Images.Where(i => i != null).Count() > 0)
                {
                    if (A.ImageURL != null && A.ImageURL != "")
                    {
                        ImageService.DeleteImage(HttpContext.Current.Server.MapPath("~" + A.ImageURL));
                        foreach (ImageSize IS in LogiConfig.KeyIssuesImageSizes)
                            ImageService.DeleteImage(HttpContext.Current.Server.MapPath("~" + A.ImageURL),IS.Width.ToString(),IS.Height.ToString());
                    }
                    AddSourceImages(A.ID, Images);
                }

                if (A.ID > 0 && Logo != null && Logo.Where(i => i != null).Count() > 0)
                {
                    if (A.LogoURL != null && A.LogoURL != "")
                    {
                        ImageService.DeleteImage(HttpContext.Current.Server.MapPath("~" + A.LogoURL));
                        ImageService.DeleteImage(HttpContext.Current.Server.MapPath("~" + A.LogoURL), 150.ToString(), 80.ToString());
                    }
                    AddSourceLogo(A.ID, Logo);
                }
                return A.ID;
            }
            catch(Exception ex)
            {
                return -1;
            }
           
        }

        public bool DeleteSource(int id)
        {
            Source a = DbContext.Sources.Where(aa => aa.ID == id).FirstOrDefault();

            if (a == null)
                return false;

            DbContext.Sources.Remove(a);

            try
            {
                DbContext.SaveChanges();
                return true;
            }catch(Exception ex)
            {
                return false;
            }
        }

        public SelectList GetSelectListSources(int? selected_value)
        {

            return new SelectList((from s in DbContext.Sources.OrderBy(s => s.Name).ToList()
                                   select new
                                   {
                                       ID_Dipendente = s.ID,
                                       SourceName = s.Name + " (" + s.NameAr + ")"
                                   }),
                                "ID_Dipendente",
                                "SourceName",
                                selected_value);
        }

        public SelectList GetSelectListFrontSources(int? selected_value,string language = "en")
        {
            if(language == "en")
                return new SelectList((from s in DbContext.Sources.ToList()
                                       select new
                                       {
                                           ID_Dipendente = s.ID,
                                           SourceName = s.Name
                                       }).OrderBy(s => s.SourceName),
                                    "ID_Dipendente",
                                    "SourceName",
                                    selected_value);
            else
                return new SelectList((from s in DbContext.Sources.ToList()
                                       select new
                                       {
                                           ID_Dipendente = s.ID,
                                           SourceName = s.NameAr
                                       }).OrderBy(s => s.SourceName),
                                "ID_Dipendente",
                                "SourceName",
                                selected_value);
        }

        public bool DeleteSourceImage(int id)
        {
            Source A = DbContext.Sources.Where(a => a.ID == id).FirstOrDefault();

            if (A == null)
                return false;

            try
            {
                if (A.ImageURL != null && A.ImageURL != "")
                {
                    ImageService.DeleteImage(HttpContext.Current.Server.MapPath("~" + A.ImageURL));
                    foreach (ImageSize IS in LogiConfig.KeyIssuesImageSizes)
                        ImageService.DeleteImage(HttpContext.Current.Server.MapPath("~" + A.ImageURL), IS.Width.ToString(), IS.Height.ToString());

                    A.ImageURL = null;
                }
                DbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool DeleteSourceLogo(int id)
        {
            Source A = DbContext.Sources.Where(a => a.ID == id).FirstOrDefault();

            if (A == null)
                return false;


            try
            {
               
               if (A.LogoURL != null && A.LogoURL != "")
                {
                    ImageService.DeleteImage(HttpContext.Current.Server.MapPath("~" + A.LogoURL));
                    ImageService.DeleteImage(HttpContext.Current.Server.MapPath("~" + A.LogoURL), 150.ToString(), 80.ToString());
                    A.LogoURL = null;
                }
               DbContext.SaveChanges();
                return true ;
            }
            catch (Exception ex)
            {
                return false ;
            }
        }
    }
}