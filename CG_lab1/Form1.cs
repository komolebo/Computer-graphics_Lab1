using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;   // для оцінки швидкодії

namespace CG_lab1
{
    public partial class Form1 : Form
    {
        const int N = 5;    // N x N псевдопіксель
        public Form1()
        {
            InitializeComponent();
            using (StreamWriter sw = new StreamWriter("result.txt", false, System.Text.Encoding.Default))
            {
                sw.WriteLine("Start!\n");
            }
        }
        public void putPixel(int x, int y) // малює псевдопіксель, розміром N x N, в позиції (x, y)
        {
            Graphics g = Graphics.FromHwnd(pictureBox1.Handle);    // оголошуємо об'єкт "g" класа Graphics і даємо йому можливість малювання на pictureBox1
            SolidBrush myPixel = new SolidBrush(Color.Black);   // створюємо об'єкт для замальовування фігур
            g.FillRectangle(myPixel, x, y, N, N); // малюємо псевдопіксель
        }
        public void SimpleDDA(int n, int x_st, int y_st, int x_fin, int y_fin)  // вектор за алгоритмом простого ЦДА
        {
            putPixel(x_st, y_st);    // початкова точка
            int x_prev = x_st, y_prev = y_st, d_x = x_fin - x_st, d_y = y_fin - y_st;
            for (int i = 0; i < n; i++) // n - кількість вузлів, використовуємих для апроксимації відрізка
            {
                int x = x_prev + d_x / n, y = y_prev + d_y / n; // x та y - початкові значення координат для наступного кроку по відрізку
                putPixel(x, y);
                x_prev = x;
                y_prev = y;
            }
        }
        public void AsymmetricalDDA(int x_st, int y_st, int x_fin, int y_fin)   // вектор за алгоритмом несиметричного ЦДА
        {
            int dx, dy, s;
            dx = x_fin - x_st; // вираховуємо прирощення
            dy = y_fin - y_st;
            if (dx >= 0 && dy >= 0) // малюємо в першому октанті
            {
                putPixel(x_st, y_st);    // малюємо початкову точку вектора
                dx += N;    // вираховуємо кількість позицій по X та Y
                dy += N;
                if (dy == dx)   // генерація вектора (нахил 45 градусів)
                {
                    while (x_st < x_fin)
                    {
                        x_st += N;
                        putPixel(x_st, x_st);
                    }
                }
                else if (dx > dy)   // нахил < 45 градусів
                {
                    s = 0;
                    while (x_st < x_fin)
                    {
                        x_st += N;
                        s += dy;
                        if (s >= dx)
                        {
                            s -= dx;
                            y_st += N;
                        }
                        putPixel(x_st, y_st);
                    }
                }
                else    // нахил > 45 градусів
                {
                    s = 0;
                    while (y_st < y_fin)
                    {
                        y_st += N;
                        s += dx;
                        if (s >= dy)
                        {
                            s -= dy;
                            x_st += N;
                        }
                        putPixel(x_st, y_st);
                    }
                }
            }
            else if (dx <= 0 && dy >= 0)    // малюємо в другому октанті
            {
                putPixel(x_st, y_st);    // малюємо початкову точку вектора
                dx -= N;    // вираховуємо кількість позицій по X та Y
                dy += N;
                if (dy == Math.Abs(dx))   // генерація вектора (нахил 45 градусів)
                {
                    while (x_st > x_fin)
                    {
                        x_st -= N;
                        putPixel(x_st, x_st);
                    }
                }
                else if (Math.Abs(dx) > dy)   // нахил < 45 градусів
                {
                    s = 0;
                    while (x_st > x_fin)
                    {
                        x_st -= N;
                        s += dy;
                        if (s >= Math.Abs(dx))
                        {
                            s -= Math.Abs(dx);
                            y_st += N;
                        }
                        putPixel(x_st, y_st);
                    }
                }
                else    // нахил > 45 градусів
                {
                    s = 0;
                    while (y_st < y_fin)
                    {
                        y_st += N;
                        s += Math.Abs(dx);
                        if (s >= dy)
                        {
                            s -= dy;
                            x_st -= N;
                        }
                        putPixel(x_st, y_st);
                    }
                }
            }
            else if (dx <= 0 && dy <= 0)    // малюємо в третьому октанті
            {
                putPixel(x_st, y_st);    // малюємо початкову точку вектора
                dx -= N;    // вираховуємо кількість позицій по X та Y
                dy -= N;
                if (dy == dx)   // генерація вектора (нахил 45 градусів)
                {
                    while (x_st > x_fin)
                    {
                        x_st -= N;
                        putPixel(x_st, x_st);
                    }
                }
                else if (dx < dy)   // нахил < 45 градусів
                {
                    s = 0;
                    while (x_st > x_fin)
                    {
                        x_st -= N;
                        s += Math.Abs(dy);
                        if (s >= Math.Abs(dx))
                        {
                            s -= Math.Abs(dx);
                            y_st -= N;
                        }
                        putPixel(x_st, y_st);
                    }
                }
                else    // нахил > 45 градусів
                {
                    s = 0;
                    while (y_st > y_fin)
                    {
                        y_st -= N;
                        s += Math.Abs(dx);
                        if (s >= Math.Abs(dy))
                        {
                            s -= Math.Abs(dy);
                            x_st -= N;
                        }
                        putPixel(x_st, y_st);
                    }
                }
            }
            else    // малюємо в четвертому октанті
            {
                putPixel(x_st, y_st);    // малюємо початкову точку вектора
                dx += N;    // вираховуємо кількість позицій по X та Y
                dy -= N;
                if (Math.Abs(dy) == dx)   // генерація вектора (нахил 45 градусів)
                {
                    while (x_st < x_fin)
                    {
                        x_st += N;
                        putPixel(x_st, x_st);
                    }
                }
                else if (dx < Math.Abs(dy))   // нахил < 45 градусів
                {
                    s = 0;
                    while (x_st < x_fin)
                    {
                        x_st += N;
                        s += Math.Abs(dy);
                        if (s >= dx)
                        {
                            s -= dx;
                            y_st -= N;
                        }
                        putPixel(x_st, y_st);
                    }
                }
                else    // нахил > 45 градусів
                {
                    s = 0;
                    while (y_st > y_fin)
                    {
                        y_st -= N;
                        s += dx;
                        if (s >= Math.Abs(dy))
                        {
                            s -= Math.Abs(dy);
                            x_st += N;
                        }
                        putPixel(x_st, y_st);
                    }
                }
            }
        }
        public void AlgorithmBrezenhema(int x_st, int y_st, int x_fin, int y_fin)   // вектор за алгоритмом Брезенхема
        {
            int dx, dy, s, sx, sy, kl, swap, incr1, incr2;
            // обчислюємо прирощення і кроки
            sx = 0;
            if ((dx = x_fin - x_st) < 0)
            {
                dx = -dx;
                --sx;
            }
            else if (dx > 0)
                ++sx;
            sy = 0;
            if ((dy = y_fin - y_st) < 0)
            {
                dy = -dy;
                --sy;
            }
            else if (dy > 0)
                ++sy;
            // враховуємо нахил
            swap = 0;
            if ((kl = dx) < (s = dy))
            {
                dx = s;
                dy = kl;
                kl = s;
                swap++;
            }
            s = (incr1 = 2 * dy) - dx;  // incr1 - константа переобчислення різниці якщо поточне s < 0 і s - початкове значення різниці
            incr2 = 2 * dx; // константа для переобчислення різниці якщо поточне s >= 0
            putPixel(x_st, y_st);    // перший псевдопіксель вектора
            while ((kl -= N) >= 0)
            {
                if (s >= 0)
                {
                    if (swap != 0)
                        x_st += (sx * N);
                    else
                        y_st += (sy * N);
                    s -= incr2;
                }
                if (swap != 0)
                    y_st += (sy * N);
                else
                    x_st += (sx * N);
                s += incr1;
                putPixel(x_st, y_st);    // поточна точка вектора
            }
        }
        public void AlgorithmBrezenhema_Circle(int x_st, int y_st, int R)   // коло за алгоритмом Брезенхема
        {
            int x, y, d;
            x = 0;
            y = R;
            d = 3 - 2 * y;
            while (x <= y)
            {
                putPixel(x + x_st, y + y_st);
                putPixel(x + x_st, -y + y_st);
                putPixel(-x + x_st, -y + y_st);
                putPixel(-x + x_st, y + y_st);
                putPixel(y + x_st, x + y_st);
                putPixel(y + x_st, -x + y_st);
                putPixel(-y + x_st, -x + y_st);
                putPixel(-y + x_st, x + y_st);
                if (d < 0)
                    d = d + 4 * x + 6;
                else
                {
                    d = d + 4 * (x - y) + 10;
                    y -= N;
                }
                x += N;
            }
        }
        public void PixelFour(int x_st, int y_st, int x, int y) // додаткова функція для побудови еліпса за алгоритмом Брезенхема
        {
            putPixel(x_st + x, y_st + y);
            putPixel(x_st + x, y_st - y);
            putPixel(x_st - x, y_st + y);
            putPixel(x_st - x, y_st - y);
        }
        public void AlgorithmBrezenhema_Ellipse(int x_st, int y_st, int a, int b)   // еліпс за алгоритмом Брезенхема
        {
            int x, y;
            long xr2, yr2, d;
            double xm;
            xr2 = (a * a) << 1;
            yr2 = (b * b) << 1;
            x = 0;
            y = b;
            d = ((yr2 - xr2 * y) + xr2) >> 1;
            xm = xr2 / Math.Sqrt((xr2 + yr2) << 1) - 1;
            PixelFour(x_st, y_st, x, y);
            while (x < xm)
            {
                if (d > 0)
                {
                    y -= N;
                    d = d + yr2 * ((x << 1) + 3) - xr2 * (y << 1);
                }
                else
                    d = d + yr2 * ((x << 1) + 3);
                x += N;
                PixelFour(x_st, y_st, x, y);
            }
            d = (xr2 - yr2 * a) + (yr2 >> 1);
            x = a;
            y = 0;
            PixelFour(x_st, y_st, x, y);
            xm = xm + 2;
            while(x > xm)
            {
                if (d > 0)
                {
                    x -= N;
                    d = d + xr2 * ((y << 1) + 3) - yr2 * (x << 1);
                }
                else
                    d = d + xr2 * ((y << 1) + 3);
                y += N;
                PixelFour(x_st, y_st, x, y);
            }
        }
        static void Swap<T>(ref T lhs, ref T rhs)   // міняє місцями значення двох змінних
        {
            T temp;
            temp = lhs;
            lhs = rhs;
            rhs = temp;
        }
        public void WuPseudopixel(int x, int y, double brightness) // малює псевдопіксель, розміром N x N, в позиції (x, y) з яскравістю brightness
        {
            Graphics d = Graphics.FromHwnd(pictureBox1.Handle);    // оголошуємо об'єкт "d" класа Graphics і даємо йому можливість малювання на pictureBox1
            int tmp = (int)((1 - (brightness)) * 255);
            SolidBrush WuPixel = new SolidBrush(Color.FromArgb(255, tmp, tmp, tmp));   // створюємо об'єкт для замальовування фігур
            d.FillRectangle(WuPixel, x, y, N, N); // малюємо псевдопіксель
        }
        public void AlgorithmWu(int x_st, int y_st, int x_fin, int y_fin)
        {
            var steep = Math.Abs(y_fin - y_st) > Math.Abs(x_fin - x_st);
            if (steep)
            {
                Swap(ref x_st, ref y_st);
                Swap(ref x_fin, ref y_fin);
            }
            if (x_st > x_fin)
            {
                Swap(ref x_st, ref x_fin);
                Swap(ref y_st, ref y_fin);
            }

            WuPseudopixel(x_st, y_st, 1);
            WuPseudopixel(x_fin, y_fin, 1);
            double dx = x_fin - x_st;
            double dy = y_fin - y_st;
            double gradient = dy / dx;
            double y = y_st + gradient;
            for (var x = x_st + N; x <= x_fin - N; x++)
            {
                WuPseudopixel(x, (int)y, 1 - (y - (int)y));
                WuPseudopixel(x, (int)y + N, y - (int)y);
                y += gradient;
            }
        }
        public void CircleMyOwn(int x_st, int y_st, int R)
        {
            int x, y, d;
            x = 0;
            y = R;
            d = 3 - 2 * y;
            while (x <= y)
            {
                putPixel(x + x_st, y + y_st);
                putPixel(x + x_st, -y + y_st);
                putPixel(y + x_st, x + y_st);
                putPixel(y + x_st, -x + y_st);
                if (d < 0)
                    d = d + 4 * x + 6;
                else
                {
                    d = d + 4 * (x - y) + 10;
                    y -= N;
                }
                x += N;
            }
        }
        public void MyOwn() // виведення власних ініціалів
        {
            AlgorithmBrezenhema_Ellipse(200, 225, 80, 175);
            putPixel(295, 400);

            AlgorithmBrezenhema(400, 50, 400, 400);
            CircleMyOwn(400, 300, 100);
            AlgorithmBrezenhema(400, 50, 450, 50);
            putPixel(510, 400);
        }
        private void button1_Click(object sender, EventArgs e)  // алгоритм простого ЦДА
        {
            Stopwatch st = new Stopwatch();
            st.Start();
            SimpleDDA(Convert.ToInt32(textBox1.Text), Convert.ToInt32(textBox2.Text), Convert.ToInt32(textBox3.Text), Convert.ToInt32(textBox4.Text), Convert.ToInt32(textBox5.Text));
            st.Stop();
            using (StreamWriter sw = new StreamWriter("result.txt", true, System.Text.Encoding.Default))
            {
                sw.Write("Time for SimpleDDA: ");
                sw.WriteLine(st.Elapsed.TotalSeconds);
            }
        }

