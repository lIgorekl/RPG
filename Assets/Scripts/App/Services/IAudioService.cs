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

        float GetMusicVolume();
        float GetSfxVolume();
    }
}