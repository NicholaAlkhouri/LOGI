using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LOGI.Models
{
    public class KeyIssue
    {
        public int ID { get; set; }

        public int Views { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        [Display(Name = "Online")]
        public bool IsOnline { get; set; }

        [Display(Name = "Is Featured")]
        public bool IsFeatured { get; set; }

        [Display(Name = "Is Home Video Featured")]
        public bool IsHomeVideoFeatured { get; set; }

        [Display(Name = "In scroller")]
        public bool InScroller { get; set; }

        [Display(Name = "Publish Date")]
        public DateTime PublishDate { get; set; }

        [Display(Name = "Friendly URL")]
        public string FriendlyURL { get; set; }

        public string FileURL { get; set; }

        [Display(Name = "Feature Image")]
        public string FeatureImageURL { get; set; }

        [Display(Name = "Feature Video ")]
        [Url]
        public string FeatureVideoLink { get; set; }

         [Display(Name = "Meta Title")]
        public string MetaTitle { get; set; }

        [Display(Name = "Meta Description")]
        public string MetaDescription { get; set; }

        [UIHint("ItemsListWithNull")]
        [Display(Name = "Author")]
        public int? AuthorID { get; set; }

        [UIHint("ItemsListWithNull")]
        [Display(Name = "Source")]
        public int? SourceID { get; set; }

        [UIHint("ForeignKey")]
        [Display(Name = "Topic")]
        public int TopicID { get; set; }

        [UIHint("ItemsListWithNull")]
        [Display(Name = "Type")]
        public int? TypeID { get; set; }

        public bool IsInNews { get; set; }

        public string Language { get; set; }

        [ForeignKey("Original")]
        public int? OriginalId { get; set; }

        public virtual Author Author { get; set; }
        public virtual Source Source { get; set; }
        public virtual Topic Topic { get; set; }
        public virtual Type Type { get; set; }
        public virtual ICollection<Tag> Tags { get; set; }
        public virtual ICollection<Country> Countries { get; set; }
        public virtual KeyIssue Original { get; set; }
    }
}