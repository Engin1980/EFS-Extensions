﻿using Eng.Chlaot.ChlaotModuleBase.ModuleUtils.SimObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eng.Chlaot.ChlaotModuleBase.ModuleUtils.WPF.VMs
{
  public class PropertyVM : WithValueVM
  {

    public SimProperty Property
    {
      get => base.GetProperty<SimProperty>(nameof(Property))!;
      set => base.UpdateProperty(nameof(Property), value);
    }

    public override string ToString() => $"{this.Property.Name}={this.Value} {{PropertyVM}}";
  }
}
