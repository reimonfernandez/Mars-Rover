using MarsRover.Service.Contracts;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Configuration;
using System.Net.Http;

namespace MarsRover.Service.Controllers
{
    class MarsRoverService : IMarsRoverService
    {
        readonly HttpClient _webApiClient;
        public MarsRoverService(HttpClient webApiClient)
        {
            _webApiClient = webApiClient;
            _webApiClient.BaseAddress = new Uri(ConfigurationManager.AppSettings["NASAAPIUri"]);
            _webApiClient.DefaultRequestHeaders.Clear();
            _webApiClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        public HttpResponseMessage GetPhotosByDate(string date)
        {
            string route = string.Format("mars-photos/api/v1/rovers/curiosity/photos?earth_date={0}&api_key=DEMO_KEY", date);
            HttpResponseMessage response = _webApiClient.GetAsync(route).Result;
            return response;
        }

        public List<string> ExtractTextLinesFromFile(System.Web.HttpPostedFileBase file)
        {
            List<string> textList = new List<string>();

            using (StreamReader sr = new StreamReader(file.InputStream))
            {
                string s = String.Empty;
                while ((s = sr.ReadLine()) != null)
                {
                    textList.Add(s.Trim());
                }
            }

            return textList;
        }

        public List<DateTime> GetAllValidDates(List<string> linesOfText)
        {
            List<DateTime> validDates = new List<DateTime>();

            if (linesOfText != null)
            {
                List<string> customPatterns = new List<string>();
                customPatterns.Add("MM/dd/yy");
                customPatterns.Add("MMMM dd',' yyyy");

                foreach (string line in linesOfText)
                {
                    DateTime dt = DateTime.MinValue;
                    if (DateTime.TryParse(line, out dt))
                    {
                        validDates.Add(dt);
                    }
                    else
                    {
                        foreach (string pattern in customPatterns)
                        {
                            if (DateTime.TryParseExact(line, pattern, null, DateTimeStyles.None, out dt))
                            {
                                validDates.Add(dt);
                                break;
                            }
                        }
                    }
                }
            }

            return validDates;
        }
    }
}