        private void button2_Click(object sender, EventArgs e)  // алгоритм асиметричного ЦДА
        {
            Stopwatch st = new Stopwatch();
            st.Start();
            AsymmetricalDDA(Convert.ToInt32(textBox6.Text), Convert.ToInt32(textBox7.Text), Convert.ToInt32(textBox8.Text), Convert.ToInt32(textBox9.Text));
            st.Stop();
            using (StreamWriter sw = new StreamWriter("result.txt", true, System.Text.Encoding.Default))
            {
                sw.Write("Time for AsymmetricalDDA: ");
                sw.WriteLine(st.Elapsed.TotalSeconds);
            }
        }

        private void button3_Click(object sender, EventArgs e)  // алгоритм Брезенхема
        {
            Stopwatch st = new Stopwatch();
            st.Start();
            AlgorithmBrezenhema(Convert.ToInt32(textBox10.Text), Convert.ToInt32(textBox11.Text), Convert.ToInt32(textBox12.Text), Convert.ToInt32(textBox13.Text));
            st.Stop();
            using (StreamWriter sw = new StreamWriter("result.txt", true, System.Text.Encoding.Default))
            {
                sw.Write("Time for AlgorithmBrezenhema: ");
                sw.WriteLine(st.Elapsed.TotalSeconds);
            }
        }

