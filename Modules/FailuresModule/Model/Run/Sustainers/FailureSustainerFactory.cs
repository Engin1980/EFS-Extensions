﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FailuresModule.Model.Run.Sustainers;
using FailuresModule.Model.Failures;

namespace FailuresModule.Model.Run.Sustainers
{
    internal class FailureSustainerFactory
  {
    internal static FailureSustainer Create(FailureDefinition failItem)
    {
      FailureSustainer ret;
      if (failItem is ToggleFailureDefinition efd)
        ret = CreateEvent(efd);
      else if (failItem is SetFailureDefinition svfd)
        ret = CreateSimVar(svfd);
      else if (failItem is StuckFailureDefinition sfd)
        ret = CreateStuck(sfd);
      else if (failItem is LeakFailureDefinition lfd)
        ret = CreateLeak(lfd);
      else if (failItem is SneakFailureDefinition nfd)
        ret = CreateSneak(nfd);
      else if (failItem is ToggleOnVarMismatchFailureDefinition svvefd)
        ret = CreateSimVarViaEvent(svvefd);
      else
        throw new NotImplementedException();
      return ret;
    }

    private static FailureSustainer CreateSimVarViaEvent(ToggleOnVarMismatchFailureDefinition svvefd)
    {
      SimVarViaEventFailureSustainer ret = new(svvefd);
      return ret;
    }

    private static FailureSustainer CreateSneak(SneakFailureDefinition nfd)
    {
      SneakFailureSustainer ret = new SneakFailureSustainer(nfd);
      return ret;
    }

    private static FailureSustainer CreateSimVar(SetFailureDefinition svfd)
    {
      SimVarFailureSustainer ret = new SimVarFailureSustainer(svfd);
      return ret;
    }

    private static FailureSustainer CreateLeak(LeakFailureDefinition lfd)
    {
      LeakFailureSustainer ret = new LeakFailureSustainer(lfd);
      return ret;
    }

    private static FailureSustainer CreateStuck(StuckFailureDefinition sfd)
    {
      StuckFailureSustainer ret = new StuckFailureSustainer(sfd);
      return ret;
    }

    private static FailureSustainer CreateEvent(ToggleFailureDefinition ifd)
    {
      EventFailureSustainer ret = new EventFailureSustainer(ifd);
      return ret;
    }
  }
}
