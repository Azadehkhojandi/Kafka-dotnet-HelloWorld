using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Confluent.Kafka;
using Confluent.Kafka.Serialization;

namespace Consumer
{
    public class Program
    {
        static void Main(string[] args)
        {
            var config = new Dictionary<string, object>
            {
                { "group.id", "sample-consumer" },
                { "bootstrap.servers", "127.0.0.1:9092" },
                { "enable.auto.commit", "false"}
            };
            var topic = "my-topic";
            Console.WriteLine($"reads messages in {topic}");
            using (var consumer = new Consumer<Null, string>(config, null, new StringDeserializer(Encoding.UTF8)))
            {                
                consumer.Subscribe(new string[]{topic});

                consumer.OnError +=(_, e) =>
                {
                    Console.WriteLine(e.Reason);
                };

                consumer.OnMessage += (_, msg) => 
                {
                    Console.WriteLine($"Topic: {msg.Topic} Partition: {msg.Partition} Offset: {msg.Offset} {msg.Value}");
                    consumer.CommitAsync(msg);
                };

                while (true)
                {
                    consumer.Poll(100);
                }
            }
        }

        
    }
}

