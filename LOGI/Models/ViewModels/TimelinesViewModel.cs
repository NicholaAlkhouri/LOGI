using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LOGI.Models.ViewModels
{
    public class TimelinesViewModel
    {
        public List<TimelineViewModel> Timelines { get; set; }

        public int? countryId { get; set; }

        public string category { get; set; }

        public List<Country> Countries { get; set; }
    }
}