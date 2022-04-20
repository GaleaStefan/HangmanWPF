using System.Collections.Generic;
using Hangman.Model;

namespace Hangman.ViewModels
{
    public interface IUserSelectorViewModelFactory
    {
        UserSelectorViewModel Create();
    }

    public interface IUserCreateViewModelFactory
    {
        UserCreateViewModel Create(IEnumerable<string> existingUserNames);
    }

    public interface IGameStartViewModelFactory
    {
        GameStartViewModel Create(User user);
    }

    public interface ICategorySelectorViewModelFactory
    {
        CategorySelectorViewModel Create();
    }

    public interface IHangmanGameViewModelFactory
    {
        HangmanGameViewModel Create(User user, IEnumerable<string> words, string category);
        HangmanGameViewModel Create(User user, HangmanGame game);
    }

    public interface ISaveFileSelectorViewModelFactory
    {
        SaveFileSelectorViewModel Create(User user);
    }

    public interface IStatisticsViewModelFactory
    {
        StatisticsViewModel Create(User user);
    }
}