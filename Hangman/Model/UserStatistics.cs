using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace Hangman.Model
{
    public class PerCategoryStatistics
    {
        public string Category { get; set; }
        public int TotalGames { get; set; }
        public int Wins { get; set; }
    }
    
    public class UserStatistics
    {
        public string Name { get; set; }

        public Dictionary<string, PerCategoryStatistics> PerCategoryGames { get; set; }
        
    }
}