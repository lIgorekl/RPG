using UnityEngine;

namespace App.Services
{
    // Интерфейс аудиосервиса
    // Определяет методы воспроизведения музыки и звуковых эффектов
    public interface IAudioService
    {
        // Инициализирует источники звука
        void Initialize(AudioSource musicSource, AudioSource sfxSource);

        // Запускает музыку с зацикливанием
        void PlayMusic(AudioClip clip);

        // Проигрывает музыку один раз
        void PlayMusicOnce(AudioClip clip);

        // Проигрывает звуковой эффект
        void PlaySFX(AudioClip clip);

        // Проигрывает звуковой эффект в указанной точке мира
        void PlaySFXAtPoint(AudioClip clip, Vector3 position);
    }
}