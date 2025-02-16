﻿
namespace Eng.Chlaot.Modules.RaaSModule.Model
{
  public class RaasSpeech
  {
    public string Speech { get; set; }

    internal virtual void CheckSanity()
    {
      if (Speech == null) throw new ApplicationException("RaasSpeech.Speech is null");
    }
  }
}
