using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LOGI.Models.ViewModels
{
    public class OfficialDocsViewModel
    {
        public SectionVariable ByLaws { get; set; }

        public SectionVariable Registration { get; set; }

        public SectionVariable GiftAcceptance { get; set; }

        public SectionVariable Whitepaper { get; set; }

        public SectionVariable WhitepaperAr { get; set; }
    }
}