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
    public partial class SubcategoryForm : Form
    {
        Paginator<SubCategoryDB, SubCategory> paginator;
        ListViewViewer viewer;
        CategoryDB categoryDB;
        SubCategoryDB dB;
        public SubcategoryForm(CategoryDB categoryDB)
        {
            InitializeComponent();
            this.categoryDB = categoryDB;
            comboBox1.DataSource = null;
            comboBox1.DataSource = categoryDB.GetList();
            comboBox1.DisplayMember = "NameCategory";
            
            // подписываемся на событие изменения выводимых записей
            paginator.ShowRowsChanges += Paginator_ShowRowsChanges;
            // подписываемся на изменение кол-ва страниц
            paginator.CountChanged += Paginator_CountChanged;
            // подписываемся на изменение текущего индекса
            paginator.CurrentIndexChanged += Paginator_CurrentIndexChanged;

            // вызываем обновление всех данных и событий
            // за счет того, что данный метод вызывается ПОСЛЕ создания пагинатора интерфейс успевает подписаться на события пагинатора и нормально отобразить все данные
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
            ShowData(paginator.ShowRows);
        }

        private void ShowData(List<SubCategory> rows)
        {
            viewer.ViewData(rows.Select(s => (IViewRow)s));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            paginator.End();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            paginator.Start();
        }

        private void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            if (e.NewValue > e.OldValue)
                paginator.Right();
            else if (e.NewValue < e.OldValue)
                paginator.Left();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int i = 0;
            if (i == 1)
            {
                paginator.ShowRowsChanges -= Paginator_ShowRowsChanges;
                // подписываемся на изменение кол-ва страниц
                paginator.CountChanged -= Paginator_CountChanged;
                // подписываемся на изменение текущего индекса
                paginator.CurrentIndexChanged -= Paginator_CurrentIndexChanged;
                i--;
            }
            dB = new SubCategoryDB(categoryDB, (Category)comboBox1.SelectedItem);
            // создаем экземпляр пагинатора для отображения 10 записей на странице. Число 10 можно сделать переменной и вынести в настройки
            paginator = new Paginator<SubCategoryDB, SubCategory>(dB, 15);
            // для отображения данных в листвью я сделал отдельный класс
            // в нем кэшируются строки
            viewer = new ListViewViewer(listView1, 1, 15);
            // подписываемся на событие изменения выводимых записей
            paginator.ShowRowsChanges += Paginator_ShowRowsChanges;
            // подписываемся на изменение кол-ва страниц
            paginator.CountChanged += Paginator_CountChanged;
            // подписываемся на изменение текущего индекса
            paginator.CurrentIndexChanged += Paginator_CurrentIndexChanged;
            i++;
            dB.Save();
        }

        private void добавитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddSubcategoryForm addSubcategoryForm = new AddSubcategoryForm(dB.Add(),categoryDB);
            addSubcategoryForm.ShowDialog();
            dB.Save();
        }

        private void редактироватьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedIndices.Count == 0)
                return;
            SubCategory subCategory = (SubCategory)listView1.SelectedItems[0].Tag;
            AddSubcategoryForm addSubcategoryForm = new AddSubcategoryForm(subCategory, categoryDB);
            addSubcategoryForm.ShowDialog();
            dB.Save();
        }

        private void удалитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedIndices.Count == 0)
                return;
            SubCategory subCategory = (SubCategory)listView1.SelectedItems[0].Tag;
            if (MessageBox.Show("Точно удалить подкатегорию?", "Предупреждение!", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                dB.Remove(subCategory);
                dB.Save();
            }
        }
    }
}
