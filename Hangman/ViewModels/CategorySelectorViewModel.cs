using System.Collections.ObjectModel;
using Hangman.Managers;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;

namespace Hangman.ViewModels
{
    public class CategorySelectorViewModel : ObservableObject
    {
        private readonly ICategoryManager _categoryManager;

        private string _selectedCategory;
        private ObservableCollection<string> _categories;

        public string SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                SetProperty(ref _selectedCategory, value);
                OnPropertyChanged(nameof(CategoryHasBeenSelected));
            }
        }

        public bool CategoryHasBeenSelected => !string.IsNullOrEmpty(SelectedCategory);
        
        public ObservableCollection<string> Categories
        {
            get => _categories;
            set => SetProperty(ref _categories, value);
        }

        public CategorySelectorViewModel(ICategoryManager categoryManager)
        {
            _categoryManager = categoryManager;
            Categories = new ObservableCollection<string>(_categoryManager.GetAllCategories());
        }
    }
}