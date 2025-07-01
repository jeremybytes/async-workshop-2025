using CommandLine;
using digits;
using System.Diagnostics;
using System.Text;

namespace digit_console;

public class Program
{
    private static int offset = 0;
    private static int count = 10;
    private static string classifierString = "";

    static async Task Main(string[] args)
    {
        // Initialize values
        ParseArguments(args);
        List<Prediction> errors = [];

        // Load data
        var (training, validation) = FileLoader.GetData("train.csv", offset, count);

        // Update UI
        Console.Clear();
        Console.WriteLine("Data Load Complete...");

        // Create classifier
        Classifier classifier = classifierString switch
        {
            "euclidean" => new EuclideanClassifier(training),
            "manhattan" => new ManhattanClassifier(training),
            _ => new EuclideanClassifier(training),
        };

        // Start timer
        var timer = new Stopwatch();
        timer.Start();

        // Run the classifier against the data
        //IProgress<int> progress =
        //    new Progress<int>(current =>
        //        PrintProgressBar(current, count));

        IProgress<RecognizerProgress> progress = 
            new Progress<RecognizerProgress>(
            current => 
            {
                PrintProgressBar(current.currentCount, count);
                DisplayImages(current.prediction, false);
            });

        //Spinner.StartSpinner();
        await DigitRecognizer.Run(classifier, validation, errors, progress);
        //Spinner.StopSpinner();

        // Stop the timer
        timer.Stop();
        var elapsed = timer.Elapsed;

        // Show summary (elapsed time, number of errors)
        PrintSummary(classifier, offset, count, elapsed, errors.Count);
        Console.WriteLine("Press [Enter] to show errors...");
        Console.ReadLine();

        // Display errors
        foreach (var item in errors)
        {
            DisplayImages(item, true);
        }

        // Show summary (again)
        PrintSummary(classifier, offset, count, elapsed, errors.Count);
    }

    private static void ParseArguments(string[] args)
    {
        CommandLine.Parser.Default.ParseArguments<Configuration>(args)
            .WithParsed(c =>
            {
                offset = c.Offset;
                count = c.Count;
                classifierString = c.Classifier.ToLower() switch
                {
                    "euclidean" => "euclidean",
                    "manhattan" => "manhattan",
                    _ => "euclidean",
                };
            })
            .WithNotParsed(c =>
            {
                Environment.Exit(0);
            });
    }


    public static void DisplayImages(Prediction prediction, bool scroll)
    {
        if (!scroll)
        {
            Console.SetCursorPosition(0, 1);
        }
        StringBuilder output = new();
        output.Append($"Actual: {prediction.Actual.Value} ");
        output.Append(' ', 46);
        output.AppendLine($" | Predicted: {prediction.Predicted.Value}");
        Display.GetImagesAsString(output, prediction.Actual.Image, prediction.Predicted.Image);
        output.Append('=', 115);
        Console.WriteLine(output);
    }

    public static void PrintSummary(Classifier classifier, int offset, int count, TimeSpan elapsed, int total_errors)
    {
        Console.WriteLine($"Using {classifier.Name} -- Offset: {offset}   Count: {count}");
        Console.WriteLine($"Total time: {elapsed}");
        Console.WriteLine($"Total errors: {total_errors}");
    }

    public static void PrintProgressBar(int currentCount, int totalRecords)
    {
        Console.SetCursorPosition(0, 0);
        int percentComplete = (int)((float)currentCount / (float)totalRecords * 100);
        Console.WriteLine($"{new string('0', percentComplete)}{new string('.', (100- percentComplete))}");
    }
}
