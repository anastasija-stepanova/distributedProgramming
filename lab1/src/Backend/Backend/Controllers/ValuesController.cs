using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;
using StackExchange.Redis;
using System.Threading;
using Core;
using Newtonsoft.Json;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private static Dictionary<string, string> properties = Configuration.GetParameters();
        static readonly ConcurrentDictionary<string, string> _data = new ConcurrentDictionary<string, string>();

        [HttpGet("{rank}")]
        public IActionResult Get([FromQuery] string id)
        {
            ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("localhost:6379");
            IDatabase database = redis.GetDatabase(Convert.ToInt32(4)); 
            string location = database.StringGet("Text_" + id);
            IDatabase redisDb = redis.GetDatabase(Message.GetDatabaseNumber(location));
            for (short i = 0; i < 5; ++i)
            {
                string rank = redisDb.StringGet("TextRank_" + id);
                if (rank == null)
                {
                    Thread.Sleep(200);
                }
                else
                {
                    return Ok(rank + " Location=" + location);
                }
            }

            return new StatusCodeResult(402);
    }

        // POST api/values
        [HttpPost]
        public string Post([FromBody] string value)
        {
            var id = Guid.NewGuid().ToString();
            Message data = new Message(value.Split(':')[0], value.Split(':')[1]);
            string textKey = "Text_" + id;
            data.SetID(textKey);
            this.SaveDataToRedis(data);
            this.makeEvent(ConnectionMultiplexer.Connect("localhost:6379"), data);

            return id;
        }
        private void SaveDataToRedis(Message message)
        {
            var redisDb = ConnectionMultiplexer.Connect("localhost:6379").GetDatabase(message.GetDatabase());
            redisDb.StringSet(message.GetId(), message.GetMessage());
            var database = ConnectionMultiplexer.Connect("localhost:6379").GetDatabase(Convert.ToInt32(4));
            database.StringSet(message.GetId(), message.GetLocation());
            Console.WriteLine(message.GetId() + ": " + message.GetMessage() + " - saved to redis " + message.GetLocation() + " : " + message.GetDatabase());
        }
        private void makeEvent(ConnectionMultiplexer redis, Message data)
        {
            ISubscriber sub = redis.GetSubscriber();
            sub.Publish("events", $"{data.GetId()}");
        }
    }
}