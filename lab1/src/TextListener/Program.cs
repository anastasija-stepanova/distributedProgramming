using System;

namespace TextListener
{
    class Program
    {
        static void Main(string[] args)
        {
            var database = RedisStore.database;
            var subscriber = database.Multiplexer.GetSubscriber();
            subscriber.Subscribe("events", (channel, message) => {
                Console.WriteLine("id: " + (string)message);
            });
            Console.ReadLine(); 
        }
    }
}