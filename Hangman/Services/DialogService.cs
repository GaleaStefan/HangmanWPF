using System.Collections.Generic;
using System.Windows;
using Hangman.Model;
using Hangman.ViewModels;
using Hangman.Views;
using Microsoft.Win32;

namespace Hangman.Services
{
    public interface IDialogService
    {
        bool? AskSave(string dialogText, string dialogTitle);
        IEnumerable<string> RunOpenFileDialog(string filter, string initialDirectory, bool multipleFiles);

        string? RunSaveFileDialog(string filter, string initialDirectory);

        User ShowUserCreationDialog(IEnumerable<string> existingUserNames);
        string? ShowCategorySelectorDialog();
        HangmanGame ShowGameLoadDialog(User user);
    }
    
    public class DialogService : IDialogService
    {
        private readonly IUserCreateViewModelFactory _userCreateViewModelFactory;
        private readonly ICategorySelectorViewModelFactory _categorySelectorViewModelFactory;
        private readonly ISaveFileSelectorViewModelFactory _saveFileSelectorViewModelFactory;
        
        public DialogService(IUserCreateViewModelFactory userCreateViewModelFactory, 
            ICategorySelectorViewModelFactory categorySelectorViewModelFactory, ISaveFileSelectorViewModelFactory saveFileSelectorViewModelFactory)
        {
            _userCreateViewModelFactory = userCreateViewModelFactory;
            _categorySelectorViewModelFactory = categorySelectorViewModelFactory;
            _saveFileSelectorViewModelFactory = saveFileSelectorViewModelFactory;
        }
        
        public bool? AskSave(string dialogText, string dialogTitle)
        {
            var messageboxResult = MessageBox.Show(Application.Current.MainWindow, dialogText, dialogTitle,
                MessageBoxButton.YesNoCancel);
            return messageboxResult switch
            {
                MessageBoxResult.Yes => true,
                MessageBoxResult.No => false,
                _ => null
            };
        }

        public IEnumerable<string> RunOpenFileDialog(string filter, string initialDirectory, bool multipleFiles)
        {
            var dialog = new OpenFileDialog
            {
                Filter = filter,
                InitialDirectory = initialDirectory,
                Multiselect = multipleFiles
            };

            var dialogResponse = dialog.ShowDialog();

            if (dialogResponse is not true)
                return null;

            return multipleFiles ? dialog.FileNames : new List<string> {dialog.FileName};
        }

        public string? RunSaveFileDialog(string filter, string initialDirectory)
        {
            var dialog = new SaveFileDialog
            {
                Filter = filter,
                InitialDirectory = initialDirectory
            };
            return dialog.ShowDialog() is true ? dialog.FileName : null;
        }

        public User ShowUserCreationDialog(IEnumerable<string> existingUserNames)
        {
            var dialogViewModel = _userCreateViewModelFactory.Create(existingUserNames);
            var dialog = new UserCreateDialog
            {
                DataContext = dialogViewModel
            };

            return dialog.ShowDialog() is true ? dialogViewModel.User : null;
        }

        public string? ShowCategorySelectorDialog()
        {
            var dialogVm = _categorySelectorViewModelFactory.Create();
            var dialog = new CategorySelectorDialog
            {
                DataContext = dialogVm
            };

            return dialog.ShowDialog() is not true ? null : dialogVm.SelectedCategory;
        }

        public HangmanGame ShowGameLoadDialog(User user)
        {
            var dialogVm = _saveFileSelectorViewModelFactory.Create(user);
            var dialog = new SaveFileSelectorDialog()
            {
                DataContext = dialogVm
            };

            return dialog.ShowDialog() is not true ? null : dialogVm.LoadedGame;
        }
    }
}