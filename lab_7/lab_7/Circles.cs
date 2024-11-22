using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace lab_7
{
    public partial class MainForm : Form
    {
        private Thread t1, t2, t3;
        private Random r = new Random();
        private bool stopThreads = false;

        public MainForm()
        {
            InitializeComponent();
            this.FormClosing += MainForm_FormClosing; 
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            StopAllThreads();
        }

        private void DrawRed()
        {
            for (int i = 0; i < 100; i++)
            {
                if (stopThreads || this.IsDisposed) break;

                int x = r.Next(0, pictureBox1.Width);
                int y = r.Next(0, pictureBox1.Height);

                Invoke((MethodInvoker)(() =>
                {
                    if (!this.IsDisposed)
                    {
                        pictureBox1.CreateGraphics().DrawEllipse(new Pen(Color.Red, 3), x, y, 10, 10);
                    }
                }));

                Thread.Sleep(100);
            }
        }

        private void DrawGreen()
        {
            for (int i = 0; i < 100; i++)
            {
                if (stopThreads || this.IsDisposed) break;

                int x = r.Next(0, pictureBox2.Width);
                int y = r.Next(0, pictureBox2.Height);

                Invoke((MethodInvoker)(() =>
                {
                    if (!this.IsDisposed)
                    {
                        pictureBox2.CreateGraphics().DrawEllipse(new Pen(Color.Green, 3), x, y, 10, 10);
                    }
                }));

                Thread.Sleep(100);
            }
        }

        private void DrawBlue()
        {
            for (int i = 0; i < 100; i++)
            {
                if (stopThreads || this.IsDisposed) break;

                int x = r.Next(0, pictureBox3.Width);
                int y = r.Next(0, pictureBox3.Height);

                Invoke((MethodInvoker)(() =>
                {
                    if (!this.IsDisposed)
                    {
                        pictureBox3.CreateGraphics().DrawEllipse(new Pen(Color.Blue, 3), x, y, 10, 10);
                    }
                }));

                Thread.Sleep(100);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            StopAllThreads();
            stopThreads = false;
            t1 = new Thread(DrawRed);
            t1.Start();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            StopAllThreads();
            stopThreads = false;
            t1 = new Thread(DrawRed);
            t2 = new Thread(DrawGreen);
            t1.Start();
            t2.Start();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            StopAllThreads();
            stopThreads = false;
            t1 = new Thread(DrawRed);
            t2 = new Thread(DrawGreen);
            t3 = new Thread(DrawBlue);
            t1.Start();
            t2.Start();
            t3.Start();
        }

        private void MainForm_Load(object sender, EventArgs e) { }

        private void button4_Click(object sender, EventArgs e)
        {
            StopAllThreads();
        }

        private void StopAllThreads()
        {
            stopThreads = true;

            // Ожидание завершения потоков
            t1?.Join();
            t2?.Join();
            t3?.Join();

            // Очистка PictureBox'ов, если форма не закрыта
            if (!this.IsDisposed)
            {
                pictureBox1.Invoke((MethodInvoker)(() => pictureBox1.CreateGraphics().Clear(Color.White)));
                pictureBox2.Invoke((MethodInvoker)(() => pictureBox2.CreateGraphics().Clear(Color.White)));
                pictureBox3.Invoke((MethodInvoker)(() => pictureBox3.CreateGraphics().Clear(Color.White)));
            }
        }
    }
}