        private void button4_Click(object sender, EventArgs e)  // очищення екрану від попередніх фігур
        {
            pictureBox1.Invalidate();
        }

        private void button5_Click(object sender, EventArgs e)  // запускає тестування простого ЦДА
        {
            SimpleDDA(10, 300, 150, 400, 200);
            SimpleDDA(10, 400, 200, 600, 100);
        }

        private void button6_Click(object sender, EventArgs e)  // запускає тестування несиметричного ЦДА
        {
            AsymmetricalDDA(300, 150, 400, 200);
            AsymmetricalDDA(400, 200, 600, 100);
        }

        private void button7_Click(object sender, EventArgs e)  // запускає тестування алгоритма Брезенхема
        {
            AlgorithmBrezenhema(300, 150, 400, 200);
            AlgorithmBrezenhema(400, 200, 600, 100);
        }
        private void button9_Click(object sender, EventArgs e)  // алгоритм Брезенхема (еліпс)
        {
            Stopwatch st = new Stopwatch();
            st.Start();
            AlgorithmBrezenhema_Ellipse(Convert.ToInt32(textBox17.Text), Convert.ToInt32(textBox18.Text), Convert.ToInt32(textBox19.Text), Convert.ToInt32(textBox20.Text));
            st.Stop();
            using (StreamWriter sw = new StreamWriter("result.txt", true, System.Text.Encoding.Default))
            {
                sw.Write("Time for AlgorithmBrezenhema_Ellipse: ");
                sw.WriteLine(st.Elapsed.TotalSeconds);
            }
        }

        private void button8_Click(object sender, EventArgs e)  // алгоритм Брезенхема (коло)
        {
            Stopwatch st = new Stopwatch();
            st.Start();
            AlgorithmBrezenhema_Circle(Convert.ToInt32(textBox14.Text), Convert.ToInt32(textBox15.Text), Convert.ToInt32(textBox16.Text));
            st.Stop();
            using (StreamWriter sw = new StreamWriter("result.txt", true, System.Text.Encoding.Default))
            {
                sw.Write("Time for AlgorithmBrezenhema_Circle: ");
                sw.WriteLine(st.Elapsed.TotalSeconds);
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            Stopwatch st = new Stopwatch();
            st.Start();
            AlgorithmWu(Convert.ToInt32(textBox21.Text), Convert.ToInt32(textBox22.Text), Convert.ToInt32(textBox23.Text), Convert.ToInt32(textBox24.Text));
            st.Stop();
            using (StreamWriter sw = new StreamWriter("result.txt", true, System.Text.Encoding.Default))
            {
                sw.Write("Time for AlgorithmWu: ");
                sw.WriteLine(st.Elapsed.TotalSeconds);
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            MyOwn();
        }
    }
}
