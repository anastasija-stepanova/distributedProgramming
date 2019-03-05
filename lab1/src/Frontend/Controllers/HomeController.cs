using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Frontend.Models;
using System.Net.Http;
using System.IO;
using System.Net.Http.Headers;

namespace Frontend.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Upload()
        {
            return View();
        }
       
        [HttpGet]
        public IActionResult TextDetails()
        {
            return View();      
        }

        [HttpPost]
        public async Task<IActionResult> Upload(string data)
        {
            string id = null; 
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("http://127.0.0.1:4888/");
            FormUrlEncodedContent content = new FormUrlEncodedContent(new[] { 
                new KeyValuePair<string, string>("value", data) 
            });
            HttpResponseMessage response = await httpClient.PostAsync("/api/values", content);
            id = await response.Content.ReadAsStringAsync();
            return Ok(id);
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
