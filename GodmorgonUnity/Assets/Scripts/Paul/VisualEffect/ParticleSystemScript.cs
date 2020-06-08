using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GodMorgon.VisualEffect
{
    /**
     * Manage one visual effect that can use one or several particle system
     */
    public class ParticleSystemScript : MonoBehaviour
    {
        [SerializeField]
        private List<ParticleSystem> particleSystemList = new List<ParticleSystem>();

        //Launch all the particle effect
        public void launchParticle()
        {
            foreach(ParticleSystem particle in particleSystemList)
            {
                if(particle != null) particle.Play();
            }
        }

        //Stop all the particle effect
        public void stopParticle()
        {
            foreach (ParticleSystem particle in particleSystemList)
            {
                particle.Stop();
            }
        }

        //Return the duration of the longest particle system in the list
        public float GetDuration()
        {
            float duration = 0;
            foreach (ParticleSystem particle in particleSystemList)
            {
                if (duration < particle.main.duration)
                    duration = particle.main.duration;
            }

            return duration;
        }

        //play the particle once and destroy gameObject at the end
        public void PlayNDestroy()
        {
            StartCoroutine(PlayNDestroyCoroutine());
        }

        public IEnumerator PlayNDestroyCoroutine()
        {
            foreach (ParticleSystem particle in particleSystemList)
            {
                particle.Play();
            }
            yield return new WaitForSeconds(GetDuration());

            Destroy(this);
        }
    }
}