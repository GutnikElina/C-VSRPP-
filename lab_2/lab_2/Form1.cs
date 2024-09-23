/*Создать программу для рисования трехконечной звезды с окраской  лучей тремя выбранными цветами. Каждый луч должен иметь свой цвет. 
 * Серцевина звезды имеет нерегулируемый цвет, заданный програмно на ваше усмотрение.
 Реализовать возможность удаления и восстановления объектов.*/


using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace lab_2
{
    public partial class Form1 : Form
    {
        Graphics ag;
        private Bitmap map;
        private List<Bitmap> bit;  // Список для хранения состояний рисунков
        private Stack<Bitmap> bitD; // Стек для хранения удаленных объектов

        SolidBrush BrushC1 = new SolidBrush(Color.FromArgb(255, 223, 0));   
        SolidBrush BrushC2 = new SolidBrush(Color.FromArgb(255, 204, 0));  
        SolidBrush BrushC3 = new SolidBrush(Color.FromArgb(255, 255, 102)); 
        SolidBrush CoreBrush = new SolidBrush(Color.FromArgb(204, 153, 0)); 

        public Form1()
        {
            InitializeComponent();
            SetSize();
        }

        private void SetSize()
        {
            Rectangle rectangle = Screen.PrimaryScreen.Bounds;     // узнаем размер нашего окна
            map = new Bitmap(rectangle.Width, rectangle.Height);   // создаем битмап map с шириной и высотой окна
            ag = Graphics.FromImage(map);                          // создание объекта для рисования
            bit = new List<Bitmap>();
            bitD = new Stack<Bitmap>(); // Стек для удаления и восстановления
            bit.Add(new Bitmap(map));   // Сохраняем начальное состояние
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox_mouseClick(object sender, MouseEventArgs e)
        {
            ag = Graphics.FromImage(map);
            DrawStar(e.X, e.Y);  // Рисуем звезду по клику
            bit.Add(new Bitmap(map));   // Сохраняем текущее состояние после рисования
            pictureBox1.Image = map;    // Обновляем изображение на PictureBox
        }

        private void DrawStar(int x, int y)
        {
            int radius = 100; 
            int innerRadius = 30; 

            float angle1 = 0f; 
            float angle2 = 120f; 
            float angle3 = 240f;

            double rad1 = angle1 * Math.PI / 180.0;
            double rad2 = angle2 * Math.PI / 180.0;
            double rad3 = angle3 * Math.PI / 180.0;

            // Вычисляем точки для каждого луча
            Point[] points1 = {
        new Point(x, y),
        new Point(x + (int)(Math.Cos(rad1) * radius), y + (int)(Math.Sin(rad1) * radius)),
        new Point(x + (int)(Math.Cos(rad1 + Math.PI / 6) * radius / 2), y + (int)(Math.Sin(rad1 + Math.PI / 6) * radius / 2))
    };

            Point[] points2 = {
        new Point(x, y),
        new Point(x + (int)(Math.Cos(rad2) * radius), y + (int)(Math.Sin(rad2) * radius)),
        new Point(x + (int)(Math.Cos(rad2 + Math.PI / 6) * radius / 2), y + (int)(Math.Sin(rad2 + Math.PI / 6) * radius / 2))
    };

            Point[] points3 = {
        new Point(x, y),
        new Point(x + (int)(Math.Cos(rad3) * radius), y + (int)(Math.Sin(rad3) * radius)),
        new Point(x + (int)(Math.Cos(rad3 + Math.PI / 6) * radius / 2), y + (int)(Math.Sin(rad3 + Math.PI / 6) * radius / 2))
    };

            ag.FillPolygon(BrushC1, points1); 
            ag.FillPolygon(BrushC2, points2);
            ag.FillPolygon(BrushC3, points3);

            ag.FillEllipse(CoreBrush, x - innerRadius / 2, y - innerRadius / 2, innerRadius, innerRadius); 
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (bit.Count <= 1)
            {
                return;  // Нельзя удалить начальное состояние
            }
            else
            {
                bitD.Push(bit[bit.Count - 1]);  // Добавляем последнее состояние в стек удаленных
                bit.RemoveAt(bit.Count - 1);    // Удаляем его из основного списка
                map = new Bitmap(bit[bit.Count - 1]); // Возвращаемся к предыдущему состоянию
                pictureBox1.Image = map;        // Обновляем изображение на PictureBox
            }
        }

        private void restoreToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (bitD.Count > 0)
            {
                bit.Add(bitD.Pop());            // Добавляем последнее удаленное состояние обратно
                map = new Bitmap(bit[bit.Count - 1]); // Обновляем текущее состояние
                pictureBox1.Image = map;        // Обновляем изображение на PictureBox
            }
        }
    }
}
