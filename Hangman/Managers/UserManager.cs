using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Hangman.Model;
using Hangman.Serialization;
using Hangman.Services;

namespace Hangman.Managers
{
    public interface IUserManager
    {
        IEnumerable<User> GetAllUsers();
        void AddUser(User user);
        User CreateUser();
        void DeleteUser(User user);
    }

    public class UserManager : IUserManager
    {
        private const string UsersSavePath = @"\Users\";
        private static readonly string UsersAbsolutePath;

        private readonly IUserXmlSerializer _userXmlSerializer;
        private readonly IDialogService _dialogService;

        static UserManager()
        {
            UsersAbsolutePath = Directory.GetParent(Assembly.GetExecutingAssembly().Location) + UsersSavePath;
            
            if (!Directory.Exists(UsersAbsolutePath))
                Directory.CreateDirectory(UsersAbsolutePath);
        }

        public UserManager(IUserXmlSerializer userXmlSerializer, IDialogService dialogService)
        {
            _userXmlSerializer = userXmlSerializer;
            _dialogService = dialogService;

            Directory.CreateDirectory(UsersAbsolutePath);
        }

        public void AddUser(User user)
        {
            var fileInfo = new FileInfo(user.ImageName);
            var newImagePath = user.ImageName;
            
            if (fileInfo.Directory?.FullName != UsersAbsolutePath)
            {
                newImagePath = UsersAbsolutePath + fileInfo.Name;
                File.Copy(user.ImageName, newImagePath);
            }

            user.ImageName = newImagePath;
            _userXmlSerializer.ToXml(user, UsersAbsolutePath + user.Name + IUserXmlSerializer.FileExtension);
        }

        public void DeleteUser(User user)
        {
            File.Delete(UsersAbsolutePath + user.Name + IUserXmlSerializer.FileExtension);
            File.Delete(user.ImageName);
        }

        public IEnumerable<User> GetAllUsers() 
            => Directory.GetFiles(UsersAbsolutePath, "*" + IUserXmlSerializer.FileExtension)
                .Select(path => _userXmlSerializer.FromXml(path));

        public User CreateUser()
        {
            var user = _dialogService.ShowUserCreationDialog(GetAllUsers().Select(user => user.Name));
            
            if(user is not null)
                AddUser(user);

            return user;
        }
    }
}