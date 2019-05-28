using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using Core;

namespace Frontend.Controllers
{
    
    public class StatisticsController : Controller
    {
        private static Dictionary<string, string> properties = Configuration.GetParameters();
        
        [HttpGet]
        public async Task<IActionResult> TextStatistic()
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync("http://127.0.0.1:4888/api/statistic/text_statistic");
            string result = await response.Content.ReadAsStringAsync();

            Console.WriteLine("Statistics -> " + result);
            ViewData["rank"] = result;
            return View();
        }

    }
}