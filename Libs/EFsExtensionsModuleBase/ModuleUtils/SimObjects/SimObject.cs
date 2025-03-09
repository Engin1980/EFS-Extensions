﻿//using Eng.EFsExtensions.EFsExtensionsModuleBase.ModuleUtils.SimConExtenders;
//using ESimConnect;
//using ESystem.Asserting;
//using System;
//using System.Collections.Generic;
//using System.Linq;

//namespace Eng.EFsExtensions.EFsExtensionsModuleBase.ModuleUtils.SimObjects
//{
//  public class SimObject
//  {
//    public record struct SimVarReg(string SimVar, string Unit);

//    #region Public Delegates

//    public delegate void SimPropertyChangedDelegate(SimProperty property, double value);

//    #endregion Public Delegates

//    #region Public Events

//    public event SimPropertyChangedDelegate? SimPropertyChanged;

//    public event Action? SimSecondElapsed;

//    public event Action? Started;

//    #endregion Public Events

//    #region Private Fields

//    private static SimObject? _instance = null;
//    private readonly OpenAsyncExtender openAsyncSimConExtender;
//    private readonly Dictionary<RequestId, SimVarReg> requestIdMapping = new();
//    private readonly SimSecondElapsedExtender secondElapsedSimConExtender;
//    private readonly ESimConnect.ESimConnect simCon;
//    private readonly Dictionary<SimProperty, double> simPropertyValues = new();
//    private readonly Dictionary<SimVarReg, List<SimProperty>> simVarReqMapping = new();
//    private readonly Dictionary<TypeId, SimVarReg> typeIdMapping = new();

//    #endregion Private Fields

//    #region Public Properties

//    public bool IsSimPaused => this.secondElapsedSimConExtender.IsSimPaused;

//    #endregion Public Properties

//    #region Public Constructors

//    public SimObject(ESimConnect.ESimConnect simCon)
//    {
//      EAssert.Argument.IsNotNull(simCon, nameof(simCon));
//      this.simCon = simCon;
//      this.simCon.ThrowsException += SimCon_ThrowsException;
//      this.simCon.DataReceived += SimCon_DataReceived;
//      this.openAsyncSimConExtender = new(simCon);
//      this.openAsyncSimConExtender.Opened += OpenAsyncSimConExtender_Opened;
//      this.secondElapsedSimConExtender = new(simCon, false);
//      this.secondElapsedSimConExtender.SimSecondElapsed += SecondElapsedSimConExtender_SimSecondElapsed;
//    }

//    #endregion Public Constructors

//    #region Public Indexers

//    public double this[string propertyName]
//    {
//      get
//      {
//        SimProperty? sp = simPropertyValues.Keys.FirstOrDefault(q => q.Name == propertyName);
//        if (sp == null)
//          throw new KeyNotFoundException($"Property {propertyName} not found among registered properties.");
//        else
//          return this[sp];
//      }
//    }

//    public double this[SimProperty simProperty]
//    {
//      get
//      {
//        EAssert.IsNotNull(simProperty, nameof(simProperty));

//        if (simPropertyValues.ContainsKey(simProperty))
//          return simPropertyValues[simProperty];
//        else
//          throw new KeyNotFoundException($"Property {simProperty.Name} not found among registered properties.");
//      }
//    }

//    #endregion Public Indexers

//    #region Public Methods

//    public static SimObject GetInstance()
//    {
//      var tmp = _instance;
//      if (tmp == null)
//      {
//        lock (typeof(SimObject))
//        {
//          if (_instance == null)
//            _instance = new SimObject(new ESimConnect.ESimConnect());
//          tmp = _instance;
//        }
//      }
//      EAssert.IsNotNull(tmp);
//      return tmp;
//    }

//    public Dictionary<SimProperty, double> GetAllPropertiesWithValues() => new(this.simPropertyValues);

//    public void RegisterProperties(IEnumerable<SimProperty> simProperties)
//    {
//      EAssert.Argument.IsNotNull(simProperties, nameof(simProperties));
//      foreach (var simProperty in simProperties)
//        this.RegisterProperty(simProperty);
//    }

//    public void RegisterProperty(SimProperty property)
//    {
//      EAssert.Argument.IsNotNull(property, nameof(property));
//      EAssert.IsTrue(this.openAsyncSimConExtender.IsOpened, "SimObject must be started first.");

//      lock (this)
//      {
//        if (this.simPropertyValues.ContainsKey(property) == false)
//        {
//          if (simPropertyValues.Keys.Any(q => q.Name == property.Name))
//          {
//            throw new ApplicationException($"SimProperty {property.Name} is already registered.");
//          }
//          else
//            RegisterPropertyToSimCon(property);
//        }

//        this.simPropertyValues[property] = double.NaN;
//      }
//    }

//    public void StartAsync()
//    {
//      this.openAsyncSimConExtender.OpenAsync();
//    }

//    #endregion Public Methods

//    #region Private Methods

//    private void OpenAsyncSimConExtender_Opened()
//    {
//      this.Started?.Invoke();
//    }

//    private void RegisterPropertyToSimCon(SimProperty property)
//    {
//      const string DEFAULT_PROPERTY_UNIT = "Number";

//      SimVarReg svr = new(property.SimVar, property.Unit ?? DEFAULT_PROPERTY_UNIT);
//      if (simVarReqMapping.ContainsKey(svr) == false)
//      {
//        TypeId typeId = simCon.Values.Register<double>(property.SimVar, property.Unit ?? DEFAULT_PROPERTY_UNIT);
//        typeIdMapping[typeId] = svr;
//        simVarReqMapping[svr] = new();

//        RequestId requestId = simCon.Values.RequestRepeatedly(typeId, SimConnectPeriod.SECOND);
//        requestIdMapping[requestId] = svr;
//      }
//      simVarReqMapping[svr].Add(property);
//    }

//    private void SecondElapsedSimConExtender_SimSecondElapsed()
//    {
//      this.SimSecondElapsed?.Invoke();
//    }

//    private void SimCon_DataReceived(ESimConnect.ESimConnect sender, ESimConnect.ESimConnect.ESimConnectDataReceivedEventArgs e)
//    {
//      double value = (double)e.Data;
//      RequestId? requestId = e.RequestId;
//      if (requestId == null) return; // not my registered type
//      if (requestIdMapping.ContainsKey(requestId.Value) == false) return; // not my registered type
//      SimVarReg svr = requestIdMapping[requestId.Value];
//      foreach (var simProperty in simVarReqMapping[svr])
//      {
//        simPropertyValues[simProperty] = value;
//        SimPropertyChanged?.Invoke(simProperty, value);
//      }
//    }

//    private void SimCon_ThrowsException(ESimConnect.ESimConnect sender, SimConnectException ex)
//    {
//      throw new ApplicationException("SimCon thows exception: " + ex.ToString());
//    }

//    #endregion Private Methods
//  }
//}
