using LiveCharts;
using LiveCharts.Configurations;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace VTHEntry
{
    class ChartViewModel : INotifyPropertyChanged
    {
        private double _xPointer;
        private double _yPointer;
        private ZoomingOptions _zoomingMode;
        public ObservableCollection<MyPoint> points { get; set; }
        public double Base { get; set; }
        public SeriesCollection SeriesCollection { get; set; }

        public ChartViewModel()
        {
            XPointer = 5;
            YPointer = 5;

            Formatter = x => x.ToString("N2");

            points = new ObservableCollection<MyPoint>();
            Base = 10;
            var mapper = Mappers.Xy<ObservablePoint>()
                .X(point => Math.Log(point.X, Base))
                .Y(point => point.Y);

            SeriesCollection = new SeriesCollection(mapper)
            {
                new LineSeries
                {
                    Values = new ChartValues<ObservablePoint>
                    {
                        new ObservablePoint(1,2),
                        new ObservablePoint(2,3),
                        new ObservablePoint(3,4),
                        new ObservablePoint(4,5),
        }
                }
            };

            Formatter = value => Math.Pow(Base, value).ToString("N2");

            ZoomingMode = ZoomingOptions.X;
        }

        public double XPointer
        {
            get { return _xPointer; }
            set
            {
                _xPointer = value;
                OnPropertyChanged("XPointer");
            }
        }

        public double YPointer
        {
            get { return _yPointer; }
            set
            {
                _yPointer = value;
                OnPropertyChanged("YPointer");
            }
        }

        public ZoomingOptions ZoomingMode
        {
            get { return _zoomingMode; }
            set
            {
                _zoomingMode = value;
                OnPropertyChanged("ZoomingMode");
            }
        }

        public Func<double, string> Formatter { get; set; }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null) PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void AddPoint(MyPoint point)
        {
            ObservablePoint pt = new ObservablePoint(Math.Log10(point.X), Math.Log10(point.Y));
            this.SeriesCollection.ElementAt(0).Values.Add(pt);
        }

        public void UpdateChart(int i, double value, bool xOrY)
        {
            ObservablePoint pt = this.SeriesCollection.ElementAt(0).Values[i] as ObservablePoint;
            if (xOrY)
                pt.X = value;
            else
                pt.Y = Math.Log(value)/Math.Log(Base);
            this.SeriesCollection.ElementAt(0).Values.RemoveAt(i);
            this.SeriesCollection.ElementAt(0).Values.Insert(i, pt);
        }
    }
}
