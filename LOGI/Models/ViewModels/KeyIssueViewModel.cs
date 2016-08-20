using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LOGI.Models.ViewModels
{
    public class KeyIssueViewModel
    {
        public KeyIssue KeyIssue { get; set; }

        public List<KeyIssueLink> RelatedReading { get; set; }

        public List<KeyIssueLink> RecommendedReading { get; set; }

        public KeyIssueLink Translation { get; set; }
    }

    public class KeyIssueLink
    {
        public int ID { get; set; }
        public string Title {get;set;}

        public string FriendlyURL {get; set;}

        public string ImageURL { get; set; }

        public string ImageURL2 { get; set; }

        public bool IsVideo { get; set; }

        public string VideoUrl { get; set; }

        public string Quote { get; set; }

        public DateTime PublishDate { get; set; }

        public int? SourceId { get; set; }

        public string  SourceName { get; set; }

        public string SourceNameAr { get; set; }
    }
}