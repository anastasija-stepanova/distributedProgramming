﻿using System;
using System.Collections.Generic;
using Core;
using StackExchange.Redis;

namespace TextListener
{
    class Program
    {
        private static Dictionary<string, string> properties = Configuration.GetParameters();

        static void Main(string[] args)
        {
            Console.WriteLine("Text listener is running.");
            try
            {
                ConnectionMultiplexer redisConnection = ConnectionMultiplexer.Connect("localhost:6379");
                ISubscriber sub = redisConnection.GetSubscriber();
                sub.Subscribe("events", (channel, message) =>
                {
                    string id = message.ToString();
                    if (id.Contains("Text_"))
                    {
                        IDatabase queueDb = redisConnection.GetDatabase(Convert.ToInt32(4));
                        int dbNumber = Message.GetDatabaseNumber(queueDb.StringGet(id));
                        IDatabase redisDb = redisConnection.GetDatabase(dbNumber);
                        string value = redisDb.StringGet(id);
                        Console.WriteLine("Event: " + id + " - " + value);
                    }
                });
                Console.ReadKey();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}