using System.Windows;
using TaskAwait.Library;
using TaskAwait.Shared;

namespace ProgressReport.UI.Desktop;

public partial class MainWindow : Window
{
    PersonReader reader = new();
    CancellationTokenSource? tokenSource;
    Progress<int> simpleProgress = new();
    Progress<(int, string)> complexProgress = new();

    public MainWindow()
    {
        InitializeComponent();
        simpleProgress.ProgressChanged += (_, e) =>
        {
            ProgressReporter.Value = e;
            ProgressText.Text = "";
        };
        complexProgress.ProgressChanged += (_, e) =>
        {
            (int percent, string message) = e;
            ProgressReporter.Value = percent;
            ProgressText.Text = percent switch
            {
                0 => "",
                100 => "",
                _ => $"Current Item: {message}",
            };
        };
    }

    private void FetchWithTaskButton_Click(object sender, RoutedEventArgs e)
    {
        tokenSource = new();
        FetchWithTaskButton.IsEnabled = false;
        ClearListBox();

        //Task<List<Person>> peopleTask = reader.GetPeopleAsync(simpleProgress, tokenSource.Token);
        Task<List<Person>> peopleTask = reader.GetPeopleAsync(complexProgress, tokenSource.Token);

        // Always
        peopleTask.ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    foreach (var ex in task.Exception!.Flatten().InnerExceptions)
                    {
                        MessagePopup.ShowMessage("ERROR", $"{ex.GetType()}\n{ex.Message}");
                    }
                }
                if (task.IsCanceled)
                {
                    MessagePopup.ShowMessage("CANCELED", "CANCELED CANCELED CANCELED");
                }
                if (task.IsCompletedSuccessfully)
                {
                    List<Person> people = task.Result;
                    foreach (var person in people)
                    {
                        PersonListBox.Items.Add(person);
                    }
                }

                FetchWithTaskButton.IsEnabled = true;
                tokenSource.Dispose();
            },
            TaskScheduler.FromCurrentSynchronizationContext());
    }

    private async void FetchWithAwaitButton_Click(object sender, RoutedEventArgs e)
    {
        tokenSource = new();
        FetchWithAwaitButton.IsEnabled = false;
        ClearListBox();

        try
        {
            IAsyncEnumerable<Person> people =
                reader.GetPeopleAsyncEnumerable(simpleProgress, tokenSource.Token);
            await foreach (var person in people)
            {
                PersonListBox.Items.Add(person);
            }

            ////List<Person> people = await reader.GetPeopleAsync(simpleProgress, tokenSource.Token);
            //List<Person> people = await reader.GetPeopleAsync(complexProgress, tokenSource.Token);
            //foreach (var person in people)
            //{
            //    PersonListBox.Items.Add(person);
            //}
        }
        catch (OperationCanceledException ex)
        {
            MessagePopup.ShowMessage("CANCELED CANCELED CANCELED", $"{ex.GetType()}\n{ex.Message}");
        }
        catch (Exception ex)
        {
            MessagePopup.ShowMessage("ERROR", $"{ex.GetType()}\n{ex.Message}");
        }
        finally
        {
            FetchWithAwaitButton.IsEnabled = true;
            tokenSource.Dispose();
        }
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            tokenSource?.Cancel();
        }
        catch (ObjectDisposedException)
        {
            // Ignore this exception
            // This will happen if a process is complete
            // and someone clicks "Cancel" before starting
            // another process.
        }
    }

    private void ClearListBox()
    {
        PersonListBox.Items.Clear();
    }
}
