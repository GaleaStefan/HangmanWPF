using System.Collections.Generic;
using System.Linq;
using Hangman.Model;
using Hangman.Services;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;

namespace Hangman.ViewModels
{
    public class UserCreateViewModel : ObservableObject
    {
        private User _user;
        private readonly IDialogService _dialogService;
        private readonly IEnumerable<string> _existingNames;

        public RelayCommand OpenImageCommand { get; }

        public bool UserNameExists => _existingNames.Any(name => name.ToLower().Equals(_user.Name.ToLower()));
        public User User
        {
            get => _user;
            set => SetProperty(ref _user, value);
        }

        public UserCreateViewModel(IEnumerable<string> existingUserNames,IDialogService dialogService)
        {
            _dialogService = dialogService;
            _existingNames = existingUserNames;

            OpenImageCommand = new RelayCommand(OpenImage);
            User = new User();
        }

        private void OpenImage()
        {
            User.ImageName = _dialogService.RunOpenFileDialog("Image Files(*.BMP;*.JPG;*.GIF,*.PNG)|*.BMP;*.JPG;*.GIF;*.PNG", null, false)?.First() ?? "default.png";
        }
    }
}