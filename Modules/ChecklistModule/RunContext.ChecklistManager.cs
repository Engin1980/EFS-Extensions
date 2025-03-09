﻿using Eng.EFsExtensions.EFsExtensionsModuleBase;
using Eng.EFsExtensions.EFsExtensionsModuleBase.ModuleUtils.SimObjects;
using Eng.EFsExtensions.EFsExtensionsModuleBase.ModuleUtils.StateChecking;
using Eng.EFsExtensions.EFsExtensionsModuleBase.ModuleUtils.WPF.VMs;
using Eng.EFsExtensions.Modules.ChecklistModule.Types.VM;
using ESystem.Asserting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Eng.EFsExtensions.Modules.ChecklistModule
{
  internal partial class RunContext
  {
    public class ChecklistManager
    {
      private readonly PlaybackManager playbackManager;
      private CheckListVM? previous;
      private CheckListVM current;
      private readonly List<CheckListVM> active = new();
      private readonly List<CheckListVM> all;
      private readonly bool isAutoplayingEnabled;
      private readonly NewSimObject simObject;
      private readonly PropertyVMS propertyVMs;

      internal ChecklistManager(PropertyVMS propertyVMs, List<CheckListVM> checkListViews, NewSimObject simObject,
        bool useAutoplay, bool readConfirmations, int? pausedAlertIntervalIfUsed, bool isPlayPerItemEnabled)
      {
        EAssert.Argument.IsNotNull(propertyVMs, nameof(propertyVMs));
        EAssert.Argument.IsNotNull(checkListViews, nameof(checkListViews));
        EAssert.Argument.IsNotNull(simObject, nameof(simObject));

        this.propertyVMs = propertyVMs;

        this.all = checkListViews;
        this.all.ForEach(q => q.CreateRuntime(this.propertyVMs));
        this.current = checkListViews.First();
        this.active.Add(current);
        this.all.ForEach(q => q.RunTime.IsActive = active.Contains(q));

        this.isAutoplayingEnabled = useAutoplay;
        this.simObject = simObject;

        this.playbackManager = new(this.current,
          new PlaybackManagerSettings()
          {
            readConfirmations = readConfirmations,
            pausedAlertIntervalIfUsed = pausedAlertIntervalIfUsed,
            isPlayPerItemEnabled = isPlayPerItemEnabled
          });
        this.playbackManager.ChecklistPlayingCompleted += PlaybackManager_ChecklistPlayingCompleted;
      }

      private void PlaybackManager_ChecklistPlayingCompleted()
      {
        this.previous = this.current;
        var nextActiveViews = this.current.CheckList.NextChecklists.Select(q => all.Single(p => p.CheckList == q));
        EAssert.IsTrue(nextActiveViews.Any(), "There must be at least one next checklist.");
        this.active.Clear();
        this.active.AddRange(nextActiveViews);
        this.all.ForEach(q => q.RunTime.IsActive = active.Contains(q));
        nextActiveViews.ForEach(q => q.RunTime.ResetEvaluator());
        if (previous == this.all.Last())
        {
          this.all.ForEach(q => q.RunTime.State = RunState.NotYet);
          this.all.SelectMany(q => q.Items).ForEach(q => q.RunTime.State = RunState.NotYet);
        }
        this.current = nextActiveViews.First();
        this.playbackManager.SetCurrent(this.current);
      }

      public void Reset()
      {
        this.current.RunTime.ResetEvaluator();
        this.playbackManager.Reset();
      }

      internal void SkipToNext()
      {
        var nextActive = current.CheckList.NextChecklists;
        var nextActiveViews = this.all.Where(q => nextActive.Contains(q.CheckList));
        this.active.Clear();
        this.active.AddRange(nextActiveViews);
        this.all.ForEach(q => q.RunTime.IsActive = active.Contains(q));
        nextActiveViews.ForEach(q => q.RunTime.ResetEvaluator());

        this.previous = this.current;
        this.current = nextActiveViews.First();
        this.playbackManager.SetCurrent(this.current);
        this.playbackManager.Play();
      }

      internal void SkipToPrev()
      {
        if (playbackManager.IsPartlyPlayed)
          playbackManager.Reset();
        else if (this.previous != null)
        {
          this.current = this.previous;
          playbackManager.SetCurrent(this.previous);
          this.active.Clear();
          this.active.Add(this.previous);
          this.all.ForEach(q => q.RunTime.IsActive = active.Contains(q));
          this.previous.RunTime.ResetEvaluator();
          this.previous = this.all.FirstOrDefault(q => q.CheckList.NextChecklists.First() == this.previous.CheckList); // tries to get previous of previous;
        }
        this.playbackManager.Play();
      }

      internal void TogglePlay()
      {
        this.playbackManager.TogglePlay();
      }

      internal void CheckForActiveChecklistsIfRequired()
      {
        if (playbackManager.IsPlaying) return;
        if (!isAutoplayingEnabled) return;
        if (simObject.IsSimPaused) return;

        CheckListVM? readyCheckList = this.active
          .Where(q => q.CheckList.Trigger != null)
          .Where(q => q.RunTime.CanBeAutoplayed)
          .FirstOrDefault(q => q.RunTime.Evaluate(q.CheckList.Trigger!));

        if (readyCheckList != null)
        {
          if (this.current != readyCheckList)
          {
            this.current.RunTime.State = RunState.NotYet;
            this.current = readyCheckList;
            this.current.RunTime.State = RunState.Current;
          }

          this.playbackManager.SetCurrent(readyCheckList);
          this.playbackManager.TogglePlay();
        }
      }

      internal void SetCurrentChecklist(CheckListVM vm)
      {
        this.current = vm;
        playbackManager.SetCurrent(vm);
        this.active.Clear();
        this.active.Add(vm);
        this.all.ForEach(q => q.RunTime.IsActive = active.Contains(q));

        if (this.previous != null)
        {
          this.previous.RunTime.ResetEvaluator();
          this.previous = this.all.FirstOrDefault(q => q.CheckList.NextChecklists.First() == this.previous.CheckList); // tries to get previous of previous;
        }
      }
    }
  }
}
