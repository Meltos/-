using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Бюро_находок_и_забытых_вещей
{
    [Serializable]
    public class Advertisement : IViewRow
    {
        public string Header { get; set; }
        public Category Category { get; set; }
        public SubCategory Subcategory { get; set; }
        public Country Country { get; set; }
        public City City { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
        public long Phone { get; set; }
        public DateTime Time { get; set; }
        public string Discovered { get; set; }
        public string ContactPerson { get; set; }
        public bool Close { get; set; }

        public string GetName(int i)
        {
            switch (i)
            {
                case 0: return Header;
                case 1: return City.NameCity;
                case 2: return Discovered;
                case 3: return Description;
                case 4:
                    {
                        if (Close == true)
                            return "Зактрыто";
                        else
                            return "Открыто";
                    }
            }
            return null;
        }
    }
}
