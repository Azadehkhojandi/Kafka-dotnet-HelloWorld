using System;
using System.Threading;
using Confluent.Kafka;

namespace Consumer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello Kafka!");

            //https://github.com/edenhill/librdkafka/blob/master/CONFIGURATION.md
            var conf = new ConsumerConfig
            {
                GroupId = "blah-blah-wwwww",
                BootstrapServers = "PLAINTEXT://:9092",
                // Note: The AutoOffsetReset property determines the start offset in the event
                // there are not yet any committed offsets for the consumer group for the
                // topic/partitions of interest. By default, offsets are committed
                // automatically, so in this example, consumption will only start from the
                // earliest message in the topic 'my-topic' the first time you run the program.
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            using (var c = new ConsumerBuilder<Ignore, string>(conf).SetErrorHandler((_, e) =>
                   Console.WriteLine($"Error: {e.Reason}"))
                .SetStatisticsHandler((_, json) =>
                    Console.WriteLine($"Statistics: {json}"))
                .SetRebalanceHandler((_, e) =>
                {
                    if (e.IsAssignment)
                    {
                        Console.WriteLine($"Assigned partitions: [{string.Join(", ", e.Partitions)}]");
                        // possibly override the default partition assignment behavior:
                        // consumer.Assign(...) 
                    }
                    else
                    {
                        Console.WriteLine($"Revoked partitions: [{string.Join(", ", e.Partitions)}]");
                        // consumer.Unassign()
                    }
                })
                .Build())
            {
                c.Subscribe("my-topic-33");

                CancellationTokenSource cts = new CancellationTokenSource();
                Console.CancelKeyPress += (_, e) =>
                {
                    e.Cancel = true; // prevent the process from terminating.
                    cts.Cancel();
                };

                try
                {
                    while (true)
                    {
                        try
                        {
                            var cr = c.Consume(cts.Token);
                            Console.WriteLine($"Consumed message '{cr.Value}' at: '{cr.TopicPartitionOffset}'.");
                        }
                        catch (ConsumeException e)
                        {
                            Console.WriteLine($"Error occured: {e.Error.Reason}");
                        }
                    }
                }
                catch (OperationCanceledException)
                {
                    // Ensure the consumer leaves the group cleanly and final offsets are committed.
                    c.Close();
                }
            }
        }
    }
}

