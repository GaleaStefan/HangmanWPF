using System;
using System.Collections.ObjectModel;
using Hangman.Managers;
using Hangman.Model;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;

namespace Hangman.ViewModels
{
    public class StatisticsViewModel : ObservableObject
    {
        public event Action<User> BackToMenuRequestedEvent;
        public event Action SwitchUserRequestedEvent;
        
        private ObservableCollection<UserStatistics> _userStatistics;
        private readonly User _user;

        public RelayCommand BackToMenuCommand => new(() => BackToMenuRequestedEvent?.Invoke(_user));
        public RelayCommand SwitchUserCommand => new(() => SwitchUserRequestedEvent?.Invoke());

        public ObservableCollection<UserStatistics> UserStatistics
        {
            get => _userStatistics;
            set => SetProperty(ref _userStatistics, value);
        }

        public StatisticsViewModel(User user, IStatisticsManager statisticsManager)
        {
            _user = user;
            UserStatistics = new ObservableCollection<UserStatistics>(statisticsManager.GetAllUsersStatistics());
        }
        
        
    }
}