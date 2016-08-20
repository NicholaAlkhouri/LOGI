using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LOGI.Models
{
    public class InfographicCategory
    {
        public int ID { get; set; }

        public int Order { get; set; }

        public string Name { get; set; }

        public string NameAr { get; set; }

        public string Description { get; set; }

        public string DescriptionAr { get; set; }

        public virtual ICollection<Infographic> Infographics { get; set; }
    }
}