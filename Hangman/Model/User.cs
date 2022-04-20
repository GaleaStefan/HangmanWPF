using System;
using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace Hangman.Model
{
    [Serializable]
    public class User : ObservableObject
    {
        private string _name;
        private string _imageName;
        
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public string ImageName
        {
            get => _imageName;
            set => SetProperty(ref _imageName, value);
        }

        public User(string name, string imageName)
        {
            Name = name;
            ImageName = imageName;
        }

        public User()
        {
            Name = null;
            ImageName = null;
        }
    }
}