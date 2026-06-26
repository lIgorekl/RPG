using UnityEngine;

namespace App.Services
{
    // Сервис настроек громкости
    // Хранит значения громкости музыки и звуковых эффектов
    public class AudioSettingsService
    {
        // Текущая громкость музыки
        private float _musicVolume = 1f;

        // Текущая громкость звуковых эффектов
        private float _sfxVolume = 1f;

        // Устанавливает громкость музыки
        public void SetMusicVolume(float volume)
        {
            _musicVolume = Mathf.Clamp01(volume);
        }

        // Устанавливает громкость звуковых эффектов
        public void SetSFXVolume(float volume)
        {
            _sfxVolume = Mathf.Clamp01(volume);
        }

        // Возвращает текущую громкость музыки
        public float GetMusicVolume()
        {
            return _musicVolume;
        }

        // Возвращает текущую громкость звуковых эффектов
        public float GetSFXVolume()
        {
            return _sfxVolume;
        }
    }
}