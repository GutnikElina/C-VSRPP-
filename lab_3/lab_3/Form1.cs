/*Разработать приложение, обеспечивающее возможность множественного выбора элементов из списка. Выбранные элементы должны 
 * образовывать строку текста и помещаться в строку редактирования. Предусмотреть возможность вывода сообщения в случае, 
 * если суммарное количество символов будет превышать 100.*/

using System;
using System.Linq;
using System.Windows.Forms;

namespace lab_3
{
    public partial class Form1 : Form
    {
        private ListBox listBox;
        private Button submitButton;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Инициализация элементов интерфейса
            listBox = new ListBox
            {
                SelectionMode = SelectionMode.MultiSimple, // Множественный выбор
                Size = new System.Drawing.Size(200, 150),
                Location = new System.Drawing.Point(10, 10)
            };
            listBox.Items.AddRange(new object[] { "Фрукты", "Хлеб", "Молоко", "Вода", "Картошка", "Овощи", "йцукеsdfghjkl;kmfghyujikolp;oikujyhgtrfewertgyhujikolp;qwertyuiopiuytrewqwertyuiopoiuytrewqwertyuiop[poiuytrewqwertyuiooiuytrewertyuioiuytrewqwertyuioiytrewнгшщзжюбьтпавыфячсмитьбюжзщшгнекуцывамитьлбдж.юбьтимсчяфцукенгшщздблорпавч" });

            submitButton = new Button
            {
                Text = "Отправить",
                Location = new System.Drawing.Point(10, 170),
                Size = new System.Drawing.Size(100, 30)
            };
            submitButton.Click += new EventHandler(SubmitButton_Click);

            this.Controls.Add(listBox);
            this.Controls.Add(submitButton);
        }

        private void SubmitButton_Click(object sender, EventArgs e)
        {
            // Получаем выбранные элементы
            var selectedItems = listBox.SelectedItems.Cast<string>();

            // Формируем строку из выбранных элементов
            string combinedText = string.Join(", ", selectedItems);

            // Проверка длины строки
            if (combinedText.Length > 100)
            {
                MessageBox.Show("Суммарное количество символов не должно превышать 100.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                // Создаем экземпляр второй формы
                Form2 form2 = new Form2(combinedText);
                form2.Show(); // Отображаем вторую форму
            }
        }
    }
}
