﻿using Eng.EFsExtensions.Modules.ChecklistModule.Types.VM;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Eng.EFsExtensions.Modules.ChecklistModule.Converters
{
  internal class RunStateToTreeNodeExpandedConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      RunState runState= (RunState)value;
      bool ret = runState == RunState.Current;
      return ret;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }
}
