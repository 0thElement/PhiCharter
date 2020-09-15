using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Phi.Chart.UI
{
    [RequireComponent(typeof(Slider))]
    public class SliderManager : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
    {
        public static SliderManager Instance { get; private set; }

        private Slider Slider;
        private void Awake()
        {
            Instance = this;
            Slider = gameObject.GetComponent<Slider>();
        }
        private bool enable;
        public bool Enable
        {
            get
            {
                return enable;
            }
            set
            {
                enable = value;
                Slider.interactable = value;
                if (value == false) Slider.value = 0;
            }
        }
        public float Length { set {Slider.maxValue = value;} }

        private bool holding = false;
        private void Update()
        {
            if (!holding) Slider.value = AudioManager.Instance.Timing;
        }
        public void OnPointerDown (PointerEventData eventData)
        {
            holding = true;
        }
        public void OnPointerUp (PointerEventData eventData)
        {
            holding = false;
        }
        public void OnDrag (PointerEventData eventData)
        {
            AudioManager.Instance.Timing = Slider.value;
        }
    }
}
