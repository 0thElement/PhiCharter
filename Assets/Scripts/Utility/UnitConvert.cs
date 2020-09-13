using Phi.Chart.Component;

namespace Phi.Utility
{
    public static class PhiUnitConvert
    {
        public static float timeToSecond(float time, float bpm)
        {
            return time * 1.875f / bpm;
        }
        public static float secondToTime(float second, float bpm)
        {
            return second * bpm / 1.875f;
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
