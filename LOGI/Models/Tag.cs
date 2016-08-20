using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LOGI.Models
{
    public class Tag
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public string Language { get; set; }

        public int KeyIssueID { get; set; }

        public virtual KeyIssue KeyIssue { get; set; }
    }
}