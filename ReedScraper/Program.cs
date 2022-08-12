using System.Net;
using System.Text;
using System.Net.Http;

namespace ReedScraper
{
    class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Fetching data...");
            Console.WriteLine();

            string url = "https://www.reed.co.uk/jobs/software-engineer-jobs-in-london?pageno=1";

            List<Job> Jobs = new List<Job>();

            string html = GetHtml(url);

            var jobAttributes = new Dictionary<string, string>
            {
                ["jobTitle"] = "data-jobtitle=\"",
                ["jobLocation"] = "data-joblocation=\"",
                ["jobRecruiterName"] = "data-jobrecruitername=\"",
                ["jobUrl"] = "data-joburl=\""
            };

            bool jobsRemain = true;

            while (jobsRemain)
            {
                var jobListingInfo = new Dictionary<string, string>();

                foreach (var attribute in jobAttributes)
                {
                    // Finds indexes of jobTitle, jobLocation, jobRecruiterName, jobUrl in html to parse
                    int startIndex = html.IndexOf(attribute.Value) + attribute.Value.Length;
                    html = html[startIndex..]; // trim previous html...
                    int endIndex = html.IndexOf("\""); // it will read up to next " mark

                    jobListingInfo.Add(attribute.Key, html.Substring(0, endIndex));
                }
                /*
                foreach (var item in jobListingInfo) // remove after testing
                {
                    Console.WriteLine(item);
                }
                */

                // If there are no matches, add object
                if (!CheckJobDescriptionForKeywords(jobListingInfo["jobUrl"]))
                {
                    var newJob = new Job(jobListingInfo["jobTitle"],
                                         jobListingInfo["jobLocation"],
                                         jobListingInfo["jobRecruiterName"],
                                         jobListingInfo["jobUrl"]);

                    Jobs.Add(newJob);
                }

                //Checks if there are no more jobs on the page, breaks out of loop if needed
                if (!html.Contains(jobAttributes["jobTitle"]))
                {
                    jobsRemain = false;
                }

                WriteToFile(Jobs, url); //output file with results
            }
        }

        private static async void WriteToFile(List<Job> Jobs, string url)
        {
            var lines = new string[Jobs.Count];

            for (int i = 0; i < lines.Length; i++)
            {
                lines[i] = $"{Jobs[i].JobTitle}\n" +
                           $"{Jobs[i].JobLocation}\n" +
                           $"{Jobs[i].JobRecruiterName}\n" +
                           $"{url.Substring(0, 22)}{Jobs[i].JobUrl}\n";

            }

            await File.WriteAllLinesAsync("Output.txt", lines);
        }


        private static bool CheckJobDescriptionForKeywords(string urlSuffix)
        {
            string url = $"https://www.reed.co.uk{urlSuffix}";
            string html = GetHtml(url);

            string jobDescription = ParseJobDescription(html);

            if (jobDescription.Contains("Degree") ||
                jobDescription.Contains("Bachelors") ||
                jobDescription.Contains("Bachelor's") ||
                jobDescription.Contains("BSc"))
            {
                return true; // if it contains blacklisted word, return true
            }

            else
            {
                return false;
            } // otherwise return the job description for object creation.
        }

        private static string ParseJobDescription(string html)
        {
            string descriptionTag = "branded-job--description";
            string descriptionEndTag = "InternalApplicationSource";

            int startIndex = html.IndexOf(descriptionTag) + descriptionTag.Length;
            int endIndex = html.IndexOf(descriptionEndTag);


            return html.Substring(startIndex, (endIndex - startIndex)).Trim();
        }



        private static string GetHtml(String url)
        {
            string html;

            using (var client = new HttpClient())
            {
                html = client.GetStringAsync(url).Result;
            }

            return html;

        }

        /*
	       <div class="job-results-actions">
                <button type="button"
                        tabindex="0"
                        title="Shortlist job"
                        aria-label="Shortlist job"
                        data-qa="shortlistBtn"
                        data-jobid="47770002"
                        data-draft="0"
                        data-jobtitle="Software Engineer"
                        data-joblocation="London"
                        data-jobrecruitername="Lorien"
                        data-joburl="/jobs/software-engineer/47770002?source=searchResults&amp;filter=%2fjobs%2fsoftware-engineer-jobs-in-london"
                        class="btn btn-secondary btn-inline btn-shortlist-job  gtmSavedJobsButton"
                        data-bind="click: function(data,e) { searchResults.modifyShortlistStatus(data,e) }, event: { keypress: function(data, e) { if (event.keyCode === 32 || event.keyCode === 13) { searchResults.modifyShortlistStatus(data,e) } } }">
                    <i class="icon"></i>
                </button>

		 */

    }
}