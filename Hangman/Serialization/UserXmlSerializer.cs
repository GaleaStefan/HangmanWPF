using System;
using System.IO;
using System.Xml.Serialization;
using Hangman.Model;

namespace Hangman.Serialization
{
    public interface IUserXmlSerializer
    {
        const string FileExtension = ".user";
        void ToXml(User user, string path);
        User FromXml(string path);
    }

    public class UserXmlSerializer : IUserXmlSerializer
    {
        private readonly XmlSerializer _serializer;

        public UserXmlSerializer()
        {
            _serializer = new XmlSerializer(typeof(User));
        }

        public void ToXml(User user, string path)
        {
            using var file = new StreamWriter(path);
            _serializer.Serialize(file, user);
        }
        
        public User FromXml(string path)
        {
            using var file = new StreamReader(path);
            try
            {
                return _serializer.Deserialize(file) as User;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}