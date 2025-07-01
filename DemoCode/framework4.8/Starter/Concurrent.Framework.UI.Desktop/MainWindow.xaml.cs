using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using TaskAwait.Framework.Library;
using TaskAwait.Framework.Shared;

namespace Concurrent.Framework.UI.Desktop
{
    public partial class MainWindow : Window
    {
        private PersonReader reader = new PersonReader();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void FetchWithTaskButton_Click(object sender, RoutedEventArgs e)
        {
            ClearListBox();

        }

        private void FetchWithAwaitButton_Click(object sender, RoutedEventArgs e)
        {
            ClearListBox();

        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ClearListBox()
        {
            PersonListBox.Items.Clear();
        }
    }
}
