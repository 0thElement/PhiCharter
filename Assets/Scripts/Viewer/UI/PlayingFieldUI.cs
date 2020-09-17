using UnityEngine;
using UnityEngine.UI;
using Phi.Utility;
using Phi.Chart.Component;

namespace Phi.Chart.View
{
    public class PlayingFieldUI : MonoBehaviour
    {
        public static PlayingFieldUI Instance { get; private set; }

        private void Awake() {
            Instance = this;
        }

        public Text ComboCounter;
        public Text ScoreCounter;

        private void Update()
        {
            UpdateScoring();
        }

        private void UpdateScoring()
        {
            float scorePerNote = 1000000 / PhiChartFileReader.Instance.CurrentChart.numOfNotes;
            int combo = GetCombo();
            ComboCounter.text = combo.ToString();
            ScoreCounter.text = Mathf.RoundToInt(combo * scorePerNote).ToString("D7");
       }

        private int GetCombo()
        {
            float second = AudioManager.Instance.Timing;
            CompareNoteByTimeAndHoldTime comparer = new CompareNoteByTimeAndHoldTime();
            int combo=0;
            foreach (var judgeline in PhiChartFileReader.Instance.CurrentChart.JudgeLineList)
            {
                float time = PhiUnitConvert.secondToTime(second, judgeline.bpm);
                PhiNote dummyNote = new PhiNote(time);
                
                int found = judgeline.notesAbove.BinarySearch(dummyNote, comparer);
                //If current timing doesn't matches exactly with one note BinarySearch returns the bitwise complement of index of the next note
                combo += (found >= 0) ? found + 1 : -found - 1;

                found = judgeline.notesBelow.BinarySearch(dummyNote, comparer);
                combo += (found >= 0) ? found + 1 : -found - 1;
                // Debug.Log(dummyNote.time + " " + found + " " + combo + " " + judgeline.numOfNotes);
            }
            return combo;
        }
    }  
}
