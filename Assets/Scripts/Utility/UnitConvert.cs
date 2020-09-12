using Phi.Chart.Component;
using System.Collections.Generic;

namespace Phi.Utility
{
    public static class PhiUnitConvert
    {
        public static float timeToMs(float time, float bpm)
        {
            return time * 1.875f / bpm;
        }
        public static float msToTime(float ms, float bpm)
        {
            return ms * bpm / 1.875f;
        }

        public static float timeToFloorPosition(float time, PhiJudgeLine judgeLine)
        {
            foreach (SpeedEvent speedevent in judgeLine.speedEvents)
            {
                if (speedevent.startTime<=time & time <=speedevent.endTime)
                {
                    //don't ask why that weird constant is there. it's just how it is
                    return (time - speedevent.startTime) * speedevent.value * 0.00815217380134062208f + speedevent.floorPosition;
                }
            }
            return -1;
        }
    }
}
