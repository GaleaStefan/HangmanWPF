using System;
using System.Collections.Generic;
using System.Linq;
using Hangman.Model;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;

namespace Hangman.ViewModels
{
    public class MainViewModel : ObservableObject
    {
        private ObservableObject _currentViewModel;

        public ObservableObject CurrentViewModel
        {
            get => _currentViewModel;
            set => SetProperty(ref _currentViewModel, value);
        }

        private readonly IUserSelectorViewModelFactory _userSelectorViewModelFactory;
        private readonly IGameStartViewModelFactory _gameStartViewModelFactory;
        private readonly IHangmanGameViewModelFactory _hangmanGameViewModelFactory;
        private readonly IStatisticsViewModelFactory _statisticsViewModelFactory;

        public MainViewModel(IUserSelectorViewModelFactory userSelectorViewModelFactory, 
            IGameStartViewModelFactory gameStartViewModelFactory, 
            IHangmanGameViewModelFactory hangmanGameViewModelFactory, IStatisticsViewModelFactory statisticsViewModelFactory)
        {
            _userSelectorViewModelFactory = userSelectorViewModelFactory;
            _gameStartViewModelFactory = gameStartViewModelFactory;
            _hangmanGameViewModelFactory = hangmanGameViewModelFactory;
            _statisticsViewModelFactory = statisticsViewModelFactory;
            SwitchToUserSelection();
        }

        private void SwitchToUserSelection()
        {
            var userSelectorViewModel = _userSelectorViewModelFactory.Create();
            userSelectorViewModel.UserChosenEvent += SwitchToGameStart;
            CurrentViewModel = userSelectorViewModel;
        }

        private void SwitchToGameStart(User user)
        {
            var startVm = _gameStartViewModelFactory.Create(user);
            startVm.NewGameSelectedEvent += CreateNewGame;
            startVm.GameLoadedEvent += LoadGame;
            startVm.SwitchToStatsRequestedEvent += SwitchToStatistics;
            CurrentViewModel = startVm;
        }

        private void LoadGame(HangmanGame game, User user)
        {
            var gameVm = _hangmanGameViewModelFactory.Create(user, game);
            gameVm.AbortEvent += SwitchToGameStart;
            gameVm.SwitchUserEvent += SwitchToUserSelection;
            gameVm.GameFinishedEvent += SwitchToStatistics;
            CurrentViewModel = gameVm;
        }

        private void CreateNewGame(User user, IEnumerable<string> possibleWords, string category)
        {
            var gameVm = _hangmanGameViewModelFactory.Create(user, SelectRandomWords(possibleWords), category);
            gameVm.AbortEvent += SwitchToGameStart;
            gameVm.SwitchUserEvent += SwitchToUserSelection;
            gameVm.GameFinishedEvent += SwitchToStatistics;
            CurrentViewModel = gameVm;
        }

        private void SwitchToStatistics(User user)
        {
            var statsVm = _statisticsViewModelFactory.Create(user);
            statsVm.BackToMenuRequestedEvent += SwitchToGameStart;
            statsVm.SwitchUserRequestedEvent += SwitchToUserSelection;
            CurrentViewModel = statsVm;
        }

        private IEnumerable<string> SelectRandomWords(IEnumerable<string> dataSet) 
            => dataSet.OrderBy(_ => Guid.NewGuid()).Take(HangmanGame.RequiredLevelsToWin);
    }
}