using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LOGI.Models.ViewModels
{
    public class SearchViewModel
    {
        public int Page { get; set; }

        public int PageSize { get; set; }

        public string keyword { get; set; }

        public int? topicId { get; set; }

        public int? sourceId { get; set; }

        public int? authorId { get; set; }

        public int? typeId { get; set; }

        public bool isVideo { get; set; }

        public bool isPDF { get; set; }

        public bool isText { get; set; }

        public int? countryId { get; set; }

        public string language { get; set; }

        public DateTime? fromDate { get; set; }

        public DateTime? toDate {get;set;}

        public string orderby { get; set; }

        public int TotalCount { get; set; }

        public int DisplayFrom { get; set; }

        public int DisplayTo { get; set; }

        public string SortBy { get; set; }

        public bool ShowNext { get; set; }
         
        public bool ShowPrev { get; set; }

        public List<SearchItemViewModel> KeyIssues { get; set; }
    }
}