using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Hangman.Managers;
using Hangman.Model;
using Hangman.Services;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;

namespace Hangman.ViewModels
{
    public class UserSelectorViewModel : ObservableObject
    {
        public event Action<User> UserChosenEvent;
        private readonly IUserManager _userManager;
        private readonly IDialogService _dialogService;

        private ObservableCollection<User> _allUsers;
        private User _selectedUser;

        public ObservableCollection<User> AllUsers
        {
            get => _allUsers;
            set => SetProperty(ref _allUsers, value);
        }

        public User SelectedUser
        {
            get => _selectedUser;
            set
            {
                SetProperty(ref _selectedUser, value); 
                DeleteUserCommand.NotifyCanExecuteChanged();
                UserChosenCommand.NotifyCanExecuteChanged();
            }
        }

        public RelayCommand NewUserCommand { get; }
        public RelayCommand DeleteUserCommand { get; }
        public RelayCommand UserChosenCommand { get; }

        public UserSelectorViewModel(IUserManager userManager, IDialogService dialogService)
        {
            _userManager = userManager;
            _dialogService = dialogService;

            _allUsers = new ObservableCollection<User>(_userManager.GetAllUsers().ToList());

            NewUserCommand = new RelayCommand(CreateUser);
            DeleteUserCommand = new RelayCommand(DeleteUser, () => SelectedUser is not null);
            UserChosenCommand = new RelayCommand(() => UserChosenEvent?.Invoke(SelectedUser),
                () => SelectedUser is not null);
        }

        private void DeleteUser()
        {
            _userManager.DeleteUser(SelectedUser);
            AllUsers.Remove(SelectedUser);
            SelectedUser = null;
        }

        private void CreateUser()
        {
            var newUser = _userManager.CreateUser();
            if(newUser is null)
                return;
            
            _allUsers.Add(newUser);
        }
    }
}