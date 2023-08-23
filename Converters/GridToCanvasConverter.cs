﻿using assignment2_LEJ.ViewModels;
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
        const int CellWidth = 40;
        const int CellHeight = 10;
        const int Margin = 1;

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