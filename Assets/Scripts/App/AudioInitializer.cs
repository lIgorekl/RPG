using UnityEngine;
using App;

namespace App
{
    public class AudioInitializer : MonoBehaviour
    {
        [SerializeField] private AudioSource musicSource;
        [SerializeField] private AudioSource sfxSource;
        [SerializeField] private AudioClip menuMusic;

        private void Start()
        {
            var audioService = GameEntryPoint.Instance.GetAudioService();
            audioService.Initialize(musicSource, sfxSource);
            audioService.PlayMusic(menuMusic);
        }
    }
}