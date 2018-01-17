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
        static int rozmiarx = 0, rozmiary = 0, ziarna =0;
        int max = 0;
        int sasiad;
        int[][] tablica1;
        int[][] tablica2;
        int[] sasiedzi = new int[8];
        Bitmap flag;
        SolidBrush[] kolory;
        int index = 1;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            rozmiarx = int.Parse(textBox1.Text);
            rozmiary = int.Parse(textBox3.Text);
            flag = new Bitmap(10 * rozmiarx, 10 * rozmiary);

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
            //tuta
            ziarna = int.Parse(textBox2.Text);
            kolory = new SolidBrush[ziarna];
            Random rnd = new Random();          
            
            switch (comboBox2.SelectedIndex)
            {
                case 0:
                    for (int i = 0; i < ziarna; i++)
                        tablica1[rnd.Next(rozmiarx)][rnd.Next(rozmiary)] = i + 1;
                    break;
                case 1:
                    for (int i = 0; i < ziarna; i++)
                    {
                        
                    }
                        break;
                case 2:
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
            int[] count = new int[ziarna];

            for (int k = 0; k < ziarna; k++)
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
                        if (tablica[i][l] ==k) count[k]++;
                        if (tablica[g][j] ==k) count[k]++;
                        if (tablica[d][j] ==k) count[k]++;
                        if (tablica[i][p] ==k) count[k]++;

                        Random rnd = new Random();
                        int w = rnd.Next(2);
                        if (w == 1)
                        {
                            if (tablica[g][l] ==k) count[k]++;
                            if (tablica[d][p] ==k) count[k]++;
                        }
                        else
                        {
                            if (tablica[d][l] ==k) count[k]++;
                            if (tablica[g][p] ==k) count[k]++;
                        }
                        if (count[0] == 6) return 0;
                        break;
                }
                if (max < count[k]) max = k;
            }
            return max;
            
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            
            g = Graphics.FromImage(flag);
            for (int i = 0; i < rozmiarx ; i++)
            {
                for (int j = 0; j < rozmiary ; j++)
                {
                    sasiad = period(tablica1, rozmiarx, rozmiary, i, j);

                    if (sasiad != 0) {
                        if (tablica1[i][j] == sasiad)
                            tablica2[i][j] = sasiad;//podtrzymanie
                        else if(tablica1 [i][j]!=sasiad)     
                                if ( tablica1[i][j] == 0)
                                 tablica2[i][j] = sasiad;//ozywianie
                            else tablica2[i][j] = tablica1[i][j];
                     else
                        ;
                     } 

                }
            }Random rnd = new Random();
            for (int i = 0; i < rozmiarx ; i++)
            {
                for (int j = 0; j < rozmiary ; j++)
                {
                    for (int k = 0; k < ziarna; k++)
                        if (tablica1[i][j] == k) g.FillRectangle(kolory[k], i * 5, j * 5, 5, 5);
                        else;

                    //if (tablica1[i][j] == 1) g.FillRectangle(Brushes.Black, i * 10, j * 10, 10, 10);
                    //else g.FillRectangle(Brushes.White, i * 10, j * 10, 10, 10);
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

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            
            rozmiarx = int.Parse(textBox1.Text);
            rozmiary = int.Parse(textBox3.Text);
            this.Cursor = new Cursor(Cursor.Current.Handle);
            var mouseEventArgs = e as MouseEventArgs;
            int xC = mouseEventArgs.X;
            int yC = mouseEventArgs.Y;
            Random rnd = new Random();
            if (tablica1[xC / 5][yC / 5] == 0)
            {
                tablica1[xC / 5][yC / 5] = index;
                tablica2[xC / 5][yC / 5] = index;
                g.FillRectangle(new SolidBrush(Color.FromArgb(rnd.Next(255), rnd.Next(255), rnd.Next(255))), xC * 5, yC * 5, 5, 5);
            }
            index++;
            pictureBox1.Image = flag;

        }

        private void button4_Click(object sender, EventArgs e)
        {
            rozmiarx = int.Parse(textBox1.Text);
            rozmiary = int.Parse(textBox3.Text);
            flag = new Bitmap(10 * rozmiarx, 10 * rozmiary);
            g = Graphics.FromImage(flag);
            tablica1 = new int[rozmiarx + 1][];
            tablica2 = new int[rozmiarx + 1][];
            for (int i = 0; i < rozmiarx; i++)
            {
                tablica1[i] = new int[rozmiary + 1];
                tablica2[i] = new int[rozmiary + 1];
            }
            
            for (int i = 0; i < rozmiarx; i++)
            {
                for (int j = 0; j < rozmiary; j++)
                {
                    tablica1[i][j] = 0;
                    tablica2[i][j] = 0;
                    g.FillRectangle(new SolidBrush(Color.White), i * 5, j * 5, 5, 5);
                    
                }

            }
            pictureBox1.Image = flag;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            timer1.Interval -= 100;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            timer1.Interval += 100;
        }


    }
}
