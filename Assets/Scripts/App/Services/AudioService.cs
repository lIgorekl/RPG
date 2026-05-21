using UnityEngine;

namespace App.Services
{
    public class AudioService : IAudioService
    {
        [SerializeField] private AudioClip testClip;

        private AudioSource _musicSource;
        private AudioSource _sfxSource;

        public void Initialize(AudioSource musicSource, AudioSource sfxSource)
        {
            _musicSource = musicSource;
            _sfxSource = sfxSource;
        }

        public void PlayMusic(AudioClip clip)
        {
            if (_musicSource == null || clip == null)
                return;

            _musicSource.clip = clip;
            _musicSource.loop = true;
            _musicSource.Play();
        }

        public void PlaySFX(AudioClip clip)
        {
            if (_sfxSource == null || clip == null)
                return;

            _sfxSource.PlayOneShot(clip);
        }

        private void PlayTestSFX()
        {
            if (_sfxSource == null)
                return;

            // короткий бип (или замени на свой звук)
            _sfxSource.PlayOneShot(testClip);
        }

        public void PlaySFXAtPoint(AudioClip clip, Vector3 position)
        {
            if (clip == null) return;

            GameObject go = new GameObject("TempAudio");
            go.transform.position = position;

            var source = go.AddComponent<AudioSource>();
            source.clip = clip;
            source.spatialBlend = 1f; // 3D звук

            source.Play();

            Object.Destroy(go, clip.length);
        }
    }
}