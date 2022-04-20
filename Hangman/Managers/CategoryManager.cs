using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Hangman.Managers
{
    public interface ICategoryManager
    {
        IEnumerable<string> GetAllCategories();
        IEnumerable<string> GetWordsInCategory(string categoryName);
    }

    public class CategoryManager : ICategoryManager
    {
        private const string CategoryExtension = ".category";
        private const string CategoriesSavePath = @"\Categories\";
        private static readonly string CategoriesAbsolutePath;
        
        static CategoryManager()
        {
            CategoriesAbsolutePath = Directory.GetParent(Assembly.GetExecutingAssembly().Location) + CategoriesSavePath;
            
            if (!Directory.Exists(CategoriesAbsolutePath))
                Directory.CreateDirectory(CategoriesAbsolutePath);
        }

        public IEnumerable<string> GetAllCategories()
            => Directory.GetFiles(CategoriesAbsolutePath, "*" + CategoryExtension)
                .Select(Path.GetFileNameWithoutExtension);

        public IEnumerable<string> GetWordsInCategory(string categoryName)
            => File.ReadLines(CategoriesAbsolutePath + categoryName + CategoryExtension);
    }
}