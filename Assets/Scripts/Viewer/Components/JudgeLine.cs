using System;
using System.Collections.Generic;
using UnityEngine;
using Phi.Utility;
using Phi.Chart.View;

namespace Phi.Chart.Component
{
    public class SpeedEvent    {
        public float startTime { get; set; } 
        public float endTime { get; set; } 
        public float value { get; set; } 
        public float floorPosition { get; set; }
    }

    public class JudgeLineDisappearEvent    {
        public float startTime { get; set; } 
        public float endTime { get; set; } 
        public float start { get; set; } 
        public float end { get; set; } 
        public float start2 { get; set; } 
        public float end2 { get; set; } 
    }

    public class JudgeLineMoveEvent    {
        public float startTime { get; set; } 
        public float endTime { get; set; } 
        public float start { get; set; } 
        public float end { get; set; } 
        public float start2 { get; set; } 
        public float end2 { get; set; } 
    }

    public class JudgeLineRotateEvent    {
        public float startTime { get; set; } 
        public float endTime { get; set; } 
        public float start { get; set; } 
        public float end { get; set; } 
        public float start2 { get; set; } 
        public float end2 { get; set; } 
    }

    public class PhiJudgeLine
    {
        public int numOfNotes { get; set; } 
        public int numOfNotesAbove { get; set; } 
        public int numOfNotesBelow { get; set; } 
        public float bpm { get; set; } 
        public List<SpeedEvent> speedEvents { get; set; } 
        public List<PhiNote> notesAbove { get; set; } 
        public List<PhiNote> notesBelow { get; set; } 
        public List<JudgeLineDisappearEvent> judgeLineDisappearEvents { get; set; } 
        public List<JudgeLineMoveEvent> judgeLineMoveEvents { get; set; } 
        public List<JudgeLineRotateEvent> judgeLineRotateEvents { get; set; } 

        public Transform JudgeLineTransform;
        private LineRenderer JudgeLineRenderer;
        public Transform NoteAboveLayer;
        public Transform NoteBelowLayer;

        private Color currentColor = new Color(1f, 1f, 1f, 1f);
        private Vector2 currentPosition = new Vector2(0f, 0f);
        private float currentRotation = 0;

        public PhiJudgeLine()
        {
            numOfNotes = numOfNotesAbove = numOfNotesBelow = 0;
            bpm = 100;
            speedEvents = new List<SpeedEvent>();
            judgeLineDisappearEvents = new List<JudgeLineDisappearEvent>();
            judgeLineMoveEvents = new List<JudgeLineMoveEvent>();
            judgeLineRotateEvents = new List<JudgeLineRotateEvent>();
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
                JudgeLineTransform = instance.GetComponent<Transform>();
                JudgeLineRenderer = instance.GetComponent<LineRenderer>();
                NoteAboveLayer = JudgeLineTransform.GetChild(0);
                NoteBelowLayer = JudgeLineTransform.GetChild(1);
            }
        }

        public void Instantiate()
        {
            Instance = UnityEngine.Object.Instantiate(PhiChartViewManager.Instance.JudgeLinePrefab, PhiChartViewManager.Instance.Playground);

            //Load all notes
            foreach (var note in notesAbove)
            {
                note.Instantiate(note.type, this, NoteAboveLayer);
            }
            foreach (var note in notesBelow)
            {
                note.Instantiate(note.type, this, NoteBelowLayer);
            }
        }

        private void UpdateValues(float time)
        {
            foreach (var opacityevent in judgeLineDisappearEvents)
            {
                if (opacityevent.startTime<=time & time<=opacityevent.endTime)
                {
                    float dTime        = time - opacityevent.startTime;
                    float duration     = opacityevent.endTime - opacityevent.startTime;
                    float opacityRange = opacityevent.end - opacityevent.start;
                    float dOpacity     = dTime/duration * opacityRange;

                    currentColor.a = opacityevent.start + dOpacity;

                    continue;
                }
            }   
            foreach (var moveevent in judgeLineMoveEvents)
            {
                if (moveevent.startTime<=time & time<=moveevent.endTime)
                {
                    Vector2 startPos = new Vector2(moveevent.start-0.5f, moveevent.start2-0.5f);
                    Vector2 endPos = new Vector2(moveevent.end-0.5f, moveevent.end2-0.5f);
                    float dTime = time - moveevent.startTime;
                    float duration = moveevent.endTime-moveevent.startTime;
                    Vector2 posRange = endPos - startPos;
                    Vector2 dPos = dTime/duration * posRange;

                    currentPosition = (startPos+dPos);
                    currentPosition.y=currentPosition.y*720;
                    currentPosition.x=currentPosition.x*1280;

                    continue;
                }
            }
            foreach (var rotateevent in judgeLineRotateEvents)
            {
                if (rotateevent.startTime<=time & time<=rotateevent.endTime)
                {
                    float dTime      = time - rotateevent.startTime;
                    float duration   = rotateevent.endTime - rotateevent.startTime;
                    float angleRange = rotateevent.end - rotateevent.start;
                    float dAngle     = dTime/duration * angleRange;

                    currentRotation = rotateevent.start + dAngle;

                    continue;
                }
            }
        }
        public void Update(float second)
        {
            //Update self's state
            float time=PhiUnitConvert.secondToTime(second, bpm);
            UpdateValues(time);
            JudgeLineRenderer.startColor = JudgeLineRenderer.endColor = currentColor;
            JudgeLineTransform.localEulerAngles = new Vector3(0, 0, currentRotation);
            JudgeLineTransform.localPosition = currentPosition;

            //Update all contained notes
            float currentFloorPosition = PhiUnitConvert.timeToFloorPosition(time, this);
            foreach (var note in notesAbove)
            {
                if (note.Enable & time >= note.time) note.Judge(time);
                if (time>note.time + note.holdTime || (note.floorPosition -  currentFloorPosition) > 5)
                {
                    note.Enable=false;
                    continue;
                }
                note.Enable=true;
                note.Update(currentFloorPosition, time);
            }
            foreach (var note in notesBelow)
            {
                if (note.Enable & time >= note.time) note.Judge(time);
                if (time>note.time + note.holdTime || (note.floorPosition -  currentFloorPosition) > 5)
                {
                    note.Enable=false;
                    continue;
                }
                note.Enable=true;
                note.Update(currentFloorPosition, time);
            }
        }
    }
}
