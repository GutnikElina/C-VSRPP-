using System;
using System.Windows.Forms;

namespace lab_3
{
    public partial class Результат : Form
    {
        private TextBox textBox;

        public Результат(string combinedText)
        {
            InitializeComponent();

            // Инициализация TextBox
            textBox = new TextBox
            {
                Location = new System.Drawing.Point(10, 10),
                Size = new System.Drawing.Size(300, 20),
                ReadOnly = true, 
                Text = combinedText
            };

            this.Controls.Add(textBox);
        }

        private void Form2_Load(object sender, EventArgs e)
        {
        }
    }
}
