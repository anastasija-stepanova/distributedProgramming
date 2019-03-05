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
            subscriber.Subscribe("TextCreated", (channel, message) => {
                Console.WriteLine("id: " + database.StringGet((string)message));

                int vowels = Regex.Matches(database.StringGet((string)message), @"[aeiouy]", RegexOptions.IgnoreCase).Count;
                int consonants = Regex.Matches(database.StringGet((string)message), @"[bcdfghjklmnpqrstvwxz]", RegexOptions.IgnoreCase).Count;
                double rank = (double)vowels / consonants;
                database.StringSet((string)message, rank);
                Console.WriteLine("Rank: " + database.StringGet((string)message));
            });

            Console.ReadLine(); 
        }
    }
}
