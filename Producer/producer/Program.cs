using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Confluent.Kafka; //version 0.11.6
using Confluent.Kafka.Serialization; //version 0.11.6

namespace producer
{
    public class Program
    {
        static void Main(string[] args)
        {
            var config = new Dictionary<string, object>
            {
                { "bootstrap.servers", "0.0.0.0:9092" }
            };
            var topic = "my-topic";
            Console.WriteLine($"Producer app, sends messages to {topic}, type exit to stop the app");
            using (var producer = new Producer<Null, string>(config, null, new StringSerializer(Encoding.UTF8)))
            {
                string text = null;
                producer.OnError += (_, e) =>
                {
                    Console.WriteLine(e.Reason);
                };

                while (text != "exit")
                {
                    text = Console.ReadLine();                                                                                                                                                              
                    producer.ProduceAsync(topic, null, text);
                }

                producer.Flush(100);
            }
        }

       
    }
}
