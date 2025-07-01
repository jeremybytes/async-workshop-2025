using DataProcessor.Library;

namespace DataProcessor;

internal class Program
{
    static async Task Main(string[] args)
    {
        var records = await ProcessData().ConfigureAwait(false);

        Console.WriteLine($"Successfully processed {records.Count()} records");
        foreach (var person in records)
        {
            Console.WriteLine(person);
        }
        Console.WriteLine("Press [Enter] to continue...");
        Console.ReadLine();
    }

    static async Task<IReadOnlyCollection<Person>> ProcessData()
    {
        DataLoader loader = new();
        IReadOnlyCollection<string> data = loader.LoadData();

        FileLogger logger = new();
        DataParser parser = new(logger);
        IReadOnlyCollection<Person> records = 
            await parser.ParseData(data).ConfigureAwait(false);
        return records;
    }
}
