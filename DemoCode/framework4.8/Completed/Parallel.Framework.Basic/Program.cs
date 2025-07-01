using System;
using System.Collections.Generic;
using System.Threading.Channels;
using System.Threading.Tasks;
using TaskAwait.Framework.Library;
using TaskAwait.Framework.Shared;

namespace ParallelFramework.Basic
{
    internal class Program
    {
        static PersonReader reader = new PersonReader();

        static async Task Main(string[] args)
        {
            var start = DateTimeOffset.Now;
            Console.Clear();

            var ids = await reader.GetIdsAsync();
            Console.WriteLine(ids.ToDelimitedString(","));

            // Option 1 = Run Sequentially
            //await RunSequentially(ids);

            // Option 2 = Task w/ Continuation
            //await RunWithContinuation(ids);

            // Option 3 = Channels
            await RunWithChannels(ids);

            // Option 4 = ForEachAsync
            // PARALLEL.FOREACHASYNC IS NOT AVAILABLE
            // IN .NET FRAMEWORK 4.8
            //await RunWithForEachAsync(ids);

            var elapsed = DateTimeOffset.Now - start;
            Console.WriteLine($"\nTotal time: {elapsed}");

            Console.ReadLine();
        }

        // Option 1
        private static async Task RunSequentially(List<int> ids)
        {
            foreach (var id in ids)
            {
                var person = await reader.GetPersonAsync(id);
                DisplayPerson(person);
            }
        }

        // Option 2
        private static async Task RunWithContinuation(List<int> ids)
        {
            List<Task> allContinuations = new List<Task>();

            foreach (var id in ids)
            {
                Task<Person> personTask = reader.GetPersonAsync(id);
                Task continuation = personTask.ContinueWith(task =>
                {
                    var person = task.Result;
                    lock (ids)
                    {
                        DisplayPerson(person);
                    }
                });
                allContinuations.Add(continuation);
            }

            await Task.WhenAll(allContinuations);
        }

        // Option 3
        private static async Task RunWithChannels(List<int> ids)
        {
            var channel = Channel.CreateBounded<Person>(10);

            Task consumer = ShowData(channel.Reader);
            Task producer = ProduceData(ids, channel.Writer);

            await producer;
            await consumer;
        }

        private static async Task ShowData(ChannelReader<Person> reader)
        {
            while (await reader.WaitToReadAsync())
            {
                while (reader.TryRead(out Person person))
                {
                    DisplayPerson(person);
                }
            }
        }

        private static async Task ProduceData(List<int> ids, ChannelWriter<Person> writer)
        {
            List<Task> allTasks = new List<Task>();

            foreach (var id in ids)
            {
                allTasks.Add(FetchRecord(id, writer));
            }

            await Task.WhenAll(allTasks);
            writer.Complete();
        }

        private static async Task FetchRecord(int id, ChannelWriter<Person> writer)
        {
            var person = await reader.GetPersonAsync(id);
            await writer.WriteAsync(person);
        }

        private static void DisplayPerson(Person person)
        {
            Console.WriteLine("--------------");
            Console.WriteLine($"{person.Id}: {person}");
            Console.WriteLine($"{person.StartDate:D}");
            Console.WriteLine($"Rating: {new string('*', person.Rating)}");
        }
    }
}
