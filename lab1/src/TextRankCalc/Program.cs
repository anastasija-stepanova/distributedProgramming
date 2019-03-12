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

                int vowels = Regex.Matches(database.StringGet((string)message), @"[aeiouy]", RegexOptions.IgnoreCase).Count;
                int consonants = Regex.Matches(database.StringGet((string)message), @"[bcdfghjklmnpqrstvwxz]", RegexOptions.IgnoreCase).Count;
                double rank = (double)vowels / consonants;
                string id = "Rank_" + (string)message;
                database.StringSet(id, rank);
                Console.WriteLine("string message: " + id);
                Console.WriteLine("message: " + id);
                Console.WriteLine("Rank: " + database.StringGet(id));
            });

            Console.ReadLine(); 
        }
    }
}
