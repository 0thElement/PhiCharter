using Newtonsoft.Json;
using System.IO;
using UnityEngine;
using Phi.Chart.Component;

namespace Phi.Chart.View
{
    public class PhiChartFileReader : MonoBehaviour
    {
        public static PhiChartFileReader Instance { get; private set;}
        public PhiChart CurrentChart = new PhiChart();
        private void Awake()
        {
            Instance = this;
        }
        private void Start()
        {
            ReadChart();
        }
        public void ReadChart()
        {
            CurrentChart = JsonConvert.DeserializeObject<PhiChart>(File.ReadAllText(Path.Combine(Application.dataPath, "Scripts", "testChart.json")));
            PhiChartViewManager.Instance.Refresh();
        }
    }
}   