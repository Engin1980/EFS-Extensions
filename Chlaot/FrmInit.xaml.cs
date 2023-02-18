﻿using Eng.Chlaot.ChlaotModuleBase;
using System;
using System.Collections.Generic;
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

namespace Chlaot
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class FrmInit : Window
  {
    public Context Context { get; set; }

    public FrmInit()
    {
      InitializeComponent();
    }

    public void LogToConsole(LogLevel level, string message)
    {
      txtConsole.AppendText("\n");
      txtConsole.AppendText(level + ":: " + message);
      txtConsole.ScrollToEnd();
    }

    private void lstModules_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      IModule module = lstModules.SelectedItem as IModule;
      pnlContent.Children.Clear();
      pnlContent.Children.Add(module.InitControl);
    }

    private void btnRun_Click(object sender, RoutedEventArgs e)
    {
      FrmRun frmRun = new FrmRun(Context);
      this.Close();
      frmRun.Show();
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {

    }

    private void Window_Initialized(object sender, EventArgs e)
    {
      this.Context = new Context();
      this.Context.SetLogHandler((l, m) => this.LogToConsole(l, m));
      this.Context.SetUpModules();

      this.DataContext = this.Context;
      if (lstModules.Items.Count > 0) lstModules.SelectedIndex = 0;

      this.Context.InitModules();
    }
  }
}
