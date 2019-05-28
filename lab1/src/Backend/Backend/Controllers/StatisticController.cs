using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using Core;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    public class StatisticController : Controller
    {
        private static Dictionary<string, string> properties = Configuration.GetParameters();

        [HttpGet("{text_statistic}")]
        public IActionResult Get()
        {
            ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("localhost:6379");
            IDatabase queueDb = redis.GetDatabase(Convert.ToInt32(4));
            for (short i = 0; i < 5; ++i)
            {
                string statistic = queueDb.StringGet("text_statistic");
                if (String.IsNullOrEmpty(statistic))
                {
                    Thread.Sleep(200);
                }
                else
                {
                    return Ok(statistic);
                }
            }

            return new NotFoundResult();
        }

        // POST api/statistic
        [HttpPost]
        public string Post([FromBody] string value)
        {
            return null;
        }

    }
}