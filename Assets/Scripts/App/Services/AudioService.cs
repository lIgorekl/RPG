using UnityEngine;

namespace App.Services
{
    public class AudioService : IAudioService
    {
        [SerializeField] private AudioClip testClip;

        private AudioSource _musicSource;
        private AudioSource _sfxSource;

        private float _musicVolume;
        private float _sfxVolume;

        private const string MUSIC_KEY = "MusicVolume";
        private const string SFX_KEY = "SfxVolume";

        public float GetMusicVolume() => _musicVolume;
        public float GetSfxVolume() => _sfxVolume;

        public void Initialize(AudioSource musicSource, AudioSource sfxSource)
        {
            _musicSource = musicSource;
            _sfxSource = sfxSource;

            // Загружаем сохранённые значения
            _musicVolume = PlayerPrefs.GetFloat(MUSIC_KEY, 1f);
            _sfxVolume = PlayerPrefs.GetFloat(SFX_KEY, 1f);

            ApplyVolumes();
        }

        public void SetMusicVolume(float value)
        {
            _musicVolume = value;
            PlayerPrefs.SetFloat(MUSIC_KEY, value);
            PlayerPrefs.Save();

            ApplyVolumes();
        }

        public void SetSfxVolume(float value)
        {
            _sfxVolume = value;
            PlayerPrefs.SetFloat(SFX_KEY, value);
            ApplyVolumes();

            // ПРОИГРЫВАЕМ ТЕСТОВЫЙ ЗВУК
            PlayTestSFX();
        }

        private void ApplyVolumes()
        {
            if (_musicSource != null)
                _musicSource.volume = _musicVolume;

            if (_sfxSource != null)
                _sfxSource.volume = _sfxVolume;
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

            _sfxSource.PlayOneShot(clip, _sfxVolume);
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
            source.volume = _sfxVolume;
            source.spatialBlend = 1f; // 3D звук

            source.Play();

            Object.Destroy(go, clip.length);
        }
    }
}