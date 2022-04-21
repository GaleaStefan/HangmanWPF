using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Hangman.Model;
using Hangman.Serialization;

namespace Hangman.Managers
{
    public interface IStatisticsManager
    {
        void ReportUserGame(User user, HangmanGameResult gameResult);
        UserStatistics GetUserStatistics(User user);
        IEnumerable<UserStatistics> GetAllUsersStatistics();
    }

    public class StatisticsManager : IStatisticsManager
    {
        public const string StatisticsFileExtension = ".stats";
        private const string StatisticsPath = @"\Statistics\";
        public static readonly string StatisticsAbsolutePath;
        
        static StatisticsManager()
        {
            StatisticsAbsolutePath = Directory.GetParent(Assembly.GetExecutingAssembly().Location) + StatisticsPath;

            if (!Directory.Exists(StatisticsAbsolutePath))
                Directory.CreateDirectory(StatisticsAbsolutePath);
        }

        private readonly IGameResultsSerializer _gameResultsSerializer;
        private readonly IUserManager _userManager;
        
        public StatisticsManager(IGameResultsSerializer gameResultsSerializer, IUserManager userManager)
        {
            _gameResultsSerializer = gameResultsSerializer;
            _userManager = userManager;
        }

        public void ReportUserGame(User user, HangmanGameResult gameResult)
        {
            _gameResultsSerializer.AppendGameResult(gameResult, StatisticsAbsolutePath + user.Name + StatisticsFileExtension);
        }

        private Dictionary<string, PerCategoryStatistics> GetPerCategoryStats(IEnumerable<HangmanGameResult> data)
        {
            return data.GroupBy(result => result.Category)
                .ToDictionary(g => g.Key, g => CountTotalAndWins(g.ToList()));
        }

        private PerCategoryStatistics CountTotalAndWins(ICollection<HangmanGameResult> data)
        {
            return new PerCategoryStatistics
            {
                Category = data.First().Category, TotalGames = data.Count, Wins = data.Count(result => result.GameWon)
            };
        }

        public UserStatistics GetUserStatistics(User user)
        {
            var gamesReport = _gameResultsSerializer
                .FromXml(StatisticsAbsolutePath + user.Name + StatisticsFileExtension)
                ?.ToList();
            return gamesReport is null ? null : new UserStatistics {Name = user.Name, PerCategoryGames = GetPerCategoryStats(gamesReport)};
        }

        public IEnumerable<UserStatistics> GetAllUsersStatistics()
        {
            return _userManager.GetAllUsers().Select(GetUserStatistics).Where(stat => stat != null);
        }
    }
}