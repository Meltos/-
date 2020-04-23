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
    public partial class AddCategoryForm : Form
    {
        Category category;
        CategoryDB dB;
        public AddCategoryForm(Category category, CategoryDB dB)
        {
            InitializeComponent();
            this.category = category;
            this.dB = dB;
            textBox1.Text = category.NameCategory;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dB.Edit(category, textBox1.Text);
            Close();
        }

    }
}
