using assignment2_LEJ.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace assignment2_LEJ.Converters
{
    public class GridToCanvasConverter : DependencyObject, IValueConverter
    {
        const int Margin = 1;

        public double CellWidth
        {
            get
            {
                return (double)GetValue(CellWidthProperty);
            }
            set
            {
                SetValue(CellWidthProperty, value);
            }
        }

        public static readonly DependencyProperty CellWidthProperty =
            DependencyProperty.Register("CellWidth", typeof(double), typeof(GridToCanvasConverter));

        public double CellHeight
        {
            get { return (double)GetValue(CellHeightProperty); }
            set { SetValue(CellHeightProperty, value); }
        }

        public static readonly DependencyProperty CellHeightProperty =
            DependencyProperty.Register("CellHeight", typeof(double), typeof(GridToCanvasConverter));

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is int gridCoordinate))
                return 0;

            if (parameter != null && parameter.ToString() == "Width")
            {
                return (CellWidth + Margin) * gridCoordinate;
            }
            else
            {
                return (CellHeight + Margin) * gridCoordinate;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


}