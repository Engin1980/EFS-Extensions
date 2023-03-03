﻿using ChlaotModuleBase.ModuleUtils.KeyHooking;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
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

namespace ChecklistModule
{
  /// <summary>
  /// Interaction logic for RunControl.xaml
  /// </summary>
  public partial class CtrRun : UserControl
  {
    private readonly RunContext context;

    public CtrRun()
    {
      InitializeComponent();
      this.context = null!;
    }

    public CtrRun(RunContext context) : this()
    {
      this.context = context;
      this.DataContext = context;

      Window window = Window.GetWindow(this);
      var keyHookWrapper = new KeyHookWrapper(window);
      this.context.Run(keyHookWrapper);
    }
  }
}
