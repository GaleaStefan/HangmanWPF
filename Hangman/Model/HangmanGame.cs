using System;
using System.Collections.Generic;
using System.Windows.Input;
using System.Xml.Serialization;
using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace Hangman.Model
{
    [Serializable]
    public class HangmanGame : ObservableObject
    {
        public const int RequiredLevelsToWin = 5;

        public event Action<HangmanGameResult> GameEndEvent;
        public event Action LevelEndEvent;

        private int _wonLevels;
        private List<string> _words;
        private HangmanLevel _currentLevel;
        private string _category;

        public string Category
        {
            get => _category;
            set => SetProperty(ref _category, value);
        }

        public List<string> Words
        {
            get => _words;
            set => SetProperty(ref _words, value);
        }
        
        public List<HangmanLevelResult> LevelResults { get; set; }

        public HangmanLevel CurrentLevel
        {
            get => _currentLevel;
            set
            {
                SetProperty(ref _currentLevel, value);
                _currentLevel.LevelEndEvent += OnLevelEnd;
            }
        }

        public int WonLevels
        {
            get => _wonLevels;
            set { 
                SetProperty(ref _wonLevels, value); 
                OnPropertyChanged(nameof(CurrentLevelCount)); 
            }
        }

        [XmlIgnore]
        public int CurrentLevelCount => WonLevels + 1;

        public HangmanGame(List<string> words, string category)
        {
            _words = words;
            _category = category;
            WonLevels = 0;
            LevelResults = new List<HangmanLevelResult>();
            CurrentLevel = new HangmanLevel(_words[_wonLevels]);
        }

        public HangmanGame()
        {
            
        }

        public void StartGame()
        {
            CurrentLevel.Start();
        }

        private void OnLevelEnd(HangmanLevelResult result)
        {
            LevelEndEvent?.Invoke();
            LevelResults.Add(result);

            if (!result.HasWon)
            {
                GameEndEvent?.Invoke(new HangmanGameResult(LevelResults, _category, _wonLevels, false));
                return;
            }
            
            OnLevelWon();
        }
        private void OnLevelWon()
        {
            WonLevels++;

            if (WonLevels == RequiredLevelsToWin)
            {
                GameEndEvent?.Invoke(new HangmanGameResult(LevelResults, _category, _wonLevels, true));
                return;
            }
            
            SwitchToNextLevel();
        }

        private void SwitchToNextLevel()
        {
            CurrentLevel = new HangmanLevel(_words[WonLevels]);
            CurrentLevel.Start();
        }
    }
}