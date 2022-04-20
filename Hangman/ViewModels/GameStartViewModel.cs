using System;
using System.Collections.Generic;
using Hangman.Managers;
using Hangman.Model;
using Hangman.Services;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;

namespace Hangman.ViewModels
{
    public class GameStartViewModel : ObservableObject
    {
        public event Action<User, IEnumerable<string>, string> NewGameSelectedEvent;
        public event Action<HangmanGame, User> GameLoadedEvent;
        public event Action<User> SwitchToStatsRequestedEvent;

        private User _user;
        
        public User User
        {
            get => _user;
            set => SetProperty(ref _user, value);
        }
        
        public RelayCommand NewGameCommand { get; }
        public RelayCommand LoadGameCommand { get; }
        public RelayCommand GoToStatsCommand { get; }

        private readonly IDialogService _dialogService;
        private readonly ICategoryManager _categoryManager;

        public GameStartViewModel(User user, IDialogService dialogService, ICategoryManager categoryManager)
        {
            User = user;
            _dialogService = dialogService;
            _categoryManager = categoryManager;

            NewGameCommand = new RelayCommand(CreateNewGame);
            LoadGameCommand = new RelayCommand(LoadGame);
            GoToStatsCommand = new RelayCommand(() => SwitchToStatsRequestedEvent?.Invoke(User));
        }

        private void LoadGame()
        {
            var game = _dialogService.ShowGameLoadDialog(_user);
            if(game is null)
                return;
            
            GameLoadedEvent?.Invoke(game, _user);
        }

        private void CreateNewGame()
        {
            var category = _dialogService.ShowCategorySelectorDialog();
            if(category is null)
                return;

            var allWords = _categoryManager.GetWordsInCategory(category);
            NewGameSelectedEvent?.Invoke(User, allWords, category);
        }
    }
}