﻿using FailuresModule.Model.Run.Sustainers;
using FailuresModule.Model.Failures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Printing;
using System.Text;
using System.Threading.Tasks;

namespace FailuresModule.Model.Run.Sustainers
{
  internal class SimVarFailureSustainer : SimVarBasedFailureSustainer
  {
    #region Private Fields

    private readonly SetFailureDefinition failure;

    #endregion Private Fields

    #region Public Constructors

    public SimVarFailureSustainer(SetFailureDefinition failure) : base(failure)
    {
      this.failure = failure;
    }

    #endregion Public Constructors

    #region Protected Methods

    protected override void InitInternal()
    {
      base.InitInternal();
    }

    protected override void ResetInternal()
    {
      SendEvent(failure.OkValue); // Expected to be 0 typically
    }

    protected override void StartInternal()
    {
      SendEvent(failure.FailValue); // Expected to be 1 typically
    }

    #endregion Protected Methods

    #region Private Methods

    private void SendEvent(double arg)
    {
      base.SendData(arg);
    }

    #endregion Private Methods
  }
}
