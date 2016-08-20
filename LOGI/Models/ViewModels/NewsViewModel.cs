using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LOGI.Models.ViewModels
{
    public class NewsViewModel
    {
        public SearchViewModel Search { get; set; }

        public KeyIssueLink FeatureVideo { get; set; }

        public List<KeyIssueLink> LatestNews { get; set; }

        public List<SourceViewModel> Sources { get; set; }

        public string PressRelease { get; set; }
    }
}