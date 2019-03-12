using StackExchange.Redis;
using System;
using System.Configuration;

namespace TextRankCalc
{
    public class RedisStore
    {
        public static ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("localhost:6379");
        public static IDatabase database = redis.GetDatabase();
    }
}