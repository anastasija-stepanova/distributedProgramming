using StackExchange.Redis;
using System;
using System.Configuration;

namespace TextListener
{
    public class RedisStore
    {
        public static ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("http://localhost:6379");
        public static IDatabase RedisCache = redis.GetDatabase();
    }
}