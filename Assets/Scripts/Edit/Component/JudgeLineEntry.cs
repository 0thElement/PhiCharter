using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Phi.Chart.Component;

namespace Phi.Chart.Editor
{
    public class JudgeLineEntry : MonoBehaviour
    {
        private PhiJudgeLine ControlledJudgeLine;
        private bool isVisible = true;
        public bool IsVisible
        {
            get
            {
                return isVisible;
            }
            set
            {
                isVisible = value;
                ControlledJudgeLine.SetVisibility(value);
            }
        }
        public InputField NameInputField;
        public Text NoteCountText;
        public Toggle VisibilitiyToggle;
        public RectTransform Transform;
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
                Transform = instance.GetComponent<RectTransform>();
            }
        }

        public void Instantiate(int index, PhiJudgeLine judgeline)
        {
            ControlledJudgeLine = judgeline;
            Transform.localPosition = new Vector2(0, -30*index);
            UpdateInfoDisplay();
        }

        public void SetVisibility(bool option) {
            Debug.Log(option);
            IsVisible = VisibilitiyToggle.isOn;
        }
        public void UpdateInfoDisplay() {
            NameInputField.text = ControlledJudgeLine.name;
            NoteCountText.text = ControlledJudgeLine.numOfNotesAbove + " | " + ControlledJudgeLine.numOfNotesBelow;
        }
        public void UpdateValue() {
            ControlledJudgeLine.name = NameInputField.text;
        }
    }
}
