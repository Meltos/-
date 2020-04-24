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
    public partial class CategoryForm : Form
    {
        Paginator<CategoryDB, Category> paginator;
        ListViewViewer viewer;
        CategoryDB dB;
        public CategoryForm()
        {
            InitializeComponent();

            dB = new CategoryDB();
            // создаем экземпляр пагинатора для отображения 10 записей на странице. Число 10 можно сделать переменной и вынести в настройки
            paginator = new Paginator<CategoryDB, Category>(dB, 15);
            // для отображения данных в листвью я сделал отдельный класс
            // в нем кэшируются строки
            viewer = new ListViewViewer(listView1, 1, 15);
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

        private void ShowData(List<Category> rows)
        {
            viewer.ViewData(rows.Select(s => (IViewRow)s));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            paginator.End();
        }

        private void button2_Click_1(object sender, EventArgs e)
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

        private void добавитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddCategoryForm addCategoryForm = new AddCategoryForm(dB.Add(), dB);
            addCategoryForm.ShowDialog();
        }

        private void редактироватьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedIndices.Count == 0)
                return;
            Category category = (Category)listView1.SelectedItems[0].Tag;
            AddCategoryForm addCategoryForm = new AddCategoryForm(category, dB);
            addCategoryForm.ShowDialog();
        }

        private void удалитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedIndices.Count == 0)
                return;
            Category category = (Category)listView1.SelectedItems[0].Tag;
            if (category.Subcategories.Count > 0)
            {
                MessageBox.Show("Нельзя удалить категорию, если у неё остались подкатегории!");
                return;
            }
            if (MessageBox.Show("Точно удалить категорию?", "Предупреждение!", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                dB.Remove(category);
            }
        }
    }
}
