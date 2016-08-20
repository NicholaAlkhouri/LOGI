using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LOGI.Areas.admin.Models.ViewModels
{
    public class AdminTopicsListViewModel
    {
        [UIHint("ItemsList")]
        [Display(Name = "Topic")]
        public int TopicID { get; set; }
    }
}