using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Threading;
using System.Xml.Serialization;
using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace Hangman.Model
{
    [Serializable]
    public class HangmanLevel : ObservableObject
    {
        private const int RoundTimeInSeconds = 120;
        private const int MaximumMistakes = 5;
        private const char EmptyDisplay = '_';
        
        public event Action<HangmanLevelResult> LevelEndEvent;

        private int _mistakes;
        private string _correctWord;
        private char[] _currentWord;
        private readonly DispatcherTimer _roundTimer;
        private int _remainingSeconds;
        private HashSet<char> _guesses;
        
        public HashSet<char> Guesses
        {
            get => _guesses;
            set => SetProperty(ref _guesses, value);
        }
        
        public string CorrectWord
        {
            get => _correctWord;
            set => SetProperty(ref _correctWord, value);
        }

        public char[] CurrentWord
        {
            get => _currentWord;
            set => SetProperty(ref _currentWord, value);
        }

        public int Mistakes
        {
            get => _mistakes;
            set => SetProperty(ref _mistakes, value);
        }

        public int RemainingSeconds
        {
            get => _remainingSeconds;
            set => SetProperty(ref _remainingSeconds, value);
        }

        [XmlIgnore]
        public char[] DisplayWord => _currentWord.Select(c => c == '\0' ? EmptyDisplay : char.ToUpper(c)).ToArray();

        public HangmanLevel(string word)
        {
            _guesses = new HashSet<char>();
            _correctWord = word.ToLower();
            _currentWord = _correctWord.Select(c => char.IsLetter(c) ? '\0' : c).ToArray();
            Mistakes = 0;
            RemainingSeconds = RoundTimeInSeconds;
            _roundTimer = new DispatcherTimer
            {
                Interval = new TimeSpan(0, 0, 1)
            };
            _roundTimer.Tick += OnTimerTick;
        }

        public HangmanLevel()
        {
            _roundTimer = new DispatcherTimer
            {
                Interval = new TimeSpan(0, 0, 1)
            };
            _roundTimer.Tick += OnTimerTick;
        }

        public void Start()
        {
            _roundTimer.Start();
        }

        public void Guess(char letter)
        {
            if (!char.IsLetter(letter))
                return;

            var guess = char.ToLower(letter);
            if (_guesses.Contains(guess))
                return;

            _guesses.Add(guess);

            var indices = _correctWord.AllIndicesOf(guess).ToList();
            if (!indices.Any())
            {
                OnWrongGuess();
            }
            
            OnCorrectGuess(guess, indices);
        }

        private void OnCorrectGuess(char guess, IEnumerable<int> indices)
        {
            foreach (var index in indices)
            {
                _currentWord[index] = guess;
            }
            OnPropertyChanged(nameof(DisplayWord));
            
            if(IsCurrentWordCorrect())
                EndGame();
        }

        private void OnWrongGuess()
        {
            Mistakes++;
            
            if (Mistakes == MaximumMistakes)
            {
                EndGame();
            }
        }

        private void OnTimerTick(object sender, EventArgs args)
        {
            if(sender is not DispatcherTimer timer)
                return;
            
            if(timer != _roundTimer)
                return;

            RemainingSeconds -= Convert.ToInt32(timer.Interval.TotalSeconds);

            if (RemainingSeconds <= 0) 
                EndGame();
        }

        private bool IsCurrentWordCorrect() => !_correctWord.Where((t, i) => t != _currentWord[i]).Any();

        private void EndGame()
        {
            _roundTimer.Stop();
            var hasWon = _remainingSeconds > 0 && IsCurrentWordCorrect();
            var levelResult = new HangmanLevelResult(hasWon, Mistakes, RoundTimeInSeconds - RemainingSeconds, _correctWord);
            LevelEndEvent?.Invoke(levelResult);
        }
    }
}