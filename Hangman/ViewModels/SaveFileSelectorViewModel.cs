using System.Collections.ObjectModel;
using Hangman.Managers;
using Hangman.Model;
using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace Hangman.ViewModels
{
    public class SaveFileSelectorViewModel : ObservableObject
    {
        private readonly IGameSaveFilesManager _saveFilesManager;
        private readonly User _user;

        private ObservableCollection<string> _saveFiles;
        private string _selectedFile;

        public ObservableCollection<string> SaveFiles
        {
            get => _saveFiles;
            set => SetProperty(ref _saveFiles, value);
        }

        public string SelectedFile
        {
            get => _selectedFile;
            set 
            { 
                SetProperty(ref _selectedFile, value); 
                OnPropertyChanged(nameof(HasFileSelected));
            }
        }

        public bool HasFileSelected => SelectedFile != null;

        public HangmanGame LoadedGame => _saveFilesManager.LoadGame(SelectedFile);

        public SaveFileSelectorViewModel(User user, IGameSaveFilesManager saveFilesManager)
        {
            _user = user;
            _saveFilesManager = saveFilesManager;

            SaveFiles = new ObservableCollection<string>(saveFilesManager.GetSaveFiles(user));
        }
    }
}