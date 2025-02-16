﻿using System;
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

namespace Eng.Chlaot.ChlaotModuleBase.ModuleUtils.TTSs
{
  /// <summary>
  /// Interaction logic for CtrTtss.xaml
  /// </summary>
  public partial class CtrTtsModuleSelector : UserControl
  {
    public CtrTtsModuleSelector()
    {
      this.SpeechTestVisibility = Visibility.Visible;

      InitializeComponent();

      tabTtss.SelectionChanged += TabTtss_SelectionChanged;
      tabTtss.Items.Add(new TabItem()
      {
        Header = "Not initialized",
        Content = new Label()
        {
          Content = "Not initialized"
        }
      });
    }

    public static readonly DependencyProperty SpeechTestVisibilityProperty = DependencyProperty.Register(
      nameof(SpeechTestVisibility), typeof(Visibility), typeof(CtrTtsModuleSelector));
    public Visibility SpeechTestVisibility
    {
      get => (Visibility)GetValue(SpeechTestVisibilityProperty);
      set => SetValue(SpeechTestVisibilityProperty, value);
    }

    public static readonly DependencyProperty SelectedModuleProperty =
        DependencyProperty.Register(nameof(SelectedModule), typeof(ITtsModule), typeof(CtrTtsModuleSelector));
    public ITtsModule SelectedModule
    {
      get { return (ITtsModule)GetValue(SelectedModuleProperty); }
      set { SetValue(SelectedModuleProperty, value); }
    }

    public void Init(IEnumerable<ITtsModule> modules)
    {
      tabTtss.Items.Clear();
      foreach (var module in modules)
      {
        DockPanel dck = new();
        var sett = module.GetDefaultSettings();
        var ctr = module.GetSettingsControl(sett);
        dck.Children.Add(ctr);

        TabItem tabItem = new()
        {
          Header = module.Name,
          Content = dck,
          Tag = module
        };
        tabTtss.Items.Add(tabItem);
      }
    }

    public void TrySpeech(string speechText)
    {
      todo implement here player from ElevenLabs
    }

    private void TabTtss_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (tabTtss.SelectedItem is TabItem ti)
        this.SelectedModule = (ITtsModule)ti.Tag;
    }

    private void btnTrySpeech_Click(object sender, RoutedEventArgs e)
    {
      TrySpeech(txtTrySpeech.Text);
    }
  }
}
