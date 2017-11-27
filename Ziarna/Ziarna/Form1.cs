using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

using System.Windows.Forms;

namespace Ziarna
{
    public partial class Form1 : Form
    {
        Graphics g = null;
        Pen p = new Pen(Color.Black);
        SolidBrush b = new SolidBrush(Color.Black);
        static int rozmiarx = 0, rozmiary = 0, ziarna = 0;
        int max = 0, t=0;
        int sasiad;
        int[][] tablica1;
        int[][] tablica2;
        int[] sasiedzi = new int[8];
        Bitmap flag;
        SolidBrush[] kolory;
        int index = 1;
        int odstep;
        List<Point> punkty;
        Random rnd = new Random();
        public Form1()
        {
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e) //start/stop
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
                tworz(tablica1, rozmiarx, rozmiary);
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
                }
                if (max < count[k]) max = k;
            }
            return max;
        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {

            Point current = pictureBox1.PointToClient(Cursor.Position);
            int key = tablica1[current.X / 5][current.Y / 5];
            int q;
            int ziarna = int.Parse(textBox2.Text);
            for (int i = 0; i < rozmiarx; i++)
            {
                for (int j = 0; j < rozmiary; j++)
                    if (tablica1[i][j] == key)
                    {
                        tablica1[i][j] = 0;
                        tablica2[i][j] = 0;
                    }
            }
            DrawBoard();            
            pictureBox1.Refresh();
            for (int i = 0; i < rozmiarx; i++)
            {
                for (int j = 0; j < rozmiary; j++)
                {
                    if (tablica1[i][j] == 0)
                    {
                        q = rnd.Next(100);
                        if (q > 70)
                            tablica1[i][j] = rnd.Next(ziarna,1000);
                    }
                }
            }            
        }
        private void button4_Click(object sender, EventArgs e) //generate
        {
            rozmiarx = int.Parse(textBox1.Text);
            rozmiary = int.Parse(textBox3.Text);
            odstep = int.Parse(textBox4.Text);
            //flag = new Bitmap(10 * rozmiarx, 10 * rozmiary);
            flag = new Bitmap(pictureBox1.Width, pictureBox1.Height);

            tablica1 = new int[rozmiarx][];
            tablica2 = new int[rozmiarx][];
            for (int i = 0; i < rozmiarx; i++)
            {
                tablica1[i] = new int[rozmiary];
                tablica2[i] = new int[rozmiary];
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
            
            g = Graphics.FromImage(flag);
            for (int i = 0; i < rozmiarx; i++)
            {
                for (int j = 0; j < rozmiary; j++)
                {
                    g.FillRectangle(new SolidBrush(Color.White), i * 5, j * 5, 5, 5);

                }
            }
            pictureBox1.Image = flag;
                    for (int i = 0; i < ziarna - 1; i++)
                        tablica1[rnd.Next(rozmiarx)][rnd.Next(rozmiary)] = i + 1;
                    
            Random rand = new Random(314);
            for (int i = 0; i < ziarna; i++)
                kolory[i] = new SolidBrush(Color.FromArgb(rand.Next(255), rand.Next(255), rand.Next(255)));
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
            DrawBoard();
            pictureBox1.Image = flag;
            for (int i = 0; i < rozmiarx; i++)
            {
                for (int j = 0; j < rozmiary; j++)
                {
                    tablica1[i][j] = tablica2[i][j];
                }
            }
        }
        private void button2_Click(object sender, EventArgs e) //faster
        {
            timer1.Interval -= 99;
        }
        private void button3_Click(object sender, EventArgs e)//slower
        {
            timer1.Interval += 100;
        }
        private void button5_Click(object sender, EventArgs e)//circle inclusions
        {
            g = Graphics.FromImage(flag);
            tworz(tablica1, rozmiarx, rozmiary);
            Random rnd = new Random();
            Point o;
            int q = rnd.Next(ziarna);
            kolory[0] = b;
            int r = 0;
            for (int w = 0; w < q; w++)
            {
                r = rnd.Next(punkty.Count);
                o = punkty[r];
                punkty.RemoveAt(r);
                tablica1[o.X][o.Y] = 0;
                seed(tablica1, rozmiarx, rozmiary, o.X, o.Y, odstep);
                for (int i = 0; i < rozmiarx; i++)
                {
                    for (int j = 0; j < rozmiary; j++)
                    {
                        for (int k = 0; k < ziarna; k++)
                            if (tablica1[i][j] == k) g.FillRectangle(kolory[k], i * 5, j * 5, 5, 5);
                    }
                }
            }
            pictureBox1.Image = flag;
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
        private void timer2_Tick(object sender, EventArgs e)
        {
            g = Graphics.FromImage(flag);
            Random rnd = new Random();
            Point o;
            int r = 0;
            while (punkty.Count > 0)
            {
                r = rnd.Next(punkty.Count);
                o = punkty[r];
                punkty.RemoveAt(r);
                int energia = energy(tablica1, rozmiarx, rozmiary, o.X, o.Y);
                tablica2[o.X][o.Y] = neighbor(tablica1, rozmiarx, rozmiary, o.X, o.Y);
                int energia2 = energy(tablica2, rozmiarx, rozmiary, o.X, o.Y);
                if (energia < energia2)
                {
                    tablica2[o.X][o.Y] = tablica1[o.X][o.Y];
                }
                tablica1[o.X][o.Y] = tablica2[o.X][o.Y];
            }
            for (int i = 0; i < rozmiarx; i++)
            {
                for (int j = 0; j < rozmiary; j++)
                {
                    for (int k = 0; k < ziarna; k++)
                        if (tablica1[i][j] == k) g.FillRectangle(kolory[k], i * 5, j * 5, 5, 5);
                }
            }
            pictureBox1.Image = flag;
            t++;
            tworz(tablica1, rozmiarx, rozmiary);
            //label7.Text = t.ToString();


        }
        private void seed(int[][] tablica, int rozmiarx, int rozmiary, int i, int j, int radius) //create circles
        {
            
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
                for (int l = w; l < r; l++)
                {
                    if ((Math.Abs(Math.Sqrt(Convert.ToDouble(Math.Pow(l - j, 2)) + Convert.ToDouble(Math.Pow(k - i, 2)))) <= radius + 1))
                        tablica[k][l] = 0;
                        //g.FillRectangle(kolory[0], k * 5, l * 5, 5, 5);
                }
            }
        }
        private void button6_Click(object sender, EventArgs e) //draw boundaries
        {
            /*if (!timer2.Enabled)
            {
                timer2.Start();
                button6.Text = "Stop";
            }
            else
            {
                timer2.Stop();
                button6.Text = "MonteCarlo";
                tworz(tablica1, rozmiarx, rozmiary);
            }*/
            tworz(tablica1, rozmiarx, rozmiary);
            foreach (Point cell in punkty) 
                g.FillRectangle(b, cell.X * 5, cell.Y * 5, 5, 5);
            pictureBox1.Refresh();
        }
        private void importToolStripMenuItem_Click(object sender, EventArgs e) //Import txt
        {
           
            OpenFileDialog theDialog = new OpenFileDialog();
            theDialog.Title = "Open Text File";
            theDialog.Filter = "TXT files|*.txt";
            //theDialog.InitialDirectory = @"";
            if (theDialog.ShowDialog() == DialogResult.OK)
            {
                StreamReader reader = new StreamReader(theDialog.OpenFile());
                using (reader)
                {
                    //Console.WriteLine(reader.ReadLine());
                    string text = reader.ReadLine();
                    string[] bits = text.Split(' ');
                    rozmiarx = int.Parse(bits[0]);
                    rozmiary = int.Parse(bits[1]);

                    int w, r;

                    for (int q = 0; q < rozmiarx * rozmiary; q++)
                    {
                        text = reader.ReadLine();
                        bits = text.Split(' ');
                        w = int.Parse(bits[0]);
                        r = int.Parse(bits[1]);
                        tablica1[w][r] = int.Parse(bits[2]);
                    }
                    for (int i = 0; i < rozmiarx; i++)
                    {
                        for (int j = 0; j < rozmiary; j++)
                            tablica2[i][j] = 0;
                    }

                    flag = new Bitmap(pictureBox1.Width, pictureBox1.Height);
                    g = Graphics.FromImage(flag);
                    g.FillRectangle(new SolidBrush(Color.White), 0, 0, rozmiarx * 5, rozmiary * 5);
                    pictureBox1.Image = flag;
                }
            }
        }
        private void exportToolStripMenuItem_Click(object sender, EventArgs e) //export txt
        {
            SaveFileDialog save = new SaveFileDialog();
            save.FileName = "DefaultOutputName.txt";
            save.Filter = "Text File | *.txt";

            if (save.ShowDialog() == DialogResult.OK)
                using (StreamWriter writer =
            new StreamWriter(save.OpenFile()))
            {
                writer.WriteLine(rozmiarx + " " + rozmiary);
                for (int i=0; i<rozmiarx; i++)
                {
                    for(int j = 0; j < rozmiary; j++)
                    {
                        writer.WriteLine(i + " " + j + " " + tablica1[i][j]);
                    }
                }               
            }
        }
        private void saveToolStripMenuItem_Click(object sender, EventArgs e) //import bmp
        {
            SaveFileDialog save = new SaveFileDialog();
            save.FileName = "DefaultOutputName.bmp";
            save.Filter = "Bitmap | *.bmp";
            ImageFormat format = ImageFormat.Bmp;

            if (save.ShowDialog() == DialogResult.OK)
                pictureBox1.Image.Save(save.FileName, format);
        }
        private void loadToolStripMenuItem_Click(object sender, EventArgs e) //export bmp
        {
            OpenFileDialog theDialog = new OpenFileDialog();
            theDialog.Title = "Open BMP File";
            theDialog.Filter = "Bitmaps|*.bmp";
            //theDialog.InitialDirectory = @"";
            if (theDialog.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Image = new Bitmap(theDialog.OpenFile());
            }
        }
        private void tworz(int[][] tablica, int rozmiarx, int rozmiary) //lista granic ziaren
        { 
            punkty = new List<Point>();
            int l, p, g, d;
            
            for (int i = 1; i < rozmiarx-1; i++)
            {
                for (int j = 1; j < rozmiary-1; j++)
                    {
                    l = j - 1;
                    p = j + 1;
                    g = i - 1;
                    d = i + 1;
                    if (j == 0) l = rozmiary - 1;
                    if (i == 0) g = rozmiarx - 1;
                    if (j == rozmiary - 1) p = 0;
                    if (i == rozmiarx - 1) d = 0;
                    if (tablica[g][l] != tablica[i][j])
                    {
                        punkty.Add(new Point(i, j));
                        continue;
                    }
                    if (tablica[g][j] != tablica[i][j]) { 
                        punkty.Add(new Point(i, j));
                        continue;
                    }
                    if (tablica[g][p] != tablica[i][j]) { 
                        punkty.Add(new Point(i, j));
                        continue;
                    }
                    if (tablica[i][l] != tablica[i][j]) { 
                        punkty.Add(new Point(i, j));
                        continue;
                    }
                    if (tablica[i][p] != tablica[i][j]) { 
                        punkty.Add(new Point(i, j));
                        continue;
                    }
                    if (tablica[d][l] != tablica[i][j]) { 
                        punkty.Add(new Point(i, j));
                        continue;
                    }
                    if (tablica[d][j] != tablica[i][j]) { 
                        punkty.Add(new Point(i, j));
                        continue;
                    }
                    if (tablica[d][p] != tablica[i][j]) { 
                        punkty.Add(new Point(i, j));
                        continue;
                    }
                }
            }
        }
        private int energy(int[][] tablica, int rozmiarx, int rozmiary, int i, int j)
        {
            int energy = 0;
            int l, p, g, d;
            l = j - 1;
            p = j + 1;
            g = i - 1;
            d = i + 1;
            if (j == 0) l = rozmiarx - 1;
            if (i == 0) g = rozmiary - 1;
            if (j == rozmiarx - 1) p = 0;
            if (i == rozmiary - 1) d = 0;
            if (tablica[g][l] != tablica[i][j]) energy++;
            if (tablica[g][j] != tablica[i][j]) energy++;
            if (tablica[g][p] != tablica[i][j]) energy++;//gorny rzad
            if (tablica[i][l] != tablica[i][j]) energy++;
            if (tablica[i][p] != tablica[i][j]) energy++;//srodek
            if (tablica[d][l] != tablica[i][j]) energy++;
            if (tablica[d][j] != tablica[i][j]) energy++;
            if (tablica[d][p] != tablica[i][j]) energy++;//dolny rzad

            return energy;
        }
        private void button7_Click(object sender, EventArgs e)//square inclusions
        {
            g = Graphics.FromImage(flag);
            tworz(tablica1, rozmiarx, rozmiary);
            Random rnd = new Random();
            Point o;
            int q = rnd.Next(ziarna);
            kolory[0] = b;
            int r = 0;
            for (int w = 0; w < q; w++)
            {
                r = rnd.Next(punkty.Count);
                o = punkty[r];
                punkty.RemoveAt(r);
                tablica1[o.X][o.Y] = 0;
                square(tablica1, rozmiarx, rozmiary, o.X, o.Y, odstep);
                DrawBoard();
            }
            pictureBox1.Image = flag;
        }
        private int neighbor(int[][] tablica, int rozmiarx, int rozmiary, int i, int j)
        {
            int result = 0;
            int l, p, g, d;
            l = j - 1;
            p = j + 1;
            g = i - 1;
            d = i + 1;
            if (j == 0) l = rozmiarx - 1;
            if (i == 0) g = rozmiary - 1;
            if (j == rozmiarx - 1) p = 0;
            if (i == rozmiary - 1) d = 0;
            int[] neighbor = new int[8];
            neighbor[0] = tablica[g][l];
            neighbor[1] = tablica[g][j];
            neighbor[2] = tablica[g][p];
            neighbor[3] = tablica[i][l];
            neighbor[4] = tablica[i][p];
            neighbor[5] = tablica[d][l];
            neighbor[6] = tablica[d][j];
            neighbor[7] = tablica[d][p];
            Random rnd = new Random();
            result = neighbor[rnd.Next(8)];

            return result;
        }
        private int rules(int[][] tablica, int rozmiarx, int rozmiary, int i, int j)
        {
            int q = 0;
            max = 0;
            int l, p, g, d;
            l = j - 1;
            p = j + 1;
            g = i - 1;
            d = i + 1;
            if (j == 0) l = rozmiary - 1;
            if (i == 0) g = rozmiarx - 1;
            if (j == rozmiary - 1) p = 0;
            if (i == rozmiarx - 1) d = 0;//periodic boundary
            int P = int.Parse(textBox5.Text);
            int[] count = new int[ziarna + 1];
            for (int k = 0; k < ziarna + 1; k++)
            {
                if (tablica[g][l] == k) count[k]++;//moore neighborhood
                if (tablica[d][l] == k) count[k]++;
                if (tablica[g][p] == k) count[k]++;
                if (tablica[d][p] == k) count[k]++;
                if (tablica[i][l] == k) count[k]++;
                if (tablica[g][j] == k) count[k]++;
                if (tablica[d][j] == k) count[k]++;
                if (tablica[i][p] == k) count[k]++;                 
            }
            for (int k = 0; k < ziarna + 1; k++)//rule 1
                if (count[k] > 4) max = k;
            if (max == 0)
            {
                for (int k = 0; k < ziarna + 1; k++)
                {
                    if (tablica[i][l] == k) count[k]++;//von neumann
                    if (tablica[g][j] == k) count[k]++;
                    if (tablica[d][j] == k) count[k]++;
                    if (tablica[i][p] == k) count[k]++;
                }
                for (int k = 0; k < ziarna + 1; k++)//rule 2
                    if (count[k] > 2) max = k;
                if (max == 0)
                {
                    for (int k = 0; k < ziarna + 1; k++)
                    {
                        if (tablica[g][l] == k) count[k]++;//reverse von neumann
                        if (tablica[d][l] == k) count[k]++;
                        if (tablica[g][p] == k) count[k]++;
                        if (tablica[d][p] == k) count[k]++;
                    }
                    for (int k = 0; k < ziarna + 1; k++)//rule 3
                        if (count[k] > 2) max = k;
                    if (max == 0)
                    {   Random rnd = new Random();
                        for (int k = 0; k < ziarna + 1; k++)
                        {
                            if (tablica[g][l] == k) count[k]++;//moore neighborhood
                            if (tablica[d][l] == k) count[k]++;
                            if (tablica[g][p] == k) count[k]++;
                            if (tablica[d][p] == k) count[k]++;
                            if (tablica[i][l] == k) count[k]++;
                            if (tablica[g][j] == k) count[k]++;
                            if (tablica[d][j] == k) count[k]++;
                            if (tablica[i][p] == k) count[k]++;
                            q = rnd.Next(101);                        
                            if(q>P)max = k;                            
                        }   
                    }
                }
            }
            return max;
        }
        private void DrawBoard()
        {
            for (int i = 0; i < rozmiarx; i++)
            {
                for (int j = 0; j < rozmiary; j++)
                {
                    for (int k = 0; k < ziarna; k++)
                        if (tablica1[i][j] == k) g.FillRectangle(kolory[k], i * 5, j * 5, 5, 5);
                }
            }
        }
        private void square(int[][] tablica, int rozmiarx, int rozmiary, int i, int j, int radius)//create squares
        {

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
                for (int l = w; l < r; l++)
                {                    
                        tablica[k][l] = 0;
                    //g.FillRectangle(kolory[0], k * 5, l * 5, 5, 5);
                }
            }
        }
    }

}
