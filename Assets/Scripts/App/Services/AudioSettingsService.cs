using UnityEngine;

namespace App.Services
{
    public class AudioSettingsService
    {
        private float _musicVolume = 1f;
        private float _sfxVolume = 1f;

        public void SetMusicVolume(float volume)
        {
            _musicVolume = Mathf.Clamp01(volume);
        }

        public void SetSFXVolume(float volume)
        {
            _sfxVolume = Mathf.Clamp01(volume);
        }

        public float GetMusicVolume()
        {
            return _musicVolume;
        }

        public float GetSFXVolume()
        {
            return _sfxVolume;
        }
    }
}