using System.Collections.Generic;
using UnityEngine;
using Phi.Chart.Component;
using Phi.Chart.UI;

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
        public GameObject[] NotePrefab;
        public Transform ComposingField;
        public List<PhiJudgeLine> judgeLines;

        private float time;
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
            // Upon loading a new chart, refresh the chart's judgeline lists

            judgeLines = new List<PhiJudgeLine>();
            foreach (var judgeline in PhiChartFileReader.Instance.CurrentChart.JudgeLineList)
            {
                judgeLines.Add(judgeline);
                judgeline.Instantiate();
            }

            IsPlaying = true;//
            SliderManager.Instance.Enable = true;//
            SliderManager.Instance.Length = AudioManager.Instance.Clip.length;//
        }
        private void UpdateJudgeLines()
        {
            time = AudioManager.Instance.Timing;
            foreach (var judgeline in judgeLines)
            {
                judgeline.Update(time);
            }
        }
    }
}
