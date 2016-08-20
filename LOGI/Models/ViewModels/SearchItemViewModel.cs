using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LOGI.Models.ViewModels
{
    public class SearchItemViewModel
    {
        public int ID { get; set; }

        public string Title { get; set; }

        public string FriendlyURL { get; set; }

        public string ImageURL { get; set; }

        public DateTime PublishDate { get; set; }

        public string Author { get; set; }

        public int? AuthorId { get; set; }

        public string Source { get; set; }

        public int? SourceID { get; set; }
    }
}