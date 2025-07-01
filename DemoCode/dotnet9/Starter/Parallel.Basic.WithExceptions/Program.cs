using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using TaskAwait.Library;
using TaskAwait.Shared;

namespace ParallelBasic.WithExceptions;

class Program
{
    static PersonReader reader = new();

    static async Task Main(string[] args)
    {
        var start = DateTimeOffset.Now;
        Console.Clear();

        var ids = await reader.GetIdsAsync();
        // Insert invalid values for exception testing
        ids = InjectBadIds(ids);

        Console.WriteLine(ids.ToDelimitedString(","));

        // Option 1 = Run Sequentially
        await RunSequentially(ids);

        // Option 2 = Task w/ Continuation
        //await RunWithContinuation(ids);

        // Option 3 = Channels
        //await RunWithChannel(ids);

        // Option 4 = ForEachAsync
        //await RunWithForEachAsync(ids);

        var elapsed = DateTimeOffset.Now - start;
        Console.WriteLine($"\nTotal time: {elapsed}");

        Console.ReadLine();
    }

    // Option 1
    static async Task RunSequentially(List<int> ids)
    {
        // No exception handling
        foreach (var id in ids)
        {
            var person = await reader.GetPersonAsync(id);
            DisplayPerson(person);
        }
    }

    // Option 2
    static async Task RunWithContinuation(List<int> ids)
    {
        // No exception handling
        object lockonme = new();
        List<Task> continuations = new();

        foreach (var id in ids)
        {
            Task<Person> personTask = reader.GetPersonAsync(id);
            Task continuation = personTask.ContinueWith(task =>
            {
                Person selectedPerson = task.Result;
                lock (lockonme)
                {
                    DisplayPerson(selectedPerson);
                }
            });
            continuations.Add(continuation);
        }

        await Task.WhenAll(continuations);
    }

    // Option 3
    static async Task RunWithChannel(List<int> ids)
    {
        Channel<Person> channel = Channel.CreateBounded<Person>(10);

        Task consumer = ShowData(channel.Reader);
        Task producer = ProduceData(ids, channel.Writer);

        await producer;
        await consumer;
    }

    private static async Task ShowData(ChannelReader<Person> reader)
    {
        await foreach (var person in reader.ReadAllAsync())
        {
            DisplayPerson(person);
        }
    }

    private static async Task ProduceData(List<int> ids, ChannelWriter<Person> writer)
    {
        List<Task> allTasks = new();

        foreach (var id in ids)
        {
            Task currentTask = FetchRecord(writer, id);
            allTasks.Add(currentTask);
        }

        await Task.WhenAll(allTasks);
        writer.Complete();
    }

    private static async Task FetchRecord(ChannelWriter<Person> writer, int id)
    {
        // No exception handling
        var person = await reader.GetPersonAsync(id);
        await writer.WriteAsync(person);
    }

    // Option 4: Parallel.ForEachAsync
    static async Task RunWithForEachAsync(List<int> ids)
    {
        // No exception handling
        await Parallel.ForEachAsync(ids,
            new ParallelOptions() { MaxDegreeOfParallelism = 3 },
            async (id, _) =>
            {
                var person = await reader.GetPersonAsync(id);
                lock (ids)
                {
                    DisplayPerson(person);
                }
            });
    }

    static void DisplayPerson(Person person)
    {
        Console.WriteLine("--------------");
        Console.WriteLine($"{person.Id}: {person}");
        Console.WriteLine($"{person.StartDate:D}");
        Console.WriteLine($"Rating: {new string('*', person.Rating)}");
    }

    static void DisplayException(Exception ex)
    {
        if (ex is AggregateException)
        {
            StringBuilder message = new();
            message.AppendLine("XXXXXXX");
            message.AppendLine("AGGREGATE EXCEPTION");
            foreach (var inner in ((AggregateException)ex).Flatten().InnerExceptions)
            {
                message.AppendLine("  Exception: ");
                message.AppendLine($"    {inner.GetType().ToString()}");
                message.AppendLine($"    {inner.Message}");
            }
            message.AppendLine("XXXXXXX");
            Console.Write(message);
        }
        else
        {
            StringBuilder message = new();
            message.AppendLine("XXXXXXX");
            message.AppendLine("Exception: ");
            message.AppendLine($"  {ex.GetType().ToString()}");
            message.AppendLine($"  {ex.Message}");
            message.AppendLine("XXXXXXX");
            Console.Write(message);
        }
    }

    static List<int> InjectBadIds(List<int> ids)
    {
        ids.Insert(3, -30);
        ids.Insert(6, 100);
        ids.Insert(12, -1);
        return ids;
    }
}

public static class Aggregate
{
    internal static async Task WithAggregateException(this Task task)
    {
        await task.ConfigureAwait(ConfigureAwaitOptions.SuppressThrowing);
        task.Wait();
    }
}