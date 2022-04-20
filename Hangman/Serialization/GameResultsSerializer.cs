using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using Hangman.Model;

namespace Hangman.Serialization
{
    public interface IGameResultsSerializer
    {
        void AppendGameResult(HangmanGameResult result, string path);
        IEnumerable<HangmanGameResult> FromXml(string path);
    }
    
    public class GameResultsSerializer : IGameResultsSerializer
    {
        [Serializable]
        public struct Games
        {
            public List<HangmanGameResult> Results { get; set; }
        }
        
        private void CreateGameResultsFile(HangmanGameResult result, string path)
        {
            using var file = new StreamWriter(path);
            var serializer = new XmlSerializer(typeof(Games));
            serializer.Serialize(file, new Games {Results = new List<HangmanGameResult> {result}});
        }
        
        public void AppendGameResult(HangmanGameResult result, string path)
        {
            if (!File.Exists(path))
            {
                CreateGameResultsFile(result, path);
                return;
            }
            
            var xmlDocument = new XmlDocument();
            xmlDocument.Load(path);
            var resultsNode = xmlDocument.GetElementsByTagName("Results")[0];
            var navigator = resultsNode.CreateNavigator();
            var emptyNs = new XmlSerializerNamespaces(new[] {XmlQualifiedName.Empty});
            using var writer = navigator.AppendChild();
            var serializer = new XmlSerializer(result.GetType());
            writer.WriteWhitespace("");
            serializer.Serialize(writer, result, emptyNs);
            writer.Close();
            xmlDocument.Save(path);
        }
        
        public IEnumerable<HangmanGameResult> FromXml(string path)
        {
            try
            {
                using var file = new StreamReader(path);
                var deserializer = new XmlSerializer(typeof(Games));
                return ((Games) deserializer.Deserialize(file)).Results;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}