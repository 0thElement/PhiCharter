using System;
using System.Collections.Generic;
using UnityEngine;
using Phi.Chart.View;
using Phi.Utility;

namespace Phi.Chart.Component
{
    public class PhiNote : IComparable<PhiNote>
    {
        private PhiJudgeLine JudgeLine;
        public int type { get; set; }
        public float time { get; set; }
        public float positionX { get; set; }
        public float holdTime { get; set; }
        public float speed { get; set; }
        public float floorPosition { get; set; }
        protected bool isHighlighted;
        public bool IsHighlighted
        {
            get
            {
                return isHighlighted;
            }
            set
            {
                isHighlighted=value;
                NoteRenderer.sprite = isHighlighted ? PhiChartViewManager.Instance.NoteHighlightSprite[type-1]: PhiChartViewManager.Instance.NoteSprite[type-1];
            }
        }

        private SpriteRenderer NoteRenderer;
        private List<JudgeEffect> JudgeEffects = new List<JudgeEffect>();
        private Transform Container;
        private Transform NoteTransform;
        private Vector2 currentPosition;

        //Overloading operator
        public int CompareTo(PhiNote other)
        {
            return time.CompareTo(other.time);
        }
        public static bool operator >  (PhiNote operand1, PhiNote operand2)
        {
        return operand1.CompareTo(operand2) == 1;   
        }
        public static bool operator <  (PhiNote operand1, PhiNote operand2)
        {
        return operand1.CompareTo(operand2) == -1;
        }
        public static bool operator >=  (PhiNote operand1, PhiNote operand2)
        {
        return operand1.CompareTo(operand2) >= 0;
        }
        public static bool operator <=  (PhiNote operand1, PhiNote operand2)
        {
        return operand1.CompareTo(operand2) <= 0;
        }

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

                IsHighlighted = false;

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
            if (type==3) Instance = UnityEngine.Object.Instantiate(PhiChartViewManager.Instance.HoldPrefab, Container);
                    else Instance = UnityEngine.Object.Instantiate(PhiChartViewManager.Instance.NotePrefab, Container);
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

        public bool IsItself(PhiNote other)
        {
            if (other==null) return false;
            return this.instance.GetInstanceID() == other.Instance.GetInstanceID();
        }
    }
}
