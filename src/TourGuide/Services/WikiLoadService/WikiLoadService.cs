using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace TourGuide.Services
{
    public class WikiLoadService
    {
        public async Task<WikiLoadServiceResult> Lookup(WikiLoadServiceParams wikiParams)
        {
            var result = new WikiLoadServiceResult()
            {
                Success = false,
                Message = "Looking up wiki pages wasn't successful"
            };

            //Lookup wiki pages
            var bingKey = Startup.Configuration["AppSettings:BingKey"];
            var url = $"https://wdq.wmflabs.org/api?q=AROUND[625,{wikiParams.Latitude},{wikiParams.Longitude},{wikiParams.Radius}]&callback=JSON_CALLBACK";

            var client = new HttpClient();

            var json = await client.GetStringAsync(url);

            if(json == null)
            {

            }
            else
            {
                result.Success = true;
                result.Url = url;
                result.Content =  json;
            }

            return result;

        }
    }
}
