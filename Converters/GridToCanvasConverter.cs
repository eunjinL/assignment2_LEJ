using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace assignment2_LEJ.Converters
{
    public class GridToCanvasConverter : IValueConverter
    {
        const int CellSize = 10;
        const int Margin = 1;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is int gridCoordinate))
                return 0;

            int canvasCoordinate = (CellSize + Margin) * gridCoordinate;
            return canvasCoordinate;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
