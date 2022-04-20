using System;
using System.IO;
using System.Xml.Serialization;
using Hangman.Model;

namespace Hangman.Serialization
{
    public interface IHangmanGameSerializer
    {
        void ToXml(HangmanGame game, string path);
        HangmanGame FromXml(string path);
    }
    
    public class HangmanGameSerializer : IHangmanGameSerializer
    {
        private readonly XmlSerializer _serializer;

        public HangmanGameSerializer()
        {
            _serializer = new XmlSerializer(typeof(HangmanGame));
        }

        public void ToXml(HangmanGame game, string path)
        {
            var fileInfo = new FileInfo(path);
            Directory.CreateDirectory(fileInfo.Directory.FullName);
            using var file = new StreamWriter(path);
            _serializer.Serialize(file, game);
        }
        
        public HangmanGame FromXml(string path)
        {
            using var file = new StreamReader(path);
            try
            {
                return _serializer.Deserialize(file) as HangmanGame;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}