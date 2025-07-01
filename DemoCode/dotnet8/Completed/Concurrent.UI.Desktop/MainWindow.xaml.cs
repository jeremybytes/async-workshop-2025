using System.Windows;
using TaskAwait.Library;
using TaskAwait.Shared;

namespace Concurrent.UI.Desktop;

public partial class MainWindow : Window
{
    private PersonReader reader = new();
    private CancellationTokenSource? tokenSource;

    public MainWindow()
    {
        InitializeComponent();
    }

    // Using Task (Single Continuations)
    private void FetchWithTaskButton_Click(object sender, RoutedEventArgs e)
    {
        tokenSource = new CancellationTokenSource();
        FetchWithTaskButton.IsEnabled = false;
        ClearListBox();

        Task<List<Person>> peopleTask = reader.GetPeopleAsync(tokenSource.Token);

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

                tokenSource.Dispose();
                FetchWithTaskButton.IsEnabled = true;
            },
            TaskScheduler.FromCurrentSynchronizationContext());
    }

    // Using Task (Multiple Continuations)
    //private void FetchWithTaskButton_Click(object sender, RoutedEventArgs e)
    //{
    //    FetchWithTaskButton.IsEnabled = false;
    //    ClearListBox();

    //    Task<List<Person>> peopleTask = reader.GetPeopleAsync();

    //    // Success
    //    peopleTask.ContinueWith(task =>
    //    {
    //        List<Person> people = task.Result;
    //        foreach (var person in people)
    //        {
    //            PersonListBox.Items.Add(person);
    //        }
    //    },
    //        CancellationToken.None,
    //        TaskContinuationOptions.OnlyOnRanToCompletion,
    //        TaskScheduler.FromCurrentSynchronizationContext());

    //    // Falted / Exception
    //    peopleTask.ContinueWith(task =>
    //    {
    //        foreach (var ex in task.Exception!.Flatten().InnerExceptions)
    //            MessagePopup.ShowMessage("ERROR", $"{ex.GetType()}\n{ex.Message}");
    //    },
    //        CancellationToken.None,
    //        TaskContinuationOptions.OnlyOnFaulted,
    //        TaskScheduler.FromCurrentSynchronizationContext());

    //    // Always
    //    peopleTask.ContinueWith(task =>
    //    {
    //        FetchWithTaskButton.IsEnabled = true;
    //    },
    //        TaskScheduler.FromCurrentSynchronizationContext());
    //}

    // Using Await
    private async void FetchWithAwaitButton_Click(object sender, RoutedEventArgs e)
    {
        tokenSource = new CancellationTokenSource();
        FetchWithAwaitButton.IsEnabled = false;
        try
        {
            ClearListBox();

            List<Person> people = await reader.GetPeopleAsync(tokenSource.Token);
            foreach (var person in people)
            {
                PersonListBox.Items.Add(person);
            }
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
            tokenSource.Dispose();
            FetchWithAwaitButton.IsEnabled = true;
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