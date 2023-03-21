﻿using AffinityModule;
using ELogging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Eng.Chlaot.Modules.AffinityModule
{
  public class Rule
  {
    public const string ROLL_REGEX = "^((\\d+)(-(\\d+))?)(,(\\d+)(-(\\d+))?)*$";

    public enum EPriority
    {
      Idle,
      BelowNormal,
      Normal,
      AboveNormal,
      Highest
    }

    public List<bool> CoreFlags { get; set; }

    public string TitleOrRegex => this.Title ?? this.Regex ?? "(null)";

    public string? Title { get; set; }

    private string _Roll = "";
    public string Roll
    {
      get => this._Roll;
      set
      {
        this._Roll = value ?? throw new ArgumentNullException("Value 'Roll' cannot be null. Use empty string instead.");
        ExpandToCores();
      }
    }

    public EPriority? Priority { get; set; }
    public ProcessPriorityClass PriorityClass
    {
      get
      {
        if (Priority == null) throw new NotSupportedException("PriorityClass property get cannot be invoked if Priority is null.");
        ProcessPriorityClass ret = this.Priority.Value switch
        {
          EPriority.Normal => ProcessPriorityClass.Normal,
          EPriority.Idle => ProcessPriorityClass.Idle,
          EPriority.BelowNormal => ProcessPriorityClass.BelowNormal,
          EPriority.AboveNormal => ProcessPriorityClass.AboveNormal,
          EPriority.Highest => ProcessPriorityClass.High,
          _ => throw new ArgumentOutOfRangeException(this.Priority.Value + " is unknown priority value.")
        };
        return ret;
      }
    }
    public bool ShouldChangePriority => this.Priority != null;

    public string Regex { get; set; }
    public bool ShouldChangeAffinity => this._Roll.Length > 0;

    public Rule()
    {
      this.CoreFlags = AffinityUtils.ToEmptyArray(false);
      _Roll = Roll = "";
      Regex = ".+";
    }

    private void ExpandToCores()
    {
      List<int> includedIndices = new();

      if (Roll.Length > 0)
      {
        if (System.Text.RegularExpressions.Regex.IsMatch(Roll, ROLL_REGEX) == false)
        {
          Logger.Log(this, LogLevel.WARNING, $"CoresPatter '{Roll}' is not in valid format.");
          return;
        }

        string[] pts = this.Roll.Split(';');
        foreach (string pt in pts)
        {
          if (pt.Contains('-'))
          {
            string[] tms = pt.Split('-');
            int fromIndex = int.Parse(tms[0]);
            int toIndex = int.Parse(tms[1]);
            for (int i = fromIndex; i <= toIndex; i++)
              includedIndices.Add(i);

          }
          else
          {
            int index = int.Parse(pt);
            includedIndices.Add(index);
          }
        }

        for (int i = 0; i < CoreFlags.Count; i++)
        {
          CoreFlags[i] = includedIndices.Contains(i);
        }
      }
      else
      {
        for (int i = 0; i < CoreFlags.Count; i++)
        {
          CoreFlags[i] = true;
        }
      }
    }

  }
}
