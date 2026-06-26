using UnityEngine;

namespace App.Services
{
    // Сервис воспроизведения музыки и звуковых эффектов
    // Предоставляет единый интерфейс для работы со звуком в игре
    public class AudioService : IAudioService
    {
        [SerializeField] private AudioClip testClip;

        // Источник фоновой музыки
        private AudioSource _musicSource;

        // Источник звуковых эффектов
        private AudioSource _sfxSource;

        // Инициализирует аудиоисточники
        public void Initialize(AudioSource musicSource, AudioSource sfxSource)
        {
            _musicSource = musicSource;
            _sfxSource = sfxSource;
        }

        // Запускает музыку по кругу
        public void PlayMusic(AudioClip clip)
        {
            if (_musicSource == null || clip == null)
                return;

            _musicSource.clip = clip;
            _musicSource.loop = true;
            _musicSource.Play();
        }

        // Проигрывает музыкальный трек один раз
        public void PlayMusicOnce(AudioClip clip)
        {
            if (clip == null)
                return;

            // Используем основной источник музыки
            if (_musicSource != null)
            {
                _musicSource.Stop();

                _musicSource.loop = false;
                _musicSource.clip = clip;
                _musicSource.mute = false;

                _musicSource.Play();

                Debug.Log(
                    $"AudioService: PlayMusicOnce '{clip.name}' " +
                    $"(length {clip.length:F1}s).");

                return;
            }

            // Резервный вариант через источник звуковых эффектов
            if (_sfxSource != null)
            {
                _sfxSource.PlayOneShot(clip);

                Debug.Log(
                    $"AudioService: PlayMusicOnce via SFX '{clip.name}'.");
            }
            else
            {
                Debug.LogWarning(
                    "AudioService: PlayMusicOnce failed — no audio sources.");
            }
        }

        // Проигрывает звуковой эффект
        public void PlaySFX(AudioClip clip)
        {
            if (_sfxSource == null || clip == null)
                return;

            _sfxSource.PlayOneShot(clip);
        }

        // Тестовое воспроизведение звука
        private void PlayTestSFX()
        {
            if (_sfxSource == null)
                return;

            _sfxSource.PlayOneShot(testClip);
        }

        // Проигрывает 3D звук в указанной точке игрового мира
        public void PlaySFXAtPoint(AudioClip clip, Vector3 position)
        {
            if (clip == null)
                return;

            // Создаем временный объект для воспроизведения звука
            GameObject go = new GameObject("TempAudio");

            go.transform.position = position;

            var source = go.AddComponent<AudioSource>();

            source.clip = clip;

            // Переводим звук в 3D режим
            source.spatialBlend = 1f;

            source.Play();

            // Удаляем объект после окончания воспроизведения
            Object.Destroy(go, clip.length);
        }
    }
}