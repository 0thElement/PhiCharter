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
        private SpriteRenderer HoldHeadRenderer;
        private SpriteRenderer HoldBodyRenderer;
        private SpriteRenderer HoldTailRenderer;
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
                if (type!=3) NoteRenderer.enabled=value;
                else HoldHeadRenderer.enabled = HoldBodyRenderer.enabled = HoldTailRenderer.enabled = value;
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

                if (type!=3) 
                {
                    NoteRenderer = instance.GetComponent<SpriteRenderer>();
                }
                else
                {
                    HoldHeadRenderer  = NoteTransform.GetChild(0).GetComponent<SpriteRenderer>();
                    HoldBodyRenderer  = NoteTransform.GetChild(1).GetComponent<SpriteRenderer>();
                    HoldBodyTransform = NoteTransform.GetChild(1).GetComponent<Transform>();
                    HoldTailRenderer  = NoteTransform.GetChild(2).GetComponent<SpriteRenderer>();
                    HoldTailTransform = NoteTransform.GetChild(2).GetComponent<Transform>();

                    //(Set up hold note's shape)
                    //At scaling of 1, a hold note (all 3 parts) spans 4 unit
                    //Head and tail spans 0.2 unit altogether
                    float holdEndFloorPosition = PhiUnitConvert.timeToFloorPosition(time + holdTime, JudgeLine);
                    float holdFloorPositionSpan = (holdEndFloorPosition - floorPosition)*5;

                    HoldBodyTransform.localScale    = new Vector3(1f, holdFloorPositionSpan / 3.8f, 1f);
                    HoldTailTransform.localPosition = new Vector3(0f, holdFloorPositionSpan + 0.1f, 0f);
                }
            }
        }

        public void Instantiate(int type, PhiJudgeLine parent, Transform Container)
        {
            JudgeLine = parent;
            Instance = UnityEngine.Object.Instantiate(PhiChartViewManager.Instance.NotePrefab[type-1], Container);
            currentPosition.x = positionX;
            currentPosition.y = floorPosition;
        }
        public void Update(float currentFloorPosition)
        {
            currentPosition.y = (floorPosition - currentFloorPosition) * 5;
            NoteTransform.localPosition = currentPosition;
        }
    }
}
