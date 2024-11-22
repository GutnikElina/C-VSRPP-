using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;

namespace lab_5
{
    public partial class Form1 : Form
    {
        public static string connectString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=Students.mdb;";
        private OleDbConnection myConnection;

        public Form1()
        {
            InitializeComponent();
            myConnection = new OleDbConnection(connectString);
            myConnection.Open();
        }

        private void Select1_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                string element = listBox1.SelectedItem.ToString();
                element = element.Substring(element.IndexOf(' '));
                textBox1.Text = element;
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите элемент из списка.");
            }
        }

        private void SelectAll_Click(object sender, EventArgs e)
        {
            string query = "SELECT st_id, st_name, st_age, st_room FROM Student ORDER BY st_id";

            using (OleDbConnection myConnection = new OleDbConnection(connectString))
            {
                try
                {
                    myConnection.Open();
                    using (OleDbCommand command = new OleDbCommand(query, myConnection))
                    {
                        using (OleDbDataReader reader = command.ExecuteReader())
                        {
                            listBox1.Items.Clear();
                            while (reader.Read())
                            {
                                string formattedString = String.Format("{0}, {1}, {2}, {3}",
                                                                        reader[0],  
                                                                        reader[1],  
                                                                        reader[2],  
                                                                        reader[3]); 
                                listBox1.Items.Add(formattedString);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при загрузке данных: " + ex.Message);
                }
            }
        }


        private void Insert_Click(object sender, EventArgs e)
        {
            string element = textBox1.Text.Trim();
            string[] words = element.Split(new char[] { ' ' });

            if (words.Length < 3)
            {
                MessageBox.Show("Пожалуйста, введите имя студента, возраст и комнату.");
                return;
            }

            string Name = words[0];
            string Age = words[1];
            string Room = words[2];

            string query = "INSERT INTO Student (st_name, st_age, st_room) VALUES (@name, @age, @room)";

            using (OleDbConnection myConnection = new OleDbConnection(connectString))
            {
                try
                {
                    myConnection.Open();

                    using (OleDbCommand command = new OleDbCommand(query, myConnection))
                    {
                        command.Parameters.AddWithValue("@name", Name);
                        command.Parameters.AddWithValue("@age", Age);
                        command.Parameters.AddWithValue("@room", Room);
                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка: " + ex.Message);
                }
            }
            SelectAll_Click(sender, e);
        }

        private int GetSelectedStudentId()
        {
            if (listBox1.SelectedItem == null)
            {
                MessageBox.Show("Пожалуйста, выберите элемент.");
                return -1;
            }

            string element = listBox1.SelectedItem.ToString();

            string idString = element.Split(',')[0].Replace("ID: ", "").Trim();
            if (int.TryParse(idString, out int studentId))
            {
                return studentId;
            }
            else
            {
                MessageBox.Show("Ошибка: Неверный формат ID.");
                return -1;
            }
        }

        private void Update_Click(object sender, EventArgs e)
        {
            int studentId = GetSelectedStudentId();
            if (studentId == -1)
            {
                return;
            }

            string input = textBox1.Text.Trim();
            if (string.IsNullOrWhiteSpace(input))
            {
                MessageBox.Show("Пожалуйста, введите данные для обновления.");
                return;
            }

            string[] words = input.Split(new char[] { ' ' });

            if (words.Length < 3)
            {
                MessageBox.Show("Пожалуйста, введите имя, возраст и комнату.");
                return;
            }

            string name = words[0];
            string age = words[1];
            string room = words[2];

            if (!int.TryParse(age, out _))
            {
                MessageBox.Show("Ошибка: Возраст должен быть числом.");
                return;
            }

            if (!int.TryParse(room, out _))
            {
                MessageBox.Show("Ошибка: Комната должна быть числом.");
                return;
            }

            string query = "UPDATE Student SET st_name = @name, st_age = @age, st_room = @room WHERE st_id = @studentId";

            using (OleDbConnection myConnection = new OleDbConnection(connectString))
            {
                try
                {
                    myConnection.Open();
                    using (OleDbCommand command = new OleDbCommand(query, myConnection))
                    {
                        command.Parameters.AddWithValue("@name", name);
                        command.Parameters.AddWithValue("@age", age);
                        command.Parameters.AddWithValue("@room", room);
                        command.Parameters.AddWithValue("@studentId", studentId);

                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка: " + ex.Message);
                }
            }
            SelectAll_Click(sender, e);
        }

        private void Delete_Click(object sender, EventArgs e)
        {
            int studentId = GetSelectedStudentId();
            if (studentId == -1)
            {
                return;
            }

            string query = "DELETE FROM Student WHERE st_id = @studentId";

            using (OleDbConnection myConnection = new OleDbConnection(connectString))
            {
                try
                {
                    myConnection.Open();
                    using (OleDbCommand command = new OleDbCommand(query, myConnection))
                    {
                        command.Parameters.AddWithValue("@studentId", studentId);
                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка: " + ex.Message);
                }
            }
            SelectAll_Click(sender, e);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            myConnection.Close();
        }
    }
}
