using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Бюро_находок_и_забытых_вещей
{
    public class CategoryDB : IDB<Category>
    {
        List<Category> categories = new List<Category>();
        string fileName = "catrgorydb.bin";

        public event EventHandler CountChanged;

        public int Count { get { return categories.Count; } }

        public void Save()
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write))
            {
                bf.Serialize(fs, categories);
            }
            CountChanged?.Invoke(this, new EventArgs());
        }

        public CategoryDB()
        {
            if (!File.Exists(fileName))
                return;
            BinaryFormatter bf = new BinaryFormatter();
            using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                categories = (List<Category>)bf.Deserialize(fs);
            }
        }

        public Category Add()
        {
            Category category = new Category { NameCategory = "" };
            categories.Add(category);
            Save();
            return category;
        }

        public List<Category> GetList()
        {
            return categories;
        }

        public SubCategory AddSubcategory(Category selectedcategory)
        {
            SubCategory newsubcategory = new SubCategory
            {
                NameSubcategory = "",
                Category = selectedcategory
            };
            selectedcategory.Subcategories.Add(newsubcategory);
            Save();
            return newsubcategory;
        }

        public List<SubCategory> GetSubCategories(Category selectedcategory)
        {
            return selectedcategory.Subcategories;
        }

        public void RemoveSubCategory(Category category, SubCategory subCategory)
        {
            category.Subcategories.Remove(subCategory);
            Save();
        }

        public List<Category> GetData(int start, int count)
        {
            if (Count > start + count)
                return categories.GetRange(start, count);
            else if (Count > start)
                return categories.GetRange(start, Count - start);
            else
                return new List<Category>();
        }

        public void Remove(Category delete)
        {
            if (delete.Subcategories.Count > 0)
                return;
            categories.Remove(delete);
            Save();
        }

        public void Edit(Category edit, string name)
        {
            edit.NameCategory = name;
            Save();
        }
        
        public void EditSubCategory(SubCategory selectedsubCategory, string name)
        {
            selectedsubCategory.NameSubcategory = name;
            Save();
        }

        public List<Category> GetListCombobox()
        {
            List<Category> categoriesbox = new List<Category>();
            Category nullcategory = new Category { NameCategory = "" };
            categoriesbox.Add(nullcategory);
            categoriesbox.AddRange(categories);
            return categoriesbox;
        }

        public List<SubCategory> GetListComboboxSubCategory(Category category)
        {
            List<SubCategory> subCategoriesbox = new List<SubCategory>();
            SubCategory nullsubCategory = new SubCategory { NameSubcategory = "", Category = category };
            subCategoriesbox.Add(nullsubCategory);
            subCategoriesbox.AddRange(category.Subcategories);
            return subCategoriesbox;
        }
    }
}
