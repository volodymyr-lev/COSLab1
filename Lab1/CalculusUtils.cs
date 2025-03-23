using ScottPlot.Interactivity.UserActionResponses;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace Lab1
{
    internal static class CalculusUtils
    {
        private static List<double> Ans { get; set; } 
        private static List<double> Bns { get; set; }

        public static double Delta { get; private set; }
        public static double Power { get; private set; }
        public static double Harmonics { get; private set; }

        static CalculusUtils()
        {
            Ans = new List<double>();
            Bns = new List<double>();
        }

        public static double F(double x)
        {
            double period = x % (2 * Math.PI);
            if (period > Math.PI) period -= 2 * Math.PI;
            if (period < -Math.PI) period += 2 * Math.PI;

            return (period <= 0) ? -period : period;
        }
        //public static double An(int n, double I1, double I2)
        //{
        //    if (n == 0) return (1.0 / Math.PI) * integral(F, I1, I2, 4000);

        //    Func<double, double> cos = x => Math.Cos(x * n);
        //    double mid = (I1 + I2) / 2;
        //    double result = (1.0 / Math.PI) * (-integral(cos, I1, mid, 4000) + integral(cos, mid, I2, 4000));

        //    return result;
        //}

        public static double An(int n, double I1, double I2)
        {
            if (n == 0)
            {
                return (1.0 / Math.PI) * integral(F, -Math.PI, Math.PI, 4000);
            }

            Func<double, double> integrand = t => F(t) * Math.Cos(n * t);
            return (1.0 / Math.PI) * integral(integrand, -Math.PI, Math.PI, 4000);
        
        }

        public static double Bn(int n, double I1, double I2)
        {
            if (n == 0)
            {
                // Для b0 = 0, оскільки sin(0*t) = 0
                return 0;
            }

            Func<double, double> integrand = t => F(t) * Math.Sin(n * t);
            return (1.0 / Math.PI) * integral(integrand, -Math.PI, Math.PI, 4000);
        }

        //public static double CountFourier(double N, double presicion, double x, double I1, double I2)
        //{
        //    double a0 = An(0, I1, I2);
        //    double sum = 0;
        //    double prevSum = 2;
        //    int k = 1;

        //    while((Math.Abs(sum-prevSum)>presicion && k < 50))
        //    {
        //        prevSum = sum;
        //        double an = An(k, I1, I2);
        //        double bn = Bn(k, I1, I2);

        //        sum += an * Math.Cos(k * x) + bn * Math.Sin(k * x);
        //        k++;
        //    }
        //    return a0 / 2 + sum;
        //}

        //workd but kinda shit

        //static double A0()
        //{
        //    double integral = 0.0, step = 0.001;
        //    for (double x = -Math.PI; x < Math.PI; x += step)
        //        integral += F(x) * step;
        //    return (1.0 / Math.PI) * integral;
        //}

        //static double An(int n)
        //{
        //    double integral = 0.0, step = 0.001;
        //    for (double x = -Math.PI; x < Math.PI; x += step)
        //        integral += F(x) * Math.Cos(n * x) * step;
        //    return (1.0 / Math.PI) * integral;
        //}

        //static double Bn(int n)
        //{
        //    double integral = 0.0, step = 0.001;
        //    for (double x = -Math.PI; x < Math.PI; x += step)
        //        integral += F(x) * Math.Sin(n * x) * step;
        //    return (1.0 / Math.PI) * integral;
        //}

        //public static double CountFourier(int maxN, double precision, double x, double I1, double I2)
        //{

        //    double sum = An(0,I1,I2) / 2;
        //    double prevSum;
        //    int n = 1;

        //    do
        //    {
        //        prevSum = sum;
        //        sum += An(n,I1,I2) * Math.Cos(n * x) + Bn(n,I1,I2) * Math.Sin(n * x);
        //        n++;
        //    }
        //    while (n <= maxN && Math.Abs(sum - prevSum) > precision);

        //    return sum;
        //}

        public static double CountFourier(int harmonics, double x, double I1, double I2)
        {
            Harmonics = harmonics;

            double result = Ans[0] / 2.0;

            for (int n = 1; n <= harmonics; n++)
            {
                double an = Ans[n];
                double bn = Bns[n];

                result += an * Math.Cos(n * x) + bn * Math.Sin(n * x);
            }

            return result;
        }

        public static double CalculateDelta(int harmonics, List<double> xs, double I1, double I2)
        {
            double delta = 0;

            for (int i = 0; i < xs.Count; i++)
            {
                delta += Math.Abs(F(xs[i]) - CountFourier(harmonics, xs[i], I1, I2));
            }

            Delta = delta / xs.Count;

            return Delta;
        }

        public static double CalculatePower(int N, double i1, double i2)
        {
            //double power = 0.5 * Math.Pow(An(0, i1, i2), 2);
            double power = 0.25 * Math.Pow(Ans[0], 2);


            for (int n = 1; n <= N; n++)
            {
                power += 0.5 * (Math.Pow(Ans[n], 2) + Math.Pow(Bns[n], 2));
            }

            Power = power;

            return power;
        }

        //public static double CalculatePower(int maxN, double I1, double I2)
        //{
        //    Func<double, double> S_N = x => CountFourier(maxN, x, I1, I2);

        //    return (1.0 / (I2 - I1)) * integral(x => Math.Pow(S_N(x), 2), I1, I2, 40000);
        //}

        public static void SaveCalculations(string fileName = "../../calcs.txt")
        {
            using (StreamWriter writetext = new StreamWriter(fileName))
            {
                writetext.WriteLine($"N: {Harmonics}");
                writetext.WriteLine($"Signal Power: {Power}");
                writetext.WriteLine($"Delta: {Delta}");

                for (int i = 0; i < Ans.Count; i++)
                {
                    writetext.WriteLine($"A[{i}] = {Ans[i]}\t B[{i}] = {Bns[i]}");
                }
            }

            Ans.Clear();
            Bns.Clear();
        }

        private static double integral(Func<double, double> f, double a, double b, double steps)
        {
            double h = (b - a) / steps;
            double sum = 0.5 * (f(a) + f(b));
            
            for(int i = 1; i < steps; i++)
            {
                sum += f(a+i*h);
            }

            return sum*h;
        }

        // 
        public static void countCoefficients(int N, double I1, double I2)
        {
            Ans.Clear(); Bns.Clear();

            double a0 = An(0, I1, I2); Ans.Add(a0);
            double b0 = Bn(0, I1, I2); Bns.Add(b0);

            for (int i = 1; i <= N; i++)
            {
                double an = An(i, I1, I2);
                double bn = Bn(i, I1, I2);

                Ans.Add(an); Bns.Add(bn);

            }
        }
    }
}
