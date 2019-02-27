using System;
using System.Text;

namespace TextListener
{
    class Program
    {
        static void Main(string[] args)
        {
            var redis = RedisStore.Database;
            var sub = redis.Multiplexer.GetSubscriber();
            sub.Subscribe("TextCreated", (channel, message) => {
               var body = message;
               var messageId = Encoding.UTF8.GetString(body);
               Console.WriteLine("Message: ", redis.StringGet(messageId));
            });
            Console.ReadLine();
        }
    }
}
