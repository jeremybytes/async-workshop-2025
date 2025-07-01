using TaskAwait.Library;
using TaskAwait.Shared;

namespace ParallelBasic;

internal class Program
{
    static PersonReader reader = new();

    static async Task Main(string[] args)
    {
        var start = DateTimeOffset.Now;
        Console.Clear();

        var ids = await reader.GetIdsAsync();
        Console.WriteLine(ids.ToDelimitedString(","));

        // Option 1 = Run Sequentially
        await RunSequentially(ids);

        // Option 2 = Task w/ Continuation
        //await RunWithContinuation(ids);

        // Option 3 = Channels
        //await RunWithChannels(ids);

        // Option 4 = ForEachAsync
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
        await Task.Delay(1);
    }

    // Option 3
    private static async Task RunWithChannels(List<int> ids)
    {
        await Task.Delay(1);
    }

    // Option 4
    private static async Task RunWithForEachAsync(List<int> ids)
    {
        await Task.Delay(1);
    }

    private static void DisplayPerson(Person person)
    {
        Console.WriteLine("--------------");
        Console.WriteLine($"{person.Id}: {person}");
        Console.WriteLine($"{person.StartDate:D}");
        Console.WriteLine($"Rating: {new string('*', person.Rating)}");
    }
}
