using Gameplay.Combat.Boss;
using UnityEngine;

namespace Presentation.Combat
{
    public class BossElementVisualController : MonoBehaviour
    {
        [SerializeField] private ParticleSystem[] particleSystems;

        public void ApplyElement(BossElementConfig config)
        {
            if (config == null)
                return;

            var color = config.ParticleColor;

            foreach (var particleSystem in ResolveParticleSystems())
            {
                if (particleSystem == null)
                    continue;

                var main = particleSystem.main;
                main.startColor = color;

                var renderer =
                    particleSystem.GetComponent<ParticleSystemRenderer>();

                if (renderer != null && renderer.material != null)
                    renderer.material.color = color;

                if (!particleSystem.isPlaying)
                    particleSystem.Play();
            }
        }

        private ParticleSystem[] ResolveParticleSystems()
        {
            if (particleSystems != null && particleSystems.Length > 0)
                return particleSystems;

            return GetComponentsInChildren<ParticleSystem>(true);
        }
    }
}
