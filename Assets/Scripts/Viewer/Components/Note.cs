using System.Collections.Generic;
using UnityEngine;
using Phi.Chart.View;
using Phi.Utility;

namespace Phi.Chart.Component
{
    public class PhiNote
    {
        private PhiJudgeLine JudgeLine;
        public int type { get; set; }
        public float time { get; set; }
        public float positionX { get; set; }
        public float holdTime { get; set; }
        public float speed { get; set; }
        public float floorPosition { get; set; }

        private SpriteRenderer NoteRenderer;
        private List<JudgeEffect> JudgeEffects = new List<JudgeEffect>();
        private Transform Container;
        private Transform NoteTransform;
        private Transform HoldBodyTransform;
        private Transform HoldTailTransform;
        private Vector2 currentPosition;

        private bool enable = false;
        public bool Enable
        {
            get
            {
                return enable;
            }
            set
            {
                enable=value;
                NoteRenderer.enabled=value;
            }
        }

        public PhiNote()
        {
            type = 1;
            time = 0;
            positionX = 0;
            holdTime = 0;
            speed = 1;
            floorPosition=0;
        }

        protected GameObject instance;
        public GameObject Instance
        {
            get
            {
                return instance;
            }
            set
            {
                instance = value;
                NoteTransform = instance.GetComponent<Transform>();

                NoteRenderer = instance.GetComponent<SpriteRenderer>();

                if (type==3) NoteRenderer.size = new Vector2(160, (PhiUnitConvert.timeToFloorPosition(time+holdTime, JudgeLine) - floorPosition)*360);

                float timeStep = PhiUnitConvert.secondToTime(0.15f, JudgeLine.bpm);
                for (float t=0; t<=holdTime; t+=timeStep)
                {
                    JudgeEffect judgeEffect = UnityEngine.Object.Instantiate(PhiChartViewManager.Instance.JudgeEffectPrefab, PhiChartViewManager.Instance.JudgeEffectLayer).GetComponent<JudgeEffect>();
                    judgeEffect.Animator.enabled=false;
                    JudgeEffects.Add(judgeEffect);
                }
            }
        }

        public void Instantiate(int type, PhiJudgeLine parent, Transform Container)
        {
            JudgeLine = parent;
            Instance = UnityEngine.Object.Instantiate(PhiChartViewManager.Instance.NotePrefab[type-1], Container);
            currentPosition.x = positionX * 64;
            currentPosition.y = floorPosition;
        }
        public void Update(float currentFloorPosition, float currenttime)
        {
            currentPosition.y = (floorPosition - currentFloorPosition) * 360;
            NoteTransform.localPosition = currentPosition;
        }

        public void Judge(float currenttime)
        {
            int indexToPlay;
            if (type!=3) indexToPlay=0;
            else indexToPlay = (int)((currenttime - time) / holdTime * (JudgeEffects.Count-1));
            if (indexToPlay >= JudgeEffects.Count) return;
            JudgeEffects[indexToPlay].PlayAt(JudgeLine.JudgeLineTransform.TransformPoint(new Vector2(currentPosition.x, 0)));
        }
    }
}
