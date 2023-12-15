﻿using FailuresModule.Model.App;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace FailuresModule.Converters
{
    public class PercentageToIntConverter : TypedConverter<Percentage, int>
  {
    protected sealed override int Convert(Percentage value, object parameter, CultureInfo culture) => (int)(value * 100);

    protected sealed override Percentage ConvertBack(int value, object parameter, CultureInfo culture) => (Percentage)(value / 100.0);

    protected override int ConvertToTarget(object value, object parameter, CultureInfo culture) => (int)(double)value;
  }
}
