using UnityEngine;

namespace App.Services
{
    public interface IAudioService
    {
        void Initialize(AudioSource musicSource, AudioSource sfxSource);

        void PlayMusic(AudioClip clip);
        void PlayMusicOnce(AudioClip clip);
        void PlaySFX(AudioClip clip);

        void PlaySFXAtPoint(AudioClip clip, Vector3 position);
    }
}