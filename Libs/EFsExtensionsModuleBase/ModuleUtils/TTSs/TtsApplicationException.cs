﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eng.EFsExtensions.EFsExtensionsModuleBase.ModuleUtils.TTSs
{
  public class TtsApplicationException : ApplicationException
  {
    public TtsApplicationException(string message, Exception innerException) : base(message, innerException) { }
    public TtsApplicationException(string message) : base(message) { }
  }
}
