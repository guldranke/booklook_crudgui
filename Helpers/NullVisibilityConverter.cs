﻿using System.Globalization;
using System.Windows.Data;
using System.Windows;
using System;

namespace booklook_crudgui.Helpers {
    public class NullVisibilityConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            return value == null ? Visibility.Hidden : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}