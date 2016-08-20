using LOGI.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace LOGI.Services
{
    public class ImageGalleryService
    {
        public string addImage(IEnumerable<HttpPostedFileBase> Images)
        {
            foreach (HttpPostedFileBase image in Images)
            {
                if (image != null)
                {
                    try
                    {
                        FilesService fs = new FilesService();
                        //Original
                        string file_name = "";
                        file_name = (fs.RandomString() + "external-" + image.FileName).Replace(" ", "-");


                        System.Drawing.Image web_image = System.Drawing.Image.FromStream(image.InputStream);

                        //save Original Image
                        ImageService.SaveImage((System.Drawing.Image)web_image.Clone(), file_name, 0, 0, "/Images/ckuploades/");

                        //LOGI.Models.Image final_img = new LOGI.Models.Image() { URL = "/Images/ckuploades/" + file_name };
                        //Update the DB value
                        //DbContext.Images.Add(final_img);
                        //DbContext.SaveChanges();
                        return "/Images/ckuploades/" + file_name;
                    }
                    catch (Exception ex)
                    {
                        return "";
                    }
                }
            }
            return "";
        }

        public List<string> getImages()
        {
            List<string> images = Directory.GetFiles(HttpContext.Current.Server.MapPath("~/Images/ckuploades/")).ToList();

            List<string> final_list = new List<string>();
            foreach(string img in images)
            {
                final_list.Add("/Images/ckuploades/" + Path.GetFileName(img));
            }
            return final_list;
        }

        public bool deleteImage(string name)
        {
            try {
                if (File.Exists(HttpContext.Current.Server.MapPath("~"+ name)))
                    File.Delete(HttpContext.Current.Server.MapPath("~" + name));
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }
    }
}