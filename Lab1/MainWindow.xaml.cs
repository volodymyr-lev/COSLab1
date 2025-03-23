using ScottPlot;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Lab1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Plotter plotter;

        private int N;
        private double I1, I2;

        public MainWindow()
        {
            InitializeComponent();

            plotter = new Plotter(WpfPlot1);
        }



        private void BtnDrawGiven_Click(object sender, RoutedEventArgs e)
        {
            plotter.PlotGiven();
        }

        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            plotter.ClearPlot();
        }



        private void BtnDrawAprox_Click(object sender, RoutedEventArgs e)
        {
            if(!GetData()) return;

            (double delta, double power) = plotter.PlotFourier(N, I1, I2);

            LblDelta.Content = $"Delta: {delta}";
            LblPower.Content = $"Power: {power}";
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            CalculusUtils.SaveCalculations();
        }

        private bool GetData()
        {
            if (string.IsNullOrEmpty(TextBoxI1.Text) || string.IsNullOrEmpty(TextBoxI2.Text)) return false;
            
            N = InputConverter.GetN(TextBoxN.Text);
            (I1,I2) = InputConverter.GetIntervals(TextBoxI1.Text, TextBoxI2.Text);
            return true;
        }
    }
}