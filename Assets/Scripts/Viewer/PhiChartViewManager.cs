using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using Phi.Chart.Component;
using Phi.Chart.UI;
using Phi.Utility;

namespace Phi.Chart.View
{
    public class PhiChartViewManager : MonoBehaviour
    {
        public static PhiChartViewManager Instance { get; private set; }
        private void Awake()
        {
            Instance = this;
        }
        public GameObject JudgeLinePrefab;
        public GameObject NotePrefab;
        public GameObject HoldPrefab;
        public Sprite[] NoteSprite;
        public Sprite[] NoteHighlightSprite;
        public GameObject JudgeEffectPrefab;
        public Transform Playground;
        public Transform JudgeEffectLayer;
        public List<PhiJudgeLine> judgeLines;

        private float second;
        private bool isPlaying = false;
        public bool IsPlaying
        {
            get
            {
                return isPlaying;
            }
            set
            {
                isPlaying = value;
                if (!isPlaying) AudioManager.Instance.Pause();
                           else AudioManager.Instance.Play();
            }
        }

        private void Update()
        {
            if (!isPlaying || judgeLines == null) return;
            UpdateJudgeLines();
        }
        public void Refresh()
        {
            ReloadJudgeLines();
            HighlightNotes();
            Debug.Log(judgeLines[0].notesAbove[0].IsItself(judgeLines[0].notesAbove[0]));

            IsPlaying = true;//
            SliderManager.Instance.Enable = true;//
            SliderManager.Instance.Length = AudioManager.Instance.Clip.length;//
        }
        private void ReloadJudgeLines()
        {
            judgeLines = new List<PhiJudgeLine>();
            foreach (var judgeline in PhiChartFileReader.Instance.CurrentChart.JudgeLineList)
            {
                judgeLines.Add(judgeline);
                judgeline.Instantiate();
            }
        }
        private void UpdateJudgeLines()
        {
            second = AudioManager.Instance.Timing;
            foreach (var judgeline in judgeLines)
            {
                judgeline.Update(second);
            }
        }
        private void HighlightNotes()
        {
            foreach (var judgeline in judgeLines)
            {
                foreach (var note in judgeline.notesAbove)
                {
                    if (!note.IsHighlighted)
                    {
                        PhiNote sameTimingNote = FindSameTiming(note);
                        if (sameTimingNote != null)
                        {
                            note.IsHighlighted = sameTimingNote.IsHighlighted = true;
                            continue;
                        }
                    }
                }
                foreach (var note in judgeline.notesBelow)
                {
                    if (!note.IsHighlighted)
                    {
                        PhiNote sameTimingNote = FindSameTiming(note);
                        if (sameTimingNote != null)
                        {
                            note.IsHighlighted = sameTimingNote.IsHighlighted = true;
                            continue;
                        }
                    }
                }
            }
        }
        private PhiNote FindSameTiming(PhiNote note)
        {
            int foundindex;
            foreach (var judgeline in judgeLines)
            {
                foundindex = judgeline.notesAbove.BinarySearch(note);
                if (foundindex >=0)
                {
                    if (note.IsItself(judgeline.notesAbove[foundindex]))
                    {
                        try {if (note == judgeline.notesAbove[foundindex-1]) return judgeline.notesAbove[foundindex-1];} catch{};
                        try {if (note == judgeline.notesAbove[foundindex+1]) return judgeline.notesAbove[foundindex+1];} catch{};
                    } else return judgeline.notesAbove[foundindex];
                }
                foundindex = judgeline.notesBelow.BinarySearch(note);
                if (foundindex >=0)
                {
                    if (note.IsItself(judgeline.notesBelow[foundindex]))
                    {
                        try {if (note == judgeline.notesBelow[foundindex-1]) return judgeline.notesBelow[foundindex-1];} catch{};
                        try {if (note == judgeline.notesBelow[foundindex+1]) return judgeline.notesBelow[foundindex+1];} catch{};
                        continue;
                    } else return judgeline.notesBelow[foundindex];
                }
            }
            return null;
        }
    }
}
