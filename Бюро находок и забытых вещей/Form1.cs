using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ����_�������_�_�������_�����
{
    public partial class Form1 : Form
    {
        Paginator<AdvertisementDB, Advertisement> paginator;
        ListViewViewer viewer;
        CategoryDB categoryDB;
        SubCategoryDB subCategoryDB;
        CountryDB countryDB;
        CityDB cityDB;
        DiscoveredDB discoveredDB;
        AdvertisementDB dB;
        public Form1()
        {
            InitializeComponent();

            countryDB = new CountryDB();
            comboBox1.DataSource = null;
            comboBox1.DataSource = countryDB.GetListCombobox();
            comboBox1.DisplayMember = "NameCountry";
            
            categoryDB = new CategoryDB();
            comboBox3.DataSource = null;
            comboBox3.DataSource = categoryDB.GetListCombobox();
            comboBox3.DisplayMember = "NameCategory";

            discoveredDB = new DiscoveredDB();
            comboBox5.DataSource = null;
            comboBox5.DataSource = discoveredDB.GetDiscoveredBox();
            comboBox5.DisplayMember = "Status";

            dB = new AdvertisementDB();
            // ������� ��������� ���������� ��� ����������� 10 ������� �� ��������. ����� 10 ����� ������� ���������� � ������� � ���������
            paginator = new Paginator<AdvertisementDB, Advertisement>(dB, 10);
            // ��� ����������� ������ � ������� � ������ ��������� �����
            // � ��� ���������� ������
            viewer = new ListViewViewer(listView1, 5, 10);
            

            // �������� ���������� ���� ������ � �������
            // �� ���� ����, ��� ������ ����� ���������� ����� �������� ���������� ��������� �������� ����������� �� ������� ���������� � ��������� ���������� ��� ������
            dB.Save();
        }

        private void Paginator_CurrentIndexChanged(object sender, EventArgs e)
        {
            hScrollBar1.Value = paginator.CurrentIndex;
        }

        private void Paginator_CountChanged(object sender, EventArgs e)
        {
            hScrollBar1.Maximum = paginator.MaxIndex;
            hScrollBar1.Value = paginator.CurrentIndex;
        }

        private void Paginator_ShowRowsChanges(object sender, EventArgs e)
        {
            ShowAdvertisementData(paginator.ShowRows);
        }

        private void ShowAdvertisementData(List<Advertisement> rows)
        {
            listView1.Items.Clear();
            int code = 0;
            Country combocountry = new Country();
            combocountry = (Country)comboBox1.SelectedItem;
            City combocity = new City();
            combocity = (City)comboBox2.SelectedItem;
            Category combocategory = new Category();
            combocategory = (Category)comboBox3.SelectedItem;
            SubCategory combosubCategory = new SubCategory();
            combosubCategory = (SubCategory)comboBox4.SelectedItem;
            Discovered combodiscovered = new Discovered();
            combodiscovered = (Discovered)comboBox5.SelectedItem;
            if (combocountry.NameCountry =="" && combocategory.NameCategory == "" && combodiscovered.Status == "")
            {
                code = 0;
                viewer.ViewData(rows, code);
                return;
            }
            if (combocountry.NameCountry != "" && combocategory.NameCategory != "" && combodiscovered.Status != "" && combocity.NameCity != "" && combosubCategory.NameSubcategory != "")
            {
                code = 1;
                viewer.ViewData(rows, code);
                return;
            }
            

        }

        private void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            if (e.NewValue > e.OldValue)
                paginator.Right();
            else if (e.NewValue < e.OldValue)
                paginator.Left();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            paginator.Start();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            paginator.End();
        }

        private void ��������������ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CountryForm countryForm = new CountryForm();
            countryForm.ShowDialog();
        }

        private void �������������ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CityForm cityForm = new CityForm();
            cityForm.ShowDialog();
        }

        private void �����������������ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CategoryForm categoryForm = new CategoryForm();
            categoryForm.ShowDialog();
        }

        private void ��������������������ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SubcategoryForm subcategoryForm = new SubcategoryForm();
            subcategoryForm.ShowDialog();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Country combocountry = new Country();
            combocountry = (Country)comboBox1.SelectedItem;
            if (combocountry.NameCountry == "")
            {
                label2.Visible = false;
                comboBox2.Visible = false;
                return;
            }
            label2.Visible = true;
            comboBox2.Visible = true;
            cityDB = new CityDB(countryDB, (Country)comboBox1.SelectedItem);
            comboBox2.DataSource = null;
            comboBox2.DataSource = cityDB.TransformationCombobox((Country)comboBox1.SelectedItem);
            comboBox2.DisplayMember = "NameCity";
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            Category combocategory = new Category();
            combocategory = (Category)comboBox3.SelectedItem;
            if (combocategory.NameCategory == "")
            {
                label4.Visible = false;
                comboBox4.Visible = false;
                return;
            }
            label4.Visible = true;
            comboBox4.Visible = true;
            subCategoryDB = new SubCategoryDB(categoryDB, (Category)comboBox3.SelectedItem);
            comboBox4.DataSource = null;
            comboBox4.DataSource = subCategoryDB.TransformationComboboxCategory((Category)comboBox3.SelectedItem);
            comboBox4.DisplayMember = "NameSubcategory";
        }

        private void ������������������ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            paginator.ShowRowsChanges -= Paginator_ShowRowsChanges;
            AddAdvertisementForm addAdvertisementForm = new AddAdvertisementForm(dB, categoryDB, subCategoryDB, countryDB, cityDB, discoveredDB, dB.Add(), 1);
            addAdvertisementForm.ShowDialog();
            dB.Save();
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            if (listView1.SelectedIndices.Count == 0)
                return;
            Advertisement advertisement1 = (Advertisement)listView1.SelectedItems[0].Tag;
            AddAdvertisementForm addAdvertisementForm = new AddAdvertisementForm(dB, categoryDB, subCategoryDB, countryDB, cityDB, discoveredDB, advertisement1, 0);
            addAdvertisementForm.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int i = 0;
            if (i == 1)
            {
                // ������������� �� ������� ��������� ��������� �������
                paginator.ShowRowsChanges -= Paginator_ShowRowsChanges;
                // ������������� �� ��������� ���-�� �������
                paginator.CountChanged -= Paginator_CountChanged;
                // ������������� �� ��������� �������� �������
                paginator.CurrentIndexChanged -= Paginator_CurrentIndexChanged;
                i--;
            }
            // ������������� �� ������� ��������� ��������� �������
            paginator.ShowRowsChanges += Paginator_ShowRowsChanges;
            // ������������� �� ��������� ���-�� �������
            paginator.CountChanged += Paginator_CountChanged;
            // ������������� �� ��������� �������� �������
            paginator.CurrentIndexChanged += Paginator_CurrentIndexChanged;
            i++;
            dB.Save();
        }
    }
}
