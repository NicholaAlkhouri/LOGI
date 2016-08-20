using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LOGI
{
    public class LogiConfig
    {
        public static List<ImageSize> KeyIssuesImageSizes = new List<ImageSize>()
        {
            new ImageSize() {Name = "SmallThumb", Width = 310, Height = 264},
            new ImageSize() {Name = "BigThumb",Width = 520, Height = 264},
            new ImageSize() {Name = "SearchThumb",Width = 410, Height = 210},
            new ImageSize() {Name = "Feature",Width = 830, Height = 460}
        };

    }

    public class ImageSize
    {
        public string Name { get; set; }
        public int Width {get;set; }

        public int Height {get;set;}
    }
}