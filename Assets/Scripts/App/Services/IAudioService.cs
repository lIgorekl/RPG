using UnityEngine;

namespace App.Services
{
    public interface IAudioService
    {
        void Initialize(AudioSource musicSource, AudioSource sfxSource);

        void SetMusicVolume(float value);
        void SetSfxVolume(float value);

        void PlayMusic(AudioClip clip);
        void PlaySFX(AudioClip clip);

        void PlaySFXAtPoint(AudioClip clip, Vector3 position);

        float GetMusicVolume();
        float GetSfxVolume();
    }
}