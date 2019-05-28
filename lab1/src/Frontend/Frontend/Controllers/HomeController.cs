using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Frontend.Models;
using System.Net.Http;
using Newtonsoft.Json;
using Core;

namespace Frontend.Controllers
{
    public class HomeController : Controller
    {
        private static Dictionary<string, string> properties = Configuration.GetParameters();
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Upload()
        {
            return View();
        }

        public IActionResult TextDetails(string id)
        {
	        string rankRoute = "http://127.0.0.1:4888/api/values/rank?" + id;
			string details = SendGetRequest(rankRoute).Result;
			ViewData["rank"] = details;
			return View();
		}

        [HttpPost]
        public IActionResult Upload(string data, string location)
        {
            string id = null; 
            string url = "http://127.0.0.1:4888/api/values/";
            HttpClient client = new HttpClient();
            Console.WriteLine("User entered data: " + data + " Location: " + location);
            string str = $"{data}:{location}";
            var response = client.PostAsJsonAsync(url, str);
            id = response.Result.Content.ReadAsStringAsync().Result;
            return new RedirectResult("http://127.0.0.1:5000/Home/TextDetails/id=" + id);
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private async Task<string> SendGetRequest(string requestUri)
		{
            HttpClient client = new HttpClient();
			var response = await client.GetAsync(requestUri);
			string value = await response.Content.ReadAsStringAsync();
			if (response.IsSuccessStatusCode && value != null)
			{
				return value;
			}
			return response.StatusCode.ToString();
		}
    }
}