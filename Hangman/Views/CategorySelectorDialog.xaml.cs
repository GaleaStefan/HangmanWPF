using System;
using System.Windows;

namespace Hangman.Views
{
    public partial class CategorySelectorDialog : Window
    {
        public CategorySelectorDialog()
        {
            InitializeComponent();
        }

        private void SelectButton_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}