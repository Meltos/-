using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Бюро_находок_и_забытых_вещей
{
    public partial class AddCountryForm : Form
    {
        Country editCountry;
        CountryDB dB;
        public AddCountryForm(Country editCountry, CountryDB dB)
        {
            InitializeComponent();
            this.editCountry = editCountry;
            this.dB = dB;
            textBox1.Text = editCountry.NameCountry;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dB.Edit(editCountry, textBox1.Text);
            Close();
        }
    }
}
