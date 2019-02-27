using System;

namespace TextListener
{
    class Program
    {
        static void Main(string[] args)
        {
            var redis = RedisStore.RedisCache;
            var sub = redis.Multiplexer.GetSubscriber();
            sub.Subscribe("events", (channel, message) => {
                Console.WriteLine("шd: " + (string)message);
                Console.WriteLine("val: " + redis.StringGet((string)message));
            });
            Console.ReadLine();
        }
    }
}
