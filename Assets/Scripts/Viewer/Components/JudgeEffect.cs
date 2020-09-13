using System.Collections;
using UnityEngine;

namespace Phi.Chart.Component
{
    public class JudgeEffect : MonoBehaviour
    {
        public bool Available { get; set; } = true;
        public ParticleSystem Particle;
        public Animator Animator;

        public void PlayAt(Vector2 pos)
        {
            if (!Available) return;
            Available = false;
            transform.position = pos;
            Animator.enabled = true;
            Particle.Play();
            Animator.Play("Base Layer.Particle", -1, 0f);
            StartCoroutine(WaitForEnd());
        }
        IEnumerator WaitForEnd()
        {
            yield return new WaitForSeconds(Animator.GetCurrentAnimatorClipInfo(0).Length);
            Particle.Stop();
            Particle.Clear();
            Animator.enabled = false;
            yield return null;
            Available = true;
        }
    }
}