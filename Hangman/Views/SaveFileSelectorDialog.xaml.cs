using System.Windows;

namespace Hangman.Views
{
    public partial class SaveFileSelectorDialog : Window
    {
        public SaveFileSelectorDialog()
        {
            InitializeComponent();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}