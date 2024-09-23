/*Разработать программу, позволяющую рисовать прямоугольники. Предусмотреть все возможности, что были реализованы при рисовании линий. Предусмотреть следующие дополнительные возможности.
	- При движении мыши с одновременным нажатием клавиши Shift – рисовать контур красным пером.
    - В меню Edit добавить пункты Blue (окрашивание границ прямоугольников голубым цветом), Pink (окрашивание границ прямоугольников розовым цветом).
*/

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace lab1
{
    public partial class Form1 : Form
    {
        private Graphics g; // Объявляем объект класса Graphics
        private Pen p; // Перо для рисования
        private SolidBrush brush = new SolidBrush(Color.Red); // Кисть для заливки
        private List<Rectangle> rectangles = new List<Rectangle>(); // Список для хранения прямоугольников

        private int startX, startY;
        private bool drawingRectangle = false;
        private Color borderColor = Color.Bisque; 
        private Rectangle currentRectangle = Rectangle.Empty; // Прямоугольник для рисования в текущий момент

        private Stack<List<Rectangle>> undoStack = new Stack<List<Rectangle>>();
        private Stack<List<Rectangle>> redoStack = new Stack<List<Rectangle>>();

        private bool shiftPressed = false; 

        public Form1()
        {
            InitializeComponent();
            this.KeyPreview = true; 
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyUp);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseUp);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Form1_Paint);

           
            p = new Pen(borderColor, 5);
        }

        private void Form1_Load(object sender, EventArgs e) { }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            g = e.Graphics;
            foreach (var rect in rectangles)
            {
                using (var pen = new Pen(borderColor, 5))
                {
                    g.DrawRectangle(pen, rect);
                }
            }
            if (drawingRectangle && currentRectangle != Rectangle.Empty)
            {
                using (var pen = new Pen(borderColor, 5))
                {
                    g.DrawRectangle(pen, currentRectangle);
                }
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Shift)
            {
                shiftPressed = true; 
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (!e.Shift)
            {
                shiftPressed = false; 
            }
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                startX = e.X;
                startY = e.Y;
                drawingRectangle = true;
                currentRectangle = new Rectangle(startX, startY, 0, 0);
            }
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (drawingRectangle)
            {
                int width = e.X - startX;
                int height = e.Y - startY;
                currentRectangle = new Rectangle(startX, startY, width, height);
                borderColor = shiftPressed ? Color.Red : borderColor;
                Invalidate();
            }
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            if (drawingRectangle)
            {
                int width = e.X - startX;
                int height = e.Y - startY;
                rectangles.Add(new Rectangle(startX, startY, width, height));
                currentRectangle = Rectangle.Empty;
                Invalidate();
                drawingRectangle = false;
            }
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (rectangles.Count > 0)
            {
                undoStack.Push(new List<Rectangle>(rectangles));

                rectangles.RemoveAt(rectangles.Count - 1);
                Invalidate(); 
            }
        }

        private void redoCtrlAToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (redoStack.Count > 0)
            {
                undoStack.Push(new List<Rectangle>(rectangles));

                rectangles = redoStack.Pop();
                Invalidate();
            }
        }

        private void undo()
        {
            if (undoStack.Count > 0)
            {
                redoStack.Push(new List<Rectangle>(rectangles));

                rectangles = undoStack.Pop();
                Invalidate();
            }
        }

        private void deleteToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            rectangles.Clear();
            Invalidate();
        }

        private void blueToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            borderColor = Color.Blue;
        }

        private void pinkToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            borderColor = Color.Pink;
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e) { }

        private void menuStrip2_ItemClicked(object sender, ToolStripItemClickedEventArgs e) { }
    }
}
