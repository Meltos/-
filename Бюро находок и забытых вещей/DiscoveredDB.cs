using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Бюро_находок_и_забытых_вещей
{
    public class DiscoveredDB
    {
        List<string> discovereds = new List<string>();
        int i = 0;
        public DiscoveredDB()
        {
            if (i == 0)
            {
                string discovered1 = "Найдено";
                string discovered2 = "Потерянно";
                discovereds.Add(discovered1);
                discovereds.Add(discovered2);
                i++;
            }
        }

        public List<string> GetDiscovered()
        {
            return discovereds;
        }

        public List<string> GetDiscoveredBox()
        {
            List<string> discoveredsbox = new List<string>();
            string discovered3 = "";
            discoveredsbox.Add(discovered3);
            discoveredsbox.AddRange(discovereds);
            return discoveredsbox;
        }
    }
}
