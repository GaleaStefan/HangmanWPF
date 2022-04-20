using System;
using System.Collections.Generic;
using System.Linq;
using Hangman.Managers;
using Hangman.Model;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;

namespace Hangman.ViewModels
{
    public class HangmanGameViewModel : ObservableObject
    {
        public event Action<User> AbortEvent;
        public event Action SwitchUserEvent;
        public event Action<User> GameFinishedEvent;
        
        private User _user;
        private HangmanGame _hangmanGame;
        private KeyboardViewModel _keyboard;
        private readonly IGameSaveFilesManager _saveFilesManager;
        private readonly IStatisticsManager _statisticsManager;

        public KeyboardViewModel Keyboard
        {
            get => _keyboard;
            set => SetProperty(ref _keyboard, value);
        }

        public HangmanGame HangmanGame
        {
            get => _hangmanGame;
            set => SetProperty(ref _hangmanGame, value);
        }

        public User User
        {
            get => _user;
            set => SetProperty(ref _user, value);
        }
        
        public RelayCommand SaveGameCommand { get; }
        public RelayCommand AbortGameCommand { get; }
        public RelayCommand SwitchUserCommand { get; }

        public HangmanGameViewModel(User user, IEnumerable<string> words, string category, IGameSaveFilesManager saveFilesManager, IStatisticsManager statisticsManager) :
            this(user, new HangmanGame(words.ToList(), category), saveFilesManager, statisticsManager)
        {
        }

        public HangmanGameViewModel(User user, HangmanGame game, IGameSaveFilesManager saveFilesManager, IStatisticsManager statisticsManager)
        {
            User = user;
            HangmanGame = game;
            _saveFilesManager = saveFilesManager;
            _statisticsManager = statisticsManager;
            HangmanGame.LevelEndEvent += OnLevelEnd;
            HangmanGame.GameEndEvent += OnGameEnd;
            HangmanGame.StartGame();

            Keyboard = new KeyboardViewModel();
            Keyboard.KeyPressedEvent += c => HangmanGame.CurrentLevel.Guess(c);

            SaveGameCommand = new RelayCommand(SaveGame);
            AbortGameCommand = new RelayCommand(() => AbortEvent?.Invoke(user));
            SwitchUserCommand = new RelayCommand(() => SwitchUserEvent?.Invoke());
        }

        private void OnGameEnd(HangmanGameResult result)
        {
            _statisticsManager.ReportUserGame(User, result);
            GameFinishedEvent?.Invoke(_user);
        }

        private void SaveGame()
        {
            _saveFilesManager.SaveGame(HangmanGame, User);
        }

        private void OnLevelEnd()
        {
            Keyboard.ResetKeyboard();
        }
    }
}