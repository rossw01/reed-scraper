using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReedScraper
{
    public class Job
    {
        public string JobTitle { get; set; }
        public string JobLocation { get; set; }
        public string JobRecruiterName { get; set; }
        public string JobUrl { get; set; }

        public Job(string jobTitle, string jobLocation, string jobRecruiterName, string jobUrl)
        {
            this.JobTitle = jobTitle;
            this.JobLocation = jobLocation;
            this.JobRecruiterName = jobRecruiterName;
            this.JobUrl = jobUrl;
        }

    }
}
