using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;
using StackExchange.Redis;
using System.Configuration;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        static readonly ConcurrentDictionary<string, string> _data = new ConcurrentDictionary<string, string>();
        public static ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("localhost:6379");
        public static IDatabase database = redis.GetDatabase(); 

        // POST api/values
        [HttpPost]
        public string Post([FromForm]string value)
        {
            var id = Guid.NewGuid().ToString();
            this.SaveToDatabase(database, id, value);
            this.CreateEvent(redis, id, "events", value);
            return id;
        }

        private void CreateEvent(StackExchange.Redis.ConnectionMultiplexer redis, String id, string events, String value = null)
        {
            ISubscriber sub = redis.GetSubscriber();
            sub.Publish(events, id);
        }
        private void SaveToDatabase(StackExchange.Redis.IDatabase database, string id, string value)
        {
            database.StringSet(id, value);
        }

        // GET api/values/<id>
        [HttpGet("{id}")]
        public string Get([FromRoute] string id)
        {
            string value = null;
            _data.TryGetValue(id, out value);
            string currentId = (string)id;
            var data = database.StringGet("Rank_" + currentId);
            Console.WriteLine("id: " + data);
            Console.WriteLine("currentId: " + "Rank_" + currentId);
            return data;
        }

    }
}