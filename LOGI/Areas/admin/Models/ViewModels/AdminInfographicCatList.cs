using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LOGI.Areas.admin.Models.ViewModels
{
    public class AdminInfographicCatList
    {
        [UIHint("ItemsList")]
        [Display(Name = "Category")]
        public int InfographicCategoryID { get; set; }
    }
}