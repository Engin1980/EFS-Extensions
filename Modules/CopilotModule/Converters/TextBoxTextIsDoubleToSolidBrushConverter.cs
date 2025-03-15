﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace Eng.EFsExtensions.Modules.CopilotModule.Converters
{
  internal class TextBoxTextIsDoubleToSolidBrushConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      string s = (string)value;
      bool tmp = double.TryParse(s, out double val);
      SolidColorBrush ret = tmp && double.IsNaN(val) == false
        ? new SolidColorBrush(Colors.LightGreen)
        : new SolidColorBrush(Colors.LightPink);
      return ret;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }
}
