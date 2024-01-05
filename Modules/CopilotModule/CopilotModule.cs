﻿using CopilotModule;
using ELogging;
using Eng.Chlaot.ChlaotModuleBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Eng.Chlaot.Modules.CopilotModule
{
  public class CopilotModule : NotifyPropertyChangedBase, IModule
  {
    public bool IsReady
    {
      get => base.GetProperty<bool>(nameof(IsReady))!;
      private set => base.UpdateProperty(nameof(IsReady), value);
    }

    private Control? _InitControl;
    public Control InitControl => _InitControl ?? throw new ApplicationException("Control not provided.");

    private Control? _RunControl;
    public Control RunControl => _RunControl ?? throw new ApplicationException("Control not provided.");

    public string Name => "Copilot";
    private readonly Logger logger;
    private InitContext? initContext;
    private RunContext? runContext;
    private Settings? settings;

    public CopilotModule()
    {
      this.IsReady = false;
      this.logger = Logger.Create(this);
    }

    public void Init()
    {
      this.initContext = new InitContext(this.settings!, q => this.IsReady = q);
      this._InitControl = new CtrInit(this.initContext);
    }

    public void Run()
    {
      this.runContext = new RunContext(this.initContext!);
      this._RunControl = new CtrRun(this.runContext);
      this.runContext.Run();

      this.initContext = null;
      this._InitControl = null;
    }

    public void SetUp(ModuleSetUpInfo setUpInfo)
    {
      try
      {
        settings = Settings.Load();
        logger.Invoke(LogLevel.INFO, "Settings loaded.");
      }
      catch (Exception ex)
      {
        logger.Invoke(LogLevel.ERROR, "Unable to load settings. " + ex.GetFullMessage());
        logger.Invoke(LogLevel.INFO, "Default settings used.");
        settings = new Settings();
      }
    }

    public void Stop()
    {
      this.runContext?.Stop();
    }
  }
}
