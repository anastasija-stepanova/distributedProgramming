using System;
using System.Collections.Generic;
using Core;
using StackExchange.Redis;

namespace TextRankCalc
{
    class Program
    {
        private static Dictionary<string, string> properties = Configuration.GetParameters();
        public static ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("localhost:6379");
        static void Main(string[] args)
        {
            ISubscriber sub = redis.GetSubscriber();
            sub.Subscribe("events", (channel, message) =>
            {
                string[] elements = message.ToString().Split(":");
                if (elements.Length != 2)
                {
                    return;
                }
                string id = elements[0];
                bool isAccepted = Convert.ToBoolean(elements[1]);
                if (!isAccepted)
                {
                    Console.WriteLine("No access.");
                    return;
                }
                if (id.Contains("Text_"))
                {
                    IDatabase queueDb = redis.GetDatabase(Convert.ToInt32(4));
                    int dbNumber = Message.GetDatabaseNumber(queueDb.StringGet(id));
                    IDatabase redisDb = redis.GetDatabase(dbNumber);
                    string value = redisDb.StringGet(id);
                    SendMessage($"{id}", queueDb);
                    Console.WriteLine("Message sent => " + id + ": " + value);
                }
            });
            Console.ReadKey();
        }

        private static void SendMessage(string message, IDatabase db)
        {
            db.ListLeftPush("counter_queue", message, flags: CommandFlags.FireAndForget);
            db.Multiplexer.GetSubscriber().Publish("counter_hints", "");
        }
    }
}