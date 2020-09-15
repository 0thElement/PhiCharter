using System.Collections.Generic;
using UnityEngine;

namespace Phi.Chart.Component
{
    public class PhiChart
    {
        public float formatversion { get; set; }
        public float offset { get; set; }
        public float numOfNotes { get; set; }
        public List<PhiJudgeLine> JudgeLineList { get; set; }
    }
}