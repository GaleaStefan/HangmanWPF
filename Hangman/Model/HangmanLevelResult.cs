using System;

namespace Hangman.Model
{
    [Serializable]
    public struct HangmanLevelResult
    {
        public HangmanLevelResult(bool hasWon, int mistakes, int remainingSeconds, string initialWord)
        {
            HasWon = hasWon;
            Mistakes = mistakes;
            RemainingSeconds = remainingSeconds;
            InitialWord = initialWord;
        }

        public bool HasWon { get; set; }
        public int Mistakes { get; set; }
        public int RemainingSeconds { get; set; }
        public string InitialWord { get; set; }
    }
}