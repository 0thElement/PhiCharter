using System;
using System.Collections.Generic;
using UnityEngine;
using Phi.Chart.View;
using Phi.Utility;

namespace Phi.Chart.Component
{
    public class PhiNote
    {
        private PhiJudgeLine JudgeLine;

        //Inputted Note Properties
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
                NoteRenderer.sprite = isHighlighted ? PhiChartView.Instance.NoteHighlightSprite[type-1]: PhiChartView.Instance.NoteSprite[type-1];
            }
        }
        
        //Scene interactions
        private SpriteRenderer NoteRenderer;
        private List<PhiJudgeEffect> JudgeEffects = new List<PhiJudgeEffect>();
        private Transform Container;
        private Transform NoteTransform;
        private Vector2 currentPosition;

        private bool enabled = false;
        public bool Enabled
        {
            get
            {
                return enabled;
            }
            set
            {
                enabled=value;
                NoteRenderer.enabled=value;
            }
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
                //Get object's component
                NoteTransform = instance.GetComponent<Transform>();
                NoteRenderer = instance.GetComponent<SpriteRenderer>();

                //Initialize properties
                IsHighlighted = false;
                currentPosition.y = floorPosition;
                currentPosition.x = positionX * 64;
                if (type==3) NoteRenderer.size = new Vector2(160, (PhiUnitConvert.timeToFloorPosition(time+holdTime, JudgeLine) - floorPosition)*360);

                //Set up judge effect objects
                float timeStep = PhiUnitConvert.secondToTime(0.15f, JudgeLine.bpm);
                for (float t=0; t<=holdTime; t+=timeStep)
                {
                    PhiJudgeEffect judgeEffect = UnityEngine.Object.Instantiate(PhiChartView.Instance.JudgeEffectPrefab, PhiChartView.Instance.JudgeEffectLayer).GetComponent<PhiJudgeEffect>();
                    judgeEffect.Animator.enabled=false;
                    JudgeEffects.Add(judgeEffect);
                }
            }
        }
        
        //Constructor
        public PhiNote(float time)
        {
            type = 1;
            this.time = time;
            positionX = 0;
            holdTime = 0;
            speed = 1;
            floorPosition=0;
        }

        public void Instantiate (int type, PhiJudgeLine parent, Transform Container)
        {
            JudgeLine = parent;
            if (type==3) Instance = UnityEngine.Object.Instantiate(PhiChartView.Instance.HoldPrefab, Container);
                    else Instance = UnityEngine.Object.Instantiate(PhiChartView.Instance.NotePrefab, Container);
        }
        public void Update(float currentFloorPosition, float currenttime)
        {
            currentPosition.y = (floorPosition - currentFloorPosition) * 360;
            NoteTransform.localPosition = currentPosition;
        }

        public void Judge (float currenttime)
        {
            int indexToPlay;
            if (type!=3) indexToPlay=0;
            else indexToPlay = (int)((currenttime - time) / holdTime * (JudgeEffects.Count-1));
            if (indexToPlay >= JudgeEffects.Count) return;
            JudgeEffects[indexToPlay].PlayAt(JudgeLine.JudgeLineTransform.TransformPoint(new Vector2(currentPosition.x, 0)));
        }

        public bool IsTheSameAs (PhiNote other)
        {
            if (other==null) return false;
            return this.instance.GetInstanceID() == other.Instance.GetInstanceID();
        }
    }


    public class CompareNoteByTime : IComparer<PhiNote>
    {
        public int Compare(PhiNote x, PhiNote y)
        {
            return x.time.CompareTo(y.time);
        }
    }
    public class CompareNoteByTimeAndHoldTime : IComparer<PhiNote>
    {
        public int Compare(PhiNote x, PhiNote y)
        {
            return (x.time+x.holdTime).CompareTo(y.time+y.holdTime);
        }
    }
}
