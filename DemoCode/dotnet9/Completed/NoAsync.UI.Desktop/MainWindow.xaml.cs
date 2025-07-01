using NoAsync.Library;
using System.Windows;
using TaskAwait.Shared;

namespace NoAsync.UI.Desktop;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void HardCodedButton_Click(object sender, RoutedEventArgs e)
    {
        ClearListBox();
        IPersonReader reader = new HardCodedPersonReader();
        FetchData(reader);
    }

    private void AsyncLibraryButton_Click(object sender, RoutedEventArgs e)
    {
        ClearListBox();

        IPersonReader reader = new APIReader();
        FetchData(reader);
    }

    private async void AsyncConstructorButton_Click(object sender, RoutedEventArgs e)
    {
        ClearListBox();

        IPersonReader reader = await AsyncConstructorReader.CreateReaderAsync();
        FetchData(reader);
    }

    private void FetchData(IPersonReader reader)
    {
        try
        {
            List<Person> people = reader.GetPeople();
            foreach (var person in people)
            {
                PersonListBox.Items.Add(person);
            }

        }
        catch (Exception ex)
        {
            MessagePopup.ShowMessage("ERROR", $"{ex.GetType()}\n{ex.Message}");
        }
    }

    private void ClearButton_Click(object sender, RoutedEventArgs e)
    {
        ClearListBox();
    }

    private void ClearListBox()
    {
        PersonListBox.Items.Clear();
    }
}