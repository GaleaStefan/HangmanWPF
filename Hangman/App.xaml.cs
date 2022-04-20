using System.Windows;
using Hangman.Managers;
using Hangman.Serialization;
using Hangman.Services;
using Hangman.ViewModels;
using Hangman.Views;
using Microsoft.Toolkit.Mvvm.Input;
using Ninject;
using Ninject.Extensions.Factory;

namespace Hangman
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static RelayCommand ExitAppCommand { get; private set; }
        
        private IKernel _kernel;
        
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            ConfigureContainer();
            ComposeObjects();
            Current.MainWindow?.Show();
            ExitAppCommand = new RelayCommand(() => Current.Shutdown());
        }

        private void ConfigureContainer()
        {
            _kernel = new StandardKernel();
            _kernel.Bind<IUserXmlSerializer>().To<UserXmlSerializer>().InSingletonScope();
            _kernel.Bind<IHangmanGameSerializer>().To<HangmanGameSerializer>().InSingletonScope();
            _kernel.Bind<IGameResultsSerializer>().To<GameResultsSerializer>().InSingletonScope();
            _kernel.Bind<IUserManager>().To<UserManager>().InSingletonScope();
            _kernel.Bind<IDialogService>().To<DialogService>().InSingletonScope();
            _kernel.Bind<ICategoryManager>().To<CategoryManager>().InSingletonScope();
            _kernel.Bind<IGameSaveFilesManager>().To<GameSaveFilesManager>().InSingletonScope();
            _kernel.Bind<IStatisticsManager>().To<StatisticsManager>().InSingletonScope();
            
            _kernel.Bind<IUserCreateViewModelFactory>().ToFactory();
            _kernel.Bind<IUserSelectorViewModelFactory>().ToFactory();
            _kernel.Bind<IGameStartViewModelFactory>().ToFactory();
            _kernel.Bind<ICategorySelectorViewModelFactory>().ToFactory();
            _kernel.Bind<IHangmanGameViewModelFactory>().ToFactory();
            _kernel.Bind<ISaveFileSelectorViewModelFactory>().ToFactory();
            _kernel.Bind<IStatisticsViewModelFactory>().ToFactory();
            
            _kernel.Bind<MainWindow>().ToSelf();
            _kernel.Bind<MainViewModel>().ToSelf();
            _kernel.Bind<UserSelectorViewModel>().ToSelf();
            _kernel.Bind<UserCreateViewModel>().ToSelf();
            _kernel.Bind<GameStartViewModel>().ToSelf();
        }

        private void ComposeObjects()
        {
            var mainWindow = _kernel.Get<MainWindow>();
            mainWindow.DataContext = _kernel.Get<MainViewModel>();
            Current.MainWindow = mainWindow;
        }
    }
}