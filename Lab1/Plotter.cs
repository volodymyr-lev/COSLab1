using ScottPlot;
using ScottPlot.Interactivity;
using ScottPlot.WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1
{
    class Plotter
    {
        public WpfPlot PlotWindow { get; init; }
        
        public int N {  get; set; }
        public double I1 {  get; set; }
        public double I2 {  get; set; }

        public double delta { get; private set; }

        public Plotter(in WpfPlot PlotWindow) 
        {
            this.PlotWindow = PlotWindow;
            PlotWindow.Plot.Add.HorizontalLine(0, color: Color.FromSDColor(System.Drawing.Color.Black));
            PlotWindow.Plot.Add.VerticalLine(0, color: Color.FromSDColor(System.Drawing.Color.Black));
        }

        public void PlotGiven()
        {
            List<double> dataX = new List<double>();
            List<double> dataY = new List<double>();

            double step = 0.01;


            for (double i = -10 * Math.PI; i < 10 * Math.PI; i += Math.PI )
            {
                // in one period
                for (double j = i+step; j < i + Math.PI; j += step)
                {
                    dataX.Add(j);
                    dataY.Add(CalculusUtils.F(j));
                }

                var scatter = PlotWindow.Plot.Add.Scatter(dataX.ToArray(), dataY.ToArray());
                scatter.Color = Colors.Black;

                PlotWindow.Refresh();

                dataX.Clear();
                dataY.Clear();
            }
        }

        public (double delta, double power) PlotFourier(int N, double i1, double i2)
        {
            List<double> dataX = new List<double>();
            List<double> dataY = new List<double>();

            double step = 0.01;

            CalculusUtils.countCoefficients(N, i1, i2);

            for (double i = i1; i < i2; i+=step)
            {
                double fourier = CalculusUtils.CountFourier(N,i,i1,i2);
                dataX.Add(i);
                dataY.Add(fourier);
            }

            var scatter = PlotWindow.Plot.Add.Scatter(dataX.ToArray(), dataY.ToArray());

            double delta = CalculusUtils.CalculateDelta(N, dataX, i1, i2);
            double power = CalculusUtils.CalculatePower(N, i1, i2);

            PlotWindow.Refresh();

            return (delta,power);
        }


        //public void PlotGiven()
        //{
        //    for(double i = -10*Math.PI; i < 10*Math.PI; i+=Math.PI)
        //    {
        //        double periodStart = i % (2 * Math.PI);
        //        if (periodStart < -Math.PI) periodStart += 2 * Math.PI;
        //        if (periodStart > Math.PI) periodStart -= 2 * Math.PI;

        //        double yStart = (periodStart <= 0) ? -periodStart : periodStart;

        //        double periodEnd = (i + Math.PI) % (2 * Math.PI);
        //        if (periodEnd < -Math.PI) periodEnd += 2 * Math.PI;
        //        if (periodEnd > Math.PI) periodEnd -= 2 * Math.PI;

        //        double yEnd = (periodEnd <= 0) ? -periodEnd : periodEnd;

        //        Coordinates start = new Coordinates(i, yStart);
        //        Coordinates end = new Coordinates(i + Math.PI, yEnd);

        //        var line = PlotWindow.Plot.Add.Po(start, end);
        //        line.Color = Colors.Black;
        //    }

        //    PlotWindow.Refresh();
        //}


        public void ClearPlot()
        {
            PlotWindow.Plot.Clear();

            PlotWindow.Plot.Add.HorizontalLine(0, color: Color.FromSDColor(System.Drawing.Color.Black));
            PlotWindow.Plot.Add.VerticalLine(0, color: Color.FromSDColor(System.Drawing.Color.Black));

            PlotWindow.Refresh();
        }
    }
}
