using System;
using System.Text;
using System.Text.RegularExpressions;

namespace TextRankCalc
{
    class Program
    {
        static void Main(string[] args)
        {
            var database = RedisStore.database;
            var subscriber = database.Multiplexer.GetSubscriber();
            subscriber.Subscribe("events", (channel, message) => {

                string id = (string)message;
                string value = database.StringGet(id);

                database.ListLeftPush("counter_queue", message);
                database.Multiplexer.GetSubscriber().Publish("counter_hints", "");
            });
            Console.WriteLine("Obsevable subscribe text rank calc is ready. For exit press Enter.");
            Console.ReadLine(); 
        }
    }
}
