using System.Windows;
using Hangman.ViewModels;

namespace Hangman.Views
{
    public partial class UserCreateDialog : Window
    {
        public UserCreateDialog()
        {
            InitializeComponent();
        }

        private void CreateButton_OnClick(object sender, RoutedEventArgs e)
        {
            var userNameExists = (DataContext as UserCreateViewModel)?.UserNameExists ?? true;
            
            if(userNameExists)
                return;
            
            DialogResult = true;
            Close();
        }
    }
}