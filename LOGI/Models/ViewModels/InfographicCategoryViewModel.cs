using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LOGI.Models.ViewModels
{
    public class InfographicCategoryViewModel
    {
        public string Name { get; set; }

        public int Order { get; set; }

        public string Description { get; set; }

        public List<InfographicViewModel> Infographics { get; set; }
    }
}