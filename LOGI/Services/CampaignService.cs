using LOGI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LOGI.Services
{
    public class CampaignService
    {
        private ApplicationDbContext DbContext;

        public CampaignService(ApplicationDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public Campaign GetCampaign()
        {
            Campaign camp = DbContext.Campaigns.FirstOrDefault();

            return camp;
        }

        public Campaign UpdateCampaign(Campaign campaign, IEnumerable<HttpPostedFileBase> Images, IEnumerable<HttpPostedFileBase> MobileImages)
        {
            Campaign camp = GetCampaign();
            if (camp == null)
            {
                camp = new Campaign();
                DbContext.Campaigns.Add(camp);
            }

            camp.Name = campaign.Name;
            camp.URL = campaign.URL;
            camp.IsOnline = campaign.IsOnline;
            camp.Location = campaign.Location;
            

            try
            {
                DbContext.SaveChanges();

                if (Images != null && Images.Where(i => i != null).Count() > 0)
                {
                    if (camp.ImageUrl != null && camp.ImageUrl != "")
                    {
                        ImageService.DeleteImage(HttpContext.Current.Server.MapPath("~" + camp.ImageUrl));
                    }
                    foreach (HttpPostedFileBase image in Images)
                    {
                        if (image != null)
                        {
                            try
                            {
                                //Original
                                string file_name = "";

                                file_name = ("campaign-" + image.FileName).Replace(" ", "-");


                                System.Drawing.Image web_image = System.Drawing.Image.FromStream(image.InputStream);

                                //save Original Image
                                ImageService.SaveImage((System.Drawing.Image)web_image.Clone(), file_name);

                                //Update the DB value
                                camp.ImageUrl = ImageService.GetImagesDirectory() + file_name;
                                DbContext.SaveChanges();
                            }
                            catch (Exception ex)
                            {
                                
                            }
                        }
                    }
                }

                if (MobileImages != null && MobileImages.Where(i => i != null).Count() > 0)
                {
                    if (camp.MobileImageUrl != null && camp.MobileImageUrl != "")
                    {
                        ImageService.DeleteImage(HttpContext.Current.Server.MapPath("~" + camp.MobileImageUrl));
                    }
                    foreach (HttpPostedFileBase image in MobileImages)
                    {
                        if (image != null)
                        {
                            try
                            {
                                //Original
                                string file_name = "";

                                file_name = ("campaign-s-" + image.FileName).Replace(" ", "-");


                                System.Drawing.Image web_image = System.Drawing.Image.FromStream(image.InputStream);

                                //save Original Image
                                ImageService.SaveImage((System.Drawing.Image)web_image.Clone(), file_name);

                                //Update the DB value
                                camp.MobileImageUrl = ImageService.GetImagesDirectory() + file_name;
                                DbContext.SaveChanges();
                            }
                            catch (Exception ex)
                            {

                            }
                        }
                    }
                }

            }catch (Exception ex)
            {
                return null;
            }

            return camp;
        }

    }
}