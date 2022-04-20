using System;
using System.Collections.Generic;

namespace Hangman.Model
{
    [Serializable]
    public struct HangmanGameResult
    {
        public List<HangmanLevelResult> LevelResults { get; set; }
        public string Category { get; set; }
        public int Wins { get; set; }
        
        public bool GameWon { get; set; }
        
        public HangmanGameResult(List<HangmanLevelResult> levelResults, string category, int wins, bool gameWon)
        {
            LevelResults = levelResults;
            Category = category;
            Wins = wins;
            GameWon = gameWon;
        }
    }
}