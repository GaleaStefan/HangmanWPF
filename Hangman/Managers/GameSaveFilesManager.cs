using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Hangman.Model;
using Hangman.Serialization;

namespace Hangman.Managers
{
    public interface IGameSaveFilesManager
    {
        void SaveGame(HangmanGame game, User player);
        IEnumerable<string> GetSaveFiles(User player);
        HangmanGame LoadGame(string path);
    }

    public class GameSaveFilesManager : IGameSaveFilesManager
    {
        private const string SaveFileExtension = ".savefile";
        private const string SaveFilePath = @"\Saves\";
        private static readonly string SaveFileAbsolutePath;
        
        static GameSaveFilesManager()
        {
            SaveFileAbsolutePath = Directory.GetParent(Assembly.GetExecutingAssembly().Location) + SaveFilePath;
            
            if (!Directory.Exists(SaveFileAbsolutePath))
                Directory.CreateDirectory(SaveFileAbsolutePath);
        }
        
        private readonly IHangmanGameSerializer _gameSerializer;

        public GameSaveFilesManager(IHangmanGameSerializer gameSerializer)
        {
            _gameSerializer = gameSerializer;
        }

        public void SaveGame(HangmanGame game, User player)
        {
            _gameSerializer.ToXml(game, SaveFileAbsolutePath + player.Name + '\\' + DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss") + SaveFileExtension);
        }

        public IEnumerable<string> GetSaveFiles(User player)
        {
            try
            {
                return Directory.GetFiles(SaveFileAbsolutePath + player.Name + '\\');
            }
            catch (Exception)
            {
                return new List<string>();
            }
        }

        public HangmanGame LoadGame(string path)
        {
            return _gameSerializer.FromXml(path);
        }
    }
}