using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Бюро_находок_и_забытых_вещей
{
    public class SubCategoryDB : IDB<SubCategory>
    {
        CategoryDB dB;
        Category category;
        public int Count { get { return category.Subcategories.Count; } }

        public event EventHandler CountChanged;

        public SubCategoryDB(CategoryDB dB, Category category)
        {
            this.dB = dB;
            this.category = category;
        }

        public SubCategory Add()
        {
            SubCategory subCategory = dB.AddSubcategory(category);
            Save();
            return subCategory;
        }

        public void Edit(SubCategory edit, string name)
        {
            dB.EditSubCategory(edit, name);
            Save();
        }

        public List<SubCategory> GetData(int start, int count)
        {
            if (Count > start + count)
                return category.Subcategories.GetRange(start, count);
            else if (Count > start)
                return category.Subcategories.GetRange(start, Count - start);
            else
                return new List<SubCategory>();
        }

        public List<SubCategory> GetList()
        {
            return null;
        }

        public void Remove(SubCategory delete)
        {
            dB.RemoveSubCategory(category, delete);
            Save();
        }

        public void Save()
        {
            dB.Save();
            CountChanged?.Invoke(this, new EventArgs());
        }

        public List<SubCategory> Transformation(Category category)
        {
            return category.Subcategories;
        }

        public List<Category> GetCategories()
        {
            return dB.GetList();
        }

        
    }
}
