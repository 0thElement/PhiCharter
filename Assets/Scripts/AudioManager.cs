using UnityEngine;
using Phi.Chart.UI;

namespace Phi.Chart
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        } 
        public AudioSource Source;
        public float Timing
        {
            get
            {
                return Source.time;
            }
            set
            {
                Source.time = value;
            }
        }
        public AudioClip Clip
        {
            get
            {
                return Source.clip;
            }
            set
            {
                Source.clip=value;
            }
        }
        public void LoadClip(AudioClip toLoad)
        {
            Clip = toLoad;
            SliderManager.Instance.Length = toLoad.length;
            Clip.LoadAudioData();
        }
        public void Play()
        {
            Source.Play();
        }
        public void Pause()
        {
            Source.Pause();
        }
    }
}