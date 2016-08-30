using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LOGI.Areas.Admin.Models
{
    public class Navigator
    {
        public enum Main {NONE, ABOUT,AUTHOR,TOPIC,SOURCE,KEYISSUES,NEWS,CONTACTS,SUBSCRIBE,DONATE,JOINLOGI,TIMELINE,INPHOGRAPHIC,CAMPAIGN}

        public enum Sub { NONE, TEAM, FINANCIAL, VACANCIES, EXPERTS, SELECTIONPOLICY,NEWSLIST,NEWSRELEASE,OFFICIALDOC,INFOCATEGORY, INFOGRAPHIC }
    }
}