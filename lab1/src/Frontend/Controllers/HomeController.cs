using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Frontend.Models;
using System.Net.Http;
using System.IO;

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

        [HttpPost]
        public async Task<IActionResult> Upload(string data)
        {
            string id = null; 
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("http://127.0.0.1:4888/");
            string json = JsonConvert.SerializeObject(data);
            HttpContent content = new StringContent(json);
            HttpResponseMessage response = await httpClient.PostAsync("/api/values", content);
            id = await response.Content.ReadAsStringAsync();
            return Redirect("~/Home/TextDetails");
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}