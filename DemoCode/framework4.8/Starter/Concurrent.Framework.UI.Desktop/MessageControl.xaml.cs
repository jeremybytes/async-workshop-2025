using System.Windows;
using System.Windows.Controls;

namespace Concurrent.Framework.UI.Desktop
{
    public partial class MessageControl : UserControl
    {
        public MessageControl()
        {
            InitializeComponent();
            Loaded += MessageControl_Loaded;
        }

        private void MessageControl_Loaded(object sender, RoutedEventArgs e)
        {
            MessageContainer.Visibility = Visibility.Hidden;
            PopupCaption.Text = "";
            PopupText.Text = "";
        }

        public void ShowMessage(string caption, string message)
        {
            PopupCaption.Text = caption;
            PopupText.Text = message;
            MessageContainer.Visibility = Visibility.Visible;
            PopupClose.Focus();
        }

        public void HideMessage()
        {
            PopupCaption.Text = null;
            PopupText.Text = null;
            MessageContainer.Visibility = Visibility.Hidden;
        }

        private void PopupClose_Click(object sender, RoutedEventArgs e)
        {
            HideMessage();
        }
    }
}