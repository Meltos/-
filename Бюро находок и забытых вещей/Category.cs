using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Бюро_находок_и_забытых_вещей
{
    [Serializable]
    public class Category : IViewRow
    {
        public string NameCategory { get; set; }
        public List<SubCategory> Subcategories { get; set; } = new List<SubCategory>();

        public string GetName(int i)
        {
            return NameCategory;
        }
    }
}
