using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;

namespace Hangman.ViewModels
{
    public class KeyboardViewModel : ObservableObject
    {
        public event Action<char> KeyPressedEvent;
        private HashSet<char> _usedKeys;

        public HashSet<char> UsedKeys
        {
            get => _usedKeys;
            set => SetProperty(ref _usedKeys, value);
        }
        
        public RelayCommand<string> KeyPressCommand { get; }

        public KeyboardViewModel()
        {
            UsedKeys = new HashSet<char>();
            KeyPressCommand = new RelayCommand<string>(OnKeyPress);
        }

        private void OnKeyPress(string keyString)
        {
            UsedKeys.Add(keyString[0]);
            KeyPressedEvent?.Invoke(keyString[0]);
            OnPropertyChanged(nameof(UsedKeys));
        }

        public void ResetKeyboard()
        {
            UsedKeys = new HashSet<char>();
            OnPropertyChanged(nameof(UsedKeys));
        }
    }
}