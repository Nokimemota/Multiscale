using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ziarna
{
    public partial class Form1 : Form
    {
        Graphics g = null;
        Pen p = new Pen(Color.Black);
        SolidBrush b = new SolidBrush(Color.Black);
        static int rozmiarx = 0, rozmiary = 0, ziarna = 0;
        int max = 0;
        int sasiad;
        int[][] tablica1;
        int[][] tablica2;
        int[] sasiedzi = new int[8];
        Bitmap flag;
        SolidBrush[] kolory;
        int index = 1;
        int odstep;
        List<Point> punkty;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!timer1.Enabled)
            {
                timer1.Start();
                button1.Text = "Stop";
            }
            else
            {
                timer1.Stop();
                button1.Text = "Start";
            }
        }

        private int period(int[][] tablica, int rozmiarx, int rozmiary, int i, int j)
        {
            max = 0;
            int l, p, g, d;
            l = j - 1;
            p = j + 1;
            g = i - 1;
            d = i + 1;
            if (j == 0) l = rozmiary - 1;
            if (i == 0) g = rozmiarx - 1;
            if (j == rozmiary - 1) p = 0;
            if (i == rozmiarx - 1) d = 0;
            int[] count = new int[ziarna + 1];

            for (int k = 0; k < ziarna + 1; k++)
            {
                switch (comboBox1.SelectedIndex)
                {
                    case 0:
                        if (tablica[g][l] == k) count[k]++;
                        if (tablica[d][l] == k) count[k]++;
                        if (tablica[g][p] == k) count[k]++;
                        if (tablica[d][p] == k) count[k]++;
                        if (tablica[i][l] == k) count[k]++;
                        if (tablica[g][j] == k) count[k]++;
                        if (tablica[d][j] == k) count[k]++;
                        if (tablica[i][p] == k) count[k]++;
                        if (count[0] == 8) return 0;
                        break;
                    case 1:
                        if (tablica[i][l] == k) count[k]++;
                        if (tablica[g][j] == k) count[k]++;
                        if (tablica[d][j] == k) count[k]++;
                        if (tablica[i][p] == k) count[k]++;
                        if (count[0] == 4) return 0;
                        break;
                    case 2:
                        if (tablica[g][l] == k) count[k]++;
                        if (tablica[d][p] == k) count[k]++;
                        if (tablica[i][l] == k) count[k]++;
                        if (tablica[g][j] == k) count[k]++;
                        if (tablica[d][j] == k) count[k]++;
                        if (tablica[i][p] == k) count[k]++;
                        if (count[0] == 6) return 0;
                        break;
                    case 3:
                        if (tablica[d][l] == k) count[k]++;
                        if (tablica[g][p] == k) count[k]++;
                        if (tablica[i][l] == k) count[k]++;
                        if (tablica[g][j] == k) count[k]++;
                        if (tablica[d][j] == k) count[k]++;
                        if (tablica[i][p] == k) count[k]++;
                        if (count[0] == 6) return 0;
                        break;
                    case 4:
                        if (tablica[i][l] == k) count[k]++;
                        if (tablica[g][j] == k) count[k]++;
                        if (tablica[d][j] == k) count[k]++;
                        if (tablica[i][p] == k) count[k]++;

                        Random rnd = new Random();
                        int w = rnd.Next(2);
                        if (w == 1)
                        {
                            if (tablica[g][l] == k) count[k]++;
                            if (tablica[d][p] == k) count[k]++;
                        }
                        else
                        {
                            if (tablica[d][l] == k) count[k]++;
                            if (tablica[g][p] == k) count[k]++;
                        }
                        if (count[0] == 6) return 0;
                        break;
                }
                if (max < count[k]) max = k;
            }
            return max;

        }


        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (comboBox2.SelectedIndex == 2)
            {

                this.Cursor = new Cursor(Cursor.Current.Handle);
                var mouseEventArgs = e as MouseEventArgs;
                int xC = mouseEventArgs.X;
                int yC = mouseEventArgs.Y;
                Random rnd = new Random();

                if (tablica1[xC / 5][yC / 5] == 0)
                {
                    tablica1[xC / 5][yC / 5] = index;
                    tablica2[xC / 5][yC / 5] = index;
                    g.FillRectangle(kolory[index], xC - 5, yC - 5, 5, 5);
                }
                index++;
                pictureBox1.Image = flag;
            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            rozmiarx = int.Parse(textBox1.Text);
            rozmiary = int.Parse(textBox3.Text);
            odstep = int.Parse(textBox4.Text);
            //flag = new Bitmap(10 * rozmiarx, 10 * rozmiary);
            flag = new Bitmap(pictureBox1.Width, pictureBox1.Height);

            tablica1 = new int[rozmiarx + 1][];
            tablica2 = new int[rozmiarx + 1][];
            for (int i = 0; i < rozmiarx; i++)
            {
                tablica1[i] = new int[rozmiary + 1];
                tablica2[i] = new int[rozmiary + 1];
            }
            sasiad = 0;
            for (int i = 0; i < rozmiarx; i++)
            {
                for (int j = 0; j < rozmiary; j++)
                {
                    tablica1[i][j] = 0;
                    tablica2[i][j] = 0;
                }
            }
            ziarna = int.Parse(textBox2.Text);
            ziarna++;
            kolory = new SolidBrush[ziarna];
            Random rnd = new Random();
            g = Graphics.FromImage(flag);
            for (int i = 0; i < rozmiarx; i++)
            {
                for (int j = 0; j < rozmiary; j++)
                {
                    g.FillRectangle(new SolidBrush(Color.White), i * 5, j * 5, 5, 5);

                }

            }
            pictureBox1.Image = flag;
            switch (comboBox2.SelectedIndex)
            {
                case 0:
                    for (int i = 0; i < ziarna - 1; i++)
                        tablica1[rnd.Next(rozmiarx)][rnd.Next(rozmiary)] = i + 1;
                    break;
                case 1:
                    int a = 1;
                    for (int i = 0; i < rozmiarx; i += odstep - 1)
                    {
                        for (int j = 0; j < rozmiary; j += odstep - 1)
                        {
                            tablica1[i][j] = a;
                            a++;

                        }

                    }
                    kolory = new SolidBrush[a];
                    ziarna = a;
                    break;
                case 2:
                    index = 1;
                    break;
                case 3:
                    double wspol = 1, yc;
                    int xc;
                    for (int i = 0; i < ziarna - 1; i++) {
                        xc = rnd.Next(rozmiarx);
                        yc = rnd.Next(rozmiary) / wspol;
                        tablica1[xc][(int)yc] = i + 1;
                        wspol += 0.01;
                    }
                    break;
                case 4:
                    int xb, yb, z = 0;
                    for (int i = 0; i < ziarna; i++)
                    {
                        if (z > 5) break;
                        xb = rnd.Next(rozmiarx);
                        yb = rnd.Next(rozmiary);
                        if (radius(tablica1, rozmiarx, rozmiary, xb, yb, odstep * 5))
                        {
                            tablica1[xb][yb] = i + 1;
                            //g.DrawEllipse(p, (xb * 5) - odstep, (yb * 5) - odstep, odstep + odstep, odstep + odstep);
                            //g.DrawLine(p, xb * 5, yb * 5, xb * 5 + 1, yb * 5 + 1);
                            z = 0;
                        }
                        else
                        {
                            System.Console.WriteLine("error");
                            i--;
                            z++;
                        }
                    }

                    break;

            }
            for (int i = 0; i < ziarna; i++)
                kolory[i] = new SolidBrush(Color.FromArgb(rnd.Next(255), rnd.Next(255), rnd.Next(255)));

            kolory[0] = new SolidBrush(Color.White);
            for (int i = 0; i < rozmiarx; i++)
            {
                for (int j = 0; j < rozmiary; j++)
                {
                    tablica2[i][j] = tablica1[i][j];
                }
            }
            pictureBox1.Image = flag;
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            //flag = new Bitmap(10 * pictureBox1.Width, 10 * pictureBox1.Height);
            g = Graphics.FromImage(flag);

            for (int i = 0; i < rozmiarx; i++)
            {
                for (int j = 0; j < rozmiary; j++)
                {
                    sasiad = period(tablica1, rozmiarx, rozmiary, i, j);

                    if (sasiad != 0) {
                        if (tablica1[i][j] == sasiad)
                            tablica2[i][j] = sasiad;//podtrzymanie
                        else if (tablica1[i][j] != sasiad)
                            if (tablica1[i][j] == 0)
                                tablica2[i][j] = sasiad;//ozywianie
                            else tablica2[i][j] = tablica1[i][j];
                    }

                }
            } Random rnd = new Random();
            for (int i = 0; i < rozmiarx; i++)
            {
                for (int j = 0; j < rozmiary; j++)
                {
                    for (int k = 0; k < ziarna; k++)
                        if (tablica1[i][j] == k) g.FillRectangle(kolory[k], i * 5, j * 5, 5, 5);


                }
            }
            pictureBox1.Image = flag;
            for (int i = 0; i < rozmiarx; i++)
            {
                for (int j = 0; j < rozmiary; j++)
                {
                    tablica1[i][j] = tablica2[i][j];
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            timer1.Interval -= 100;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            timer1.Interval += 100;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            tworz(tablica1, rozmiarx, rozmiary);
            Random rnd = new Random();
            Point o;
            o = punkty[rnd.Next(punkty.Count)];
            tablica1[o.X][o.Y] = 0;
            if(!radius(tablica1,rozmiarx,rozmiary,o.X,o.Y,odstep))



        }

        private bool radius(int[][] tablica, int rozmiarx, int rozmiary, int i, int j, int radius) {
            int q, w, e, r;
            e = i + radius;
            r = j + radius;
            q = i - radius;
            w = j - radius;

            if (q < 0) q = 0;
            if (w < 0) w = 0;
            if (e > rozmiarx) e = rozmiarx;
            if (r > rozmiary) r = rozmiary;

            for (int k = q; k < e; k++)
            {
                for (int l = w; l < r; l++) {
                    if ((Math.Abs(Math.Sqrt(Convert.ToDouble(Math.Pow(l - j, 2)) + Convert.ToDouble(Math.Pow(k - i, 2)))) <= radius + 1))
                        return true;
                }
            }
            return false;
        }
        private void tworz(int[][] tablica, int rozmiarx, int rozmiary) {
            punkty = new List<Point>();
            for (int i = 1; i < rozmiarx-1; i++)
            {
                for (int j = 1; j < rozmiary-1; j++)
                {
                    if (tablica[i - 1][j - 1] != tablica[i][j])
                    {
                        punkty.Add(new Point(i, j));
                        continue;
                    }
                    if (tablica[i - 1][j] != tablica[i][j]) { 
                        punkty.Add(new Point(i, j));
                        continue;
                    }
                    if (tablica[i - 1][j + 1] != tablica[i][j]) { 
                        punkty.Add(new Point(i, j));
                        continue;
                    }
                    if (tablica[i][j - 1] != tablica[i][j]) { 
                        punkty.Add(new Point(i, j));
                        continue;
                    }
                    if (tablica[i][j + 1] != tablica[i][j]) { 
                        punkty.Add(new Point(i, j));
                        continue;
                    }
                    if (tablica[i + 1][j - 1] != tablica[i][j]) { 
                        punkty.Add(new Point(i, j));
                        continue;
                    }
                    if (tablica[i + 1][j] != tablica[i][j]) { 
                        punkty.Add(new Point(i, j));
                        continue;
                    }
                    if (tablica[i + 1][j + 1] != tablica[i][j]) { 
                        punkty.Add(new Point(i, j));
                        continue;
                    }
                }
            }
        }
    }
}
