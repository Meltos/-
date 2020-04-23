using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Бюро_находок_и_забытых_вещей
{
    [Serializable]
    public class SubCategory : IViewRow
    {
        public string NameSubcategory { get; set; }
        public Category Category { get; set; }

        public string GetName(int i)
        {
            return NameSubcategory;
        }
    }
}
