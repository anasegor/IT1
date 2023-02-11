using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Numerics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Zadanie1_IT
{
    public partial class Form1 : Form
    {
        private double A1, A2, A3;
        private double v1, v2, v3;
        private double f1, f2, f3;
        private double f_d,a,j;
        private double Es;//энергия сигнала

        private int N;
        private int padding = 10;
        private int left_keys_padding = 20;

        private void pictureBox1_Resize(object sender, EventArgs e)
        {
            pictureBox1.Invalidate();
        }
        private double[] sign;
        private PointF[] func_points_before;
        private PointF[] func_points;
        private PointF[] A_points;
        private PointF[] func_points_restored;

       
        // private PointF[] rand_noise;

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            for_A1.Text = "1";
            for_A2.Text = "1";
            for_A3.Text = "1";
            for_v1.Text = "4";
            for_v2.Text = "6";
            for_v3.Text = "9";
            for_f1.Text = "0";
            for_f2.Text = "0";
            for_f3.Text = "0";
            for_f_d.Text = "23";
            for_N.Text = "8";
            for_a.Text = "0,1";
            for_j.Text = "0,8";


        }
        public Form1()
        {
            InitializeComponent();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = null;
            pictureBox1.Update();
            pictureBox2.Image = null;
            pictureBox2.Update();
            pictureBox3.Image = null;
            pictureBox3.Update();
            int wX2, hX2;
            int wX1, hX1;
            Graphics graphics1 = pictureBox1.CreateGraphics();
            Graphics graphics2 = pictureBox2.CreateGraphics();
            Graphics graphics3 = pictureBox3.CreateGraphics();
            Pen pen = new Pen(Color.DarkRed, 2f);
            Pen pen1 = new Pen(Color.Black, 2f);
            Pen pen2 = new Pen(Color.Blue, 2f);
            //graphics1.TranslateTransform(pictureBox1.Width / 2, pictureBox1.Height / 2);//центр
            //graphics1.ScaleTransform(1, -1); //перевернуть
            wX1 = pictureBox1.Width;
            hX1 = pictureBox1.Height;
            wX2 = pictureBox2.Width;
            hX2 = pictureBox2.Height;
            //координатные оси:
            Pen GreenPen;
            GreenPen = new Pen(Color.Black, 2f);
            //Ось X
            Point KX1, KX2;
            KX1 = new Point(30, (hX1/2 ));
            KX2 = new Point(wX1-10, (hX1/2 ) );
            graphics1.DrawLine(GreenPen, KX1, KX2);
            graphics3.DrawLine(GreenPen, KX1, KX2);
            Point KX12, KX22;
            KX12 = new Point(30, hX1 - 10);
            KX22 = new Point(wX1 - 10, hX1 - 10);
            graphics2.DrawLine(GreenPen, KX12, KX22);
            //Ось Y
            Point KY1, KY2;
            KY1 = new Point(30, 10);
            KY2 = new Point(30, hX1 - 10);
            graphics1.DrawLine(GreenPen, KY1, KY2);
            graphics3.DrawLine(GreenPen, KY1, KY2);
            graphics2.DrawLine(GreenPen, KY1, KY2);
            //сетка
            int actual_width1 = wX1 - 2 * padding - left_keys_padding;
            int actual_height1 = hX1 - 2 * padding;
            int actual_top = padding;
            int actual_bottom1 = actual_top + actual_height1;
            int actual_left = padding + left_keys_padding;
            int actual_right1 = actual_left + actual_width1;
            Pen GridPen= new Pen(Color.Gray, 1f);
            int grid_size = 11;
            float maxY = 0;
            SumSin(ref maxY);
            float maxYNew = 0;
            BDVPF(wX2, hX2);
            for (int i = 0; i < N; i++)
            {
                if (maxYNew < Math.Abs(func_points[i].Y)) maxYNew = Math.Abs(func_points_restored[i].Y);//макс значение Y
            }
            PointF K1, K2,K3,K4;
            for (double i = 0.5; i < grid_size; i += 1.0)
            {
                //вертикальная
                K1 = new PointF((float)(actual_left + i * actual_width1 / grid_size), actual_top);
                K2 = new PointF((float)(actual_left + i * actual_width1 / grid_size), actual_bottom1);
                graphics1.DrawLine(GridPen, K1, K2);
                graphics3.DrawLine(GridPen, K1, K2);
                double v = 0 + i * ((double)(N / f_d) - 0) / grid_size;
                string s1 = v.ToString("0.00");
                graphics1.DrawString(s1, new Font("Arial", 7), Brushes.Green, actual_left + (float)i * actual_width1 / grid_size, actual_bottom1 + 0);
                graphics3.DrawString(s1, new Font("Arial", 7), Brushes.Green, actual_left + (float)i * actual_width1 / grid_size, actual_bottom1 + 0);

                K3 = new PointF(actual_left, (float)(actual_top + i * actual_height1 / grid_size));
                K4 = new PointF(actual_right1, (float)(actual_top + i * actual_height1 / grid_size));
                double g = 0 + i * (double) (maxY / grid_size);
                string s2 = g.ToString("0.00");
                graphics1.DrawString(s2, new Font("Arial", 7), Brushes.Green, actual_left- left_keys_padding, actual_bottom1 - (float)i * actual_height1 / grid_size- hX1 / 2);//????
                graphics1.DrawLine(GridPen, K3, K4);
                double g3 = 0 + i * (double)(maxYNew / grid_size);
                string s3 = g.ToString("0.00");
                graphics3.DrawString(s3, new Font("Arial", 7), Brushes.Green, actual_left - left_keys_padding, actual_bottom1 - (float)i * actual_height1 / grid_size - hX1 / 2);//????
                graphics3.DrawLine(GridPen, K3, K4);
            }
            int actual_width2 = wX2 - 2 * padding - left_keys_padding; 
            int actual_height2 = hX2 - 2 * padding;
            int actual_bottom2 = actual_top + actual_height2;
            int actual_right2 = actual_left + actual_width2;
            PointF K5, K6, K7, K8;
            grid_size = 12;
            for (double i = 0.5; i < grid_size; i += 1.0)
            {

                K5 = new PointF((float)(actual_left + i * actual_width2 / grid_size), actual_top);
                K6 = new PointF((float)(actual_left + i * actual_width2 / grid_size), actual_bottom2);
                double v = 0 + i * ((double)(f_d) - 0) / grid_size;
                string s1 = v.ToString("0.00");
                graphics2.DrawString(s1, new Font("Arial", 7), Brushes.Green, actual_left + (float)i * actual_width1 / grid_size, actual_bottom1 + 0);
                graphics2.DrawLine(GridPen, K5, K6);

                K7 = new PointF(actual_left, (float)(actual_top + i * actual_height2 / grid_size));
                K8 = new PointF(actual_right2, (float)(actual_top + i * actual_height2 / grid_size));
                double g = 0 + i * (double)(maxY / grid_size);
                string s2 = g.ToString("0.00");
                graphics2.DrawString(s2, new Font("Arial", 7), Brushes.Green, actual_left - left_keys_padding, actual_bottom1 - (float)i * actual_height1 / grid_size);//???
                graphics2.DrawLine(GridPen, K7, K8);
            }
           
            PointF actual_tb = new PointF(actual_top, actual_bottom1);//для y
            PointF actual_rl = new PointF(actual_right1, actual_left);//для x
            PointF from_toX = new PointF(0, (float)(N / f_d));
            PointF from_toY = new PointF(-maxY * (float)1.2, maxY * (float)1.2);
            double d = 0;
            for (int i = 0; i < N; i++)
                d+=(func_points_restored[i].Y  - func_points_before[i].Y )*(func_points_restored[i].Y - func_points_before[i].Y);
            d = d / N;
            for_d.Text = d.ToString("F3");
            convert_range_graph(ref func_points, actual_rl, actual_tb, from_toX, from_toY);
            convert_range_graph(ref func_points_before, actual_rl, actual_tb, from_toX, from_toY);
            convert_range_graph(ref func_points_restored, actual_rl, actual_tb, from_toX, from_toY);
            graphics1.DrawLines(pen1, func_points);
            graphics2.DrawLines(pen, A_points);
            graphics3.DrawLines(pen2, func_points_before);//незашумленный сигнал
            graphics3.DrawLines(pen, func_points_restored);//восстановленный
            
        }
       

        public void SumSin(ref float maxY)
        {

            if (for_A1.Text != "" || for_A2.Text != "" || for_A3.Text != "")
            {
                A1 = Convert.ToDouble(for_A1.Text);
                A2 = Convert.ToDouble(for_A2.Text);
                A3 = Convert.ToDouble(for_A3.Text);
            } 
            else { MessageBox.Show("параметры A по умолчанию", "Внимание!"); }
            if (for_v1.Text != "" || for_v2.Text != "" || for_v3.Text != "")
            {
                v1 = Convert.ToDouble(for_v1.Text);
                v2 = Convert.ToDouble(for_v2.Text);
                v3 = Convert.ToDouble(for_v3.Text);
            }
            else { MessageBox.Show("параметры v по умолчанию", "Внимание!"); }
            if (for_f1.Text != "" || for_f2.Text != "" || for_f3.Text != "")
            {
                f1 = Convert.ToDouble(for_f1.Text);
                f2 = Convert.ToDouble(for_f2.Text);
                f3 = Convert.ToDouble(for_f3.Text);
            }
            else { MessageBox.Show("параметры f по умолчанию", "Внимание!"); }
            if (for_f_d.Text != "" || for_N.Text != "")
            {
                f_d = Convert.ToDouble(for_f_d.Text);
                N = Convert.ToInt32(for_N.Text);
                N = (int)Math.Pow(2, N);
                a = Convert.ToDouble(for_a.Text);
                j = Convert.ToDouble(for_j.Text);
            }
            else { MessageBox.Show("параметры f_d,N,a по умолчанию", "Внимание!"); }
            double[] rand_noise = new double[N];
            Random rnd = new Random();
            for (int i = 0; i < N; i++)//генерируем случайный шум
            {
                double sn = 0;
                for (int k = 0; k < 12; k++)
                {
                   
                    sn += 2*rnd.NextDouble()-1;
                }
                rand_noise[i] = sn;
            }
           
            sign = new double[N];
            double bufrn = 0;
            Es = 0;
            for (int i = 0; i < N; i++)
            {
                sign[i] = (float)(A1 * Math.Sin(2 * Math.PI * v1 * (float)i / f_d + f1) + A2 * Math.Sin(2 * Math.PI * v2 * (float)i / f_d + f2) + A3 * Math.Sin(2 * Math.PI * v3 * (float)i / f_d + f3));
                Es += sign[i] * sign[i];
                bufrn += rand_noise[i] * rand_noise[i];//энегрия сген шума
            }

            double b = Math.Sqrt(a*Es/bufrn);
            func_points = new PointF[N];
            func_points_before = new PointF[N];
            float dt = (float)(1 / f_d);
            for (int i=0; i<N; i++)
            {
                func_points_before[i] = new PointF((float)i * dt, (float)sign[i]);
                func_points[i] = new PointF((float)i*dt, (float)sign[i]+(float)b*(float)rand_noise[i]);
                if(maxY < Math.Abs(func_points[i].Y)) maxY=Math.Abs(func_points[i].Y);//макс значение Y
            }


        }
        public void BDVPF(int wX2, int hX2)
        {

            int actual_width = wX2 - 2 * padding - left_keys_padding;
            int actual_height = hX2 - 2 * padding;

            int actual_top = padding;
            int actual_bottom = actual_top + actual_height;
            int actual_left = padding + left_keys_padding;
            int actual_right = actual_left + actual_width;
            Cmplx[] arr = new Cmplx[N];
            for (int i = 0; i < N; i++)
            {
                arr[i] = new Cmplx(func_points[i].Y, 0);
            }
            Cmplx.Fourea(N, ref arr, -1);
            double En=0;//полная энергия зашумленного сигнала
            for (int i = 0; i < N ; i++)
            {
                En += (arr[i].re * arr[i].re + arr[i].im * arr[i].im) ; //определяем как квадрат амплитуды
            }
            double Esv = j * En;//предполагаемя энергия очищенного сигнала
            double E_before=0; //переменная для сравнения
            int k = 0;
            //Очищаем спектр
            do
            {
                E_before += (arr[k].re * arr[k].re + arr[k].im * arr[k].im) ;//считываем энергию с левого
                E_before +=(arr[N - 1 - k].re * arr[N - 1 - k].re + arr[N - 1 - k].im * arr[N - 1 - k].im); //и правого концов частотного графика параллельно
                k++;
            } while (E_before <= Esv);
            
            A_points = new PointF[N];
            float maxY = 0;
            float df = (float)f_d / (N - 1);
            for (int i = 0; i <= k; i++)
            {
                A_points[i] = new PointF((float)(i * df), (float)Math.Sqrt(arr[i].re * arr[i].re + arr[i].im * arr[i].im));
                if (maxY < Math.Abs(A_points[i].Y)) maxY = Math.Abs(A_points[i].Y);//макс значение Y
            }
            for (int i = k + 1; i < N - 1 - k; i++)
            {
                arr[i].re = 0;
                arr[i].im = 0;
                A_points[i] = new PointF((float)(i * df), (float)Math.Sqrt(arr[i].re * arr[i].re + arr[i].im * arr[i].im));
               
            }
            for (int i = N - 1 - k; i < N; i++)
            {
                A_points[i] = new PointF((float)(i * df), (float)Math.Sqrt(arr[i].re * arr[i].re + arr[i].im * arr[i].im));
                
            }
            //рисуем очищенный модуль спектра
            PointF actual_tb = new PointF(actual_top, actual_bottom);//для y
            PointF actual_rl = new PointF(actual_right, actual_left);//для x
            PointF from_toX = new PointF(0, (float)f_d);
            PointF from_toY = new PointF(0, maxY * (float)1.1);
            convert_range_graph(ref A_points, actual_rl, actual_tb, from_toX, from_toY);
            //обратное преобразование фурье
            //рисуем очищенный сигнал по ощиченному спектру
            Cmplx.Fourea(N, ref arr, 1);
            func_points_restored = new PointF[N];
            for (int i = 0; i < N; i++)
            {
                func_points_restored[i] = new PointF((float)(i / f_d), (float)arr[i].re);
            }

        }

        public void convert_range_graph(ref PointF[] data, PointF actual_rl, PointF actual_tb, PointF from_toX, PointF from_toY )
        {
           //actual-размер:X-top/right Y-right,left
           //from_to: X-мин, Y-макс
            float kx = (actual_rl.X - actual_rl.Y) / (from_toX.Y - from_toX.X);
            float ky = (actual_tb.X - actual_tb.Y) / (from_toY.Y - from_toY.X);
            for (int i=0; i< data.Length;i++)
           {
               data[i].X = (data[i].X - from_toX.X) * kx + actual_rl.Y;
               data[i].Y = (data[i].Y - from_toY.X) * ky + actual_tb.Y;
           }
        }
        public void convert_range_point(PointF data, PointF actual_rl, PointF actual_tb, PointF from_toX, PointF from_toY)
        {

            float kx = (actual_rl.X - actual_rl.Y) / (from_toX.Y - from_toX.X);
            float ky = (actual_tb.X - actual_tb.Y) / (from_toY.Y - from_toY.X);
            data.X = (data.X - from_toX.X) * kx + actual_rl.Y;
            data.Y = (data.Y - from_toY.X) * ky + actual_tb.Y;
            
        }
    }
    
    
}
