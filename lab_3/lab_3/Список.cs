/*Разработать приложение, обеспечивающее возможность множественного выбора элементов из списка. Выбранные элементы должны 
 * образовывать строку текста и помещаться в строку редактирования. Предусмотреть возможность вывода сообщения в случае, 
 * если суммарное количество символов будет превышать 100.*/

using System;
using System.Linq;
using System.Windows.Forms;

namespace lab_3
{
    public partial class Список : Form
    {
        private ListBox listBox;
        private Button submitButton;
        private Button addButton;
        private Button removeButton;
        private TextBox inputTextBox;

        public Список()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            listBox = new ListBox
            {
                SelectionMode = SelectionMode.MultiSimple, 
                Size = new System.Drawing.Size(200, 150),
                Location = new System.Drawing.Point(10, 10)
            };
            listBox.Items.AddRange(new object[] { "Фрукты", "Хлеб", "Молоко", "Вода", "Картошка", "Овощи", "йцукеsdfghjkl;kmfghyujikolp;oikujyhgtrfewertgyhujikolp;qwertyuiopiuytrewqwertyuiopoiuytrewqwertyuiop[poiuytrewqwertyuiooiuytrewertyuioiuytrewqwertyuioiytrewнгшщзжюбьтпавыфячсмитьбюжзщшгнекуцывамитьлбдж.юбьтимсчяфцукенгшщздблорпавч" });

            submitButton = new Button
            {
                Text = "Отправить",
                Location = new System.Drawing.Point(10, 240),
                Size = new System.Drawing.Size(100, 30)
            };
            submitButton.Click += new EventHandler(SubmitButton_Click);

            inputTextBox = new TextBox
            {
                Location = new System.Drawing.Point(10, 170),
                Size = new System.Drawing.Size(200, 20)
            };

            addButton = new Button
            {
                Text = "Добавить",
                Location = new System.Drawing.Point(220, 170),
                Size = new System.Drawing.Size(120, 20)
            };
            addButton.Click += new EventHandler(AddButton_Click);

            removeButton = new Button
            {
                Text = "Удалить",
                Location = new System.Drawing.Point(220, 200),
                Size = new System.Drawing.Size(120, 20)
            };
            removeButton.Click += new EventHandler(RemoveButton_Click);

            this.Controls.Add(listBox);
            this.Controls.Add(submitButton);
            this.Controls.Add(inputTextBox);
            this.Controls.Add(addButton);
            this.Controls.Add(removeButton);
        }

        private void SubmitButton_Click(object sender, EventArgs e)
        {
            var selectedItems = listBox.SelectedItems.Cast<string>();

            string combinedText = string.Join(", ", selectedItems);

            if (combinedText.Length > 100)
            {
                MessageBox.Show("Суммарное количество символов не должно превышать 100.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                Результат form2 = new Результат(combinedText);
                form2.Show(); 
            }
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            string newItem = inputTextBox.Text.Trim();

            if (!string.IsNullOrEmpty(newItem))
            {
                listBox.Items.Add(newItem);

                inputTextBox.Clear();
            }
            else
            {
                MessageBox.Show("Поле ввода пустое", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void RemoveButton_Click(object sender, EventArgs e)
        {
            if (listBox.SelectedItem != null)
            {
                while (listBox.SelectedItems.Count > 0)
                {
                    listBox.Items.Remove(listBox.SelectedItems[0]);
                }
            }
            else
            {
                MessageBox.Show("Не выбран ни один элемент", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
