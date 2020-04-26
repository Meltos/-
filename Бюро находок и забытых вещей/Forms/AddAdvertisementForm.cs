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
    public partial class AddAdvertisementForm : Form
    {
        AdvertisementDB dB;
        CategoryDB categoryDB;
        CountryDB countryDB;
        DiscoveredDB discoveredDB;
        Advertisement advertisement;
        int code;
        public AddAdvertisementForm(AdvertisementDB dB, CategoryDB categoryDB, CountryDB countryDB, DiscoveredDB discoveredDB,Advertisement advertisement, int code)
        {
            InitializeComponent();
            this.categoryDB = categoryDB;
            this.countryDB = countryDB;
            this.discoveredDB = discoveredDB;
            this.advertisement = advertisement;
            this.dB = dB;
            this.code = code;
            textBox1.Text = advertisement.Header;
            textBox4.Text = advertisement.Address;
            textBox2.Text = advertisement.Phone.ToString();
            textBox5.Text = advertisement.ContactPerson;
            textBox3.Text = advertisement.Description;
            comboBox3.DataSource = null;
            comboBox3.DataSource = countryDB.GetList();
            comboBox3.DisplayMember = "NameCountry";
            
            comboBox1.DataSource = null;
            comboBox1.DataSource = categoryDB.GetList();
            comboBox1.DisplayMember = "NameCategory";
            
            comboBox5.DataSource = null;
            comboBox5.DataSource = discoveredDB.GetDiscovered();
            comboBox5.DisplayMember = "Status";

            if (code == 1)
            {
                button2.Visible = false;
                label11.Visible = false;
            }
            else if (code == 0)
            {
                comboBox3.SelectedItem = advertisement.Country;
                comboBox1.SelectedItem = advertisement.Category;
                comboBox5.SelectedItem = advertisement.Discovered;
                button2.Visible = true;
                label11.Visible = true;
                TimeSpan timeSpan = new TimeSpan();
                timeSpan = DateTime.Now - advertisement.Time;
                label11.Text = $"Время прошло: {timeSpan}";
            }
            if (advertisement.Close == true)
            {
                textBox1.ReadOnly = true;
                textBox2.ReadOnly = true;
                textBox3.ReadOnly = true;
                textBox4.ReadOnly = true;
                textBox5.ReadOnly = true;
                comboBox1.Enabled = false;
                comboBox2.Enabled = false;
                comboBox3.Enabled = false;
                comboBox4.Enabled = false;
                comboBox5.Enabled = false;
                button2.Enabled = false;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Category combocategory = new Category();
            combocategory = (Category)comboBox1.SelectedItem;
            if (combocategory.Subcategories.Count == 0)
            {
                label3.Visible = false;
                comboBox2.Visible = false;
                comboBox2.DataSource = null;
                comboBox2.DataSource = categoryDB.GetListComboboxSubCategory((Category)comboBox1.SelectedItem);
                comboBox2.DisplayMember = "NameSubcategory";
                return;
            }
            label3.Visible = true;
            comboBox2.Visible = true;
            comboBox2.DataSource = null;
            comboBox2.DataSource = categoryDB.GetSubCategories((Category)comboBox1.SelectedItem);
            comboBox2.DisplayMember = "NameSubcategory";
            if (code == 0)
                comboBox2.SelectedItem = advertisement.Subcategory;
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            Country combocountry = new Country();
            combocountry = (Country)comboBox3.SelectedItem;
            if (combocountry.Cities.Count == 0)
            {
                label5.Visible = false;
                comboBox4.Visible = false;
                comboBox4.DataSource = null;
                comboBox4.DataSource = countryDB.GetListComboboxCity((Country)comboBox3.SelectedItem);
                comboBox4.DisplayMember = "NameCity";
                return;
            }
            label5.Visible = true;
            comboBox4.Visible = true;
            comboBox4.DataSource = null;
            comboBox4.DataSource = countryDB.GetCities((Country)comboBox3.SelectedItem);
            comboBox4.DisplayMember = "NameCity";
            if (code == 0)
                comboBox4.SelectedItem = advertisement.City;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            advertisement.Header = textBox1.Text;
            advertisement.Phone = long.Parse(textBox2.Text);
            advertisement.Description = textBox3.Text;
            advertisement.Address = textBox4.Text;
            advertisement.ContactPerson = textBox5.Text;
            advertisement.Category = (Category)comboBox1.SelectedItem;
            advertisement.Subcategory = (SubCategory)comboBox2.SelectedItem;
            advertisement.Country = (Country)comboBox3.SelectedItem;
            advertisement.City = (City)comboBox4.SelectedItem;
            advertisement.Discovered = (string)comboBox5.SelectedItem;
            advertisement.Time = DateTime.Now;
            dB.Save();
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            advertisement.Close = true;
            textBox1.ReadOnly = true;
            textBox2.ReadOnly = true;
            textBox3.ReadOnly = true;
            textBox4.ReadOnly = true;
            textBox5.ReadOnly = true;
            comboBox1.Enabled = false;
            comboBox2.Enabled = false;
            comboBox3.Enabled = false;
            comboBox4.Enabled = false;
            comboBox5.Enabled = false;
            button2.Enabled = false;
        }
    }
}
