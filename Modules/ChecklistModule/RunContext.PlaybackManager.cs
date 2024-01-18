﻿using Eng.Chlaot.ChlaotModuleBase.ModuleUtils.Playing;
using Eng.Chlaot.Modules.ChecklistModule.Types.RunViews;
using ESystem.Asserting;
using System;

namespace Eng.Chlaot.Modules.ChecklistModule
{
  public partial class RunContext
  {
    public class PlaybackManager
    {
      private int currentItemIndex = 0;
      private bool isCallPlayed = false;
      private bool isEntryPlayed = false;
      private bool isMainLoopAbortRequested = false;
      private bool isMainLoopActive = false;
      private bool isCurrentLastSpeechPlaying = false;
      private readonly bool readConfirmations;
      public event Action? ChecklistPlayingCompleted;
      public bool IsWaitingForNextChecklist { get => currentItemIndex == 0 && isEntryPlayed == false; }
      public bool IsPartlyPlayed => currentItemIndex > 0;

      public PlaybackManager(CheckListView initialChecklist, bool readConfirmations)
      {
        EAssert.Argument.IsNotNull(initialChecklist, nameof(initialChecklist));
        this.readConfirmations = readConfirmations;
        this.Current = initialChecklist;
        this.SetCurrent(this.Current); // ensures correct initialization
      }

      public CheckListView Current { get; private set; }
      public bool IsPlaying { get => this.isMainLoopActive; }

      public void SetCurrent(CheckListView value)
      {
        EAssert.Argument.IsNotNull(value, nameof(value));
        // reset old one
        Current.State = RunState.Runned;

        // setting new one
        Current = value;
        Current.State = RunState.Current;
        Current.Items.ForEach(q => q.State = RunState.NotYet);
        currentItemIndex = 0;
        isEntryPlayed = false;
        isCallPlayed = false;
      }

      public void PauseAsync()
      {
        lock (this)
        {
          this.isCallPlayed = false;
          this.isEntryPlayed = false;
          this.isMainLoopAbortRequested = true;
        }
      }

      private void PlayNext()
      {
        EAssert.IsTrue(isMainLoopActive);
        EAssert.IsFalse(isMainLoopAbortRequested);
        lock (this)
        {
          byte[] playData = ResolveAndMarkNexPlayBytes(out isCurrentLastSpeechPlaying);
          Player player = new(playData);
          player.PlaybackFinished += Player_PlaybackFinished;
          player.PlayAsync();
        }
        AdjustRunStates();
      }

      private void Player_PlaybackFinished(Player sender)
      {
        lock (this)
        {
          if (isMainLoopActive)
          {
            if (isMainLoopAbortRequested)
            {
              isMainLoopActive = false;
              isMainLoopAbortRequested = false;
            }
            else if (isCurrentLastSpeechPlaying)
            {
              isMainLoopActive = false;
              isMainLoopAbortRequested = false;
              isCurrentLastSpeechPlaying = false;
              this.ChecklistPlayingCompleted?.Invoke();
            }
            else
              this.PlayNext();
          }
        }
      }

      public void Reset()
      {
        this.currentItemIndex = 0;
        this.isCallPlayed = false;
        this.isEntryPlayed = false;
        this.Current.Items.ForEach(q => q.State = RunState.NotYet);
      }

      public void Play()
      {
        lock (this)
        {
          if (!isMainLoopActive)
          {
            this.isMainLoopActive = true;
            PlayNext();
          }
        }
      }

      public void TogglePlay()
      {
        lock (this)
        {
          if (isMainLoopActive)
            PauseAsync();
          else
            Play();
        }
      }

      private void AdjustRunStates()
      {
        for (int i = 0; i < Current.Items.Count; i++)
        {
          if (i < currentItemIndex)
            Current.Items[i].State = RunState.Runned;
          else if (i > currentItemIndex)
            Current.Items[i].State = RunState.NotYet;
          else
            Current.Items[i].State = RunState.Current;
        }
        //TODO asi navíc
        //if (currentItemIndex < _Current.Items.Count)
        //  currentList.Items[currentItemIndex].State = RunState.Current;
      }



      private byte[] ResolveAndMarkNexPlayBytes(out bool isThisLastChecklistSpeech)
      {
        byte[] ret;
        if (currentItemIndex == 0 && !this.isEntryPlayed)
        {
          // playing checklist entry speech
          ret = Current.CheckList.EntrySpeechBytes;
          this.isEntryPlayed = true;
          isThisLastChecklistSpeech = false;
        }
        else if (currentItemIndex < Current.Items.Count)
        {
          // play checklist item and increase index
          if (isCallPlayed == false)
          {
            ret = Current.Items[currentItemIndex].CheckItem.Call.Bytes;
            isCallPlayed = true;

            if (!this.readConfirmations)
            {
              currentItemIndex++;
              isCallPlayed = false;
            }
          }
          else
          {
            ret = Current.Items[currentItemIndex].CheckItem.Confirmation.Bytes;
            currentItemIndex++;
            isCallPlayed = false;
          }
          isThisLastChecklistSpeech = false;
        }
        else
        {
          // playing at the end
          ret = Current.CheckList.ExitSpeechBytes;
          isThisLastChecklistSpeech = true;
        }
        return ret;
      }
    }
  }
}
