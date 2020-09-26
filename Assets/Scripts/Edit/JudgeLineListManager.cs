using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Phi.Chart.View;
using Phi.Chart.Component;

namespace Phi.Chart.Editor
{
    public class JudgeLineListManager : MonoBehaviour
    {
        public static JudgeLineListManager Instance { get; private set; }
        
        private void Awake()
        {
            Instance = this;
        }

        public RectTransform Viewport;
        public GameObject JudgeLineEntryPrefab;

        [HideInInspector]
        public List<JudgeLineEntry> Buttons = new List<JudgeLineEntry>();

        public void CreateList()
        {
            int index = 0;
            foreach (PhiJudgeLine judgeline in PhiChartFileReader.Instance.CurrentChart.JudgeLineList) {

                GameObject newEntryObject = UnityEngine.Object.Instantiate(JudgeLineEntryPrefab, Viewport);
                JudgeLineEntry newEntry = newEntryObject.GetComponent<JudgeLineEntry>();
                newEntry.Instantiate(index, judgeline);

                Buttons.Add(newEntry);

                index+=1;
            }
        }
    }
}
