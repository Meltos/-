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
    public partial class AddCityForm : Form
    {
        CountryDB dB;
        City editCity;
        public AddCityForm(City editCity, CountryDB dB)
        {
            InitializeComponent();
            this.editCity = editCity;
            this.dB = dB;
            textBox1.Text = editCity.NameCity;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dB.EditCity(editCity, textBox1.Text);
            Close();
        }
    }
}
