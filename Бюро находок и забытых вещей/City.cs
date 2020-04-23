using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Бюро_находок_и_забытых_вещей
{
    [Serializable]
    public class City: IViewRow
    {
        public string NameCity { get; set; }
        public Country Country { get; set; }

        public string GetName(int i)
        {
            return NameCity;
        }
    }
}
