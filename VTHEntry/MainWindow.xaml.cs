using LiveCharts;
using LiveCharts.Helpers;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace VTHEntry
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new ChartViewModel();
            Values.CellEditEnding += Values_CellEditEnding;
        }
        private void UIElement_OnMouseMove(object sender, MouseEventArgs e)
        {
            var vm = (ChartViewModel)DataContext;
            var chart = (LiveCharts.Wpf.CartesianChart)sender;

            var mouseCoordinate = e.GetPosition(chart);

            var p = chart.ConvertToChartValues(mouseCoordinate);

            vm.YPointer = p.Y;

            vm.XPointer = p.X;
            PointTracker tr = chart.Series[0].Values.GetTracker(chart.Series[0]);
            var points = chart.Series[0].ActualValues.GetPoints(chart.Series[0]).ToList();
            var series = chart.Series[0];

            series.ClosestPointTo(p.X, AxisOrientation.X);

        }


        public Func<double, string> Formatter { get; set; }

        private void CartesianChart_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            X.MinValue = double.NaN;
            X.MaxValue = double.NaN;
            Y.MinValue = double.NaN;
            Y.MaxValue = double.NaN;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var chart = DataContext as ChartViewModel;
            MyPoint pt = new MyPoint();
            chart.points.Add(pt);
            chart.AddPoint(pt);
        }

        private void Values_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
                var column = e.Column as DataGridBoundColumn;
                if (column != null)
                {
                    var bindingPath = (column.Binding as Binding).Path.Path;
                    if (bindingPath == "X")
                    {
                        int rowIndex = e.Row.GetIndex();
                        var el = e.EditingElement as TextBox;
                        var chart = DataContext as ChartViewModel;
                        chart.points[rowIndex].X = Convert.ToDouble(el.Text);
                        chart.UpdateChart(rowIndex, chart.points[rowIndex].X, true);
                    }
                    if (bindingPath == "Y")
                    {
                        int rowIndex = e.Row.GetIndex();
                        var el = e.EditingElement as TextBox;
                        var chart = DataContext as ChartViewModel;
                        chart.points[rowIndex].Y = Convert.ToDouble(el.Text);
                        chart.UpdateChart(rowIndex, chart.points[rowIndex].Y, false);
                    }
                }
            }
        }


        public class ZoomingModeCoverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                switch ((ZoomingOptions)value)
                {
                    case ZoomingOptions.None:
                        return "None";
                    case ZoomingOptions.X:
                        return "X";
                    case ZoomingOptions.Y:
                        return "Y";
                    case ZoomingOptions.Xy:
                        return "XY";
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }
    }
}
