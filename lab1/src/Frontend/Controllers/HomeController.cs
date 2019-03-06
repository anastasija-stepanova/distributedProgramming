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
using System.Web;
using Microsoft.AspNetCore.Http.Extensions; 

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
        public async Task<IActionResult> TextDetails(string id)
        {
            ViewData["test"] = id;

HttpClient httpClient = new HttpClient();
        //  HttpResponseMessage response = await httpClient.GetStringAsync($"http://127.0.0.1:4888/api/values/{id}");
        //  response.EnsureSuccessStatusCode();
        //  string responseBody = await response.Content.ReadAsStringAsync();

         Console.WriteLine(await httpClient.GetStringAsync($"http://127.0.0.1:4888/api/values/{id}"));
    
            // HttpClient httpClient = new HttpClient();
            // httpClient.BaseAddress = new Uri("http://127.0.0.1:4888/");
            // HttpResponseMessage response = await httpClient.GetAsync("/api/values", id);
            // var data = await response.Content.ReadAsStringAsync();
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
            return new RedirectResult("http://localhost:5000/Home/TextDetails?id=" + id);
            return Ok(id);
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
