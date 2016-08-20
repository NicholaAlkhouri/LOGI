using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LOGI.Models.ViewModels
{
    public class KeyIssuesViewModel
    {
        public List<Topic> Topics { get; set; }

        public KeyIssueLink FeatureArticle { get; set; }

        public KeyIssueLink FeatureVideo { get; set; }

        public KeyIssueLink TweetArticle { get; set; }

    }
}