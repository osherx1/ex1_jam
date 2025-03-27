﻿using System.Collections;
using Managers;
using UnityEngine;
namespace Sounds
{
    /// <summary>
    /// Manages sound effects and background music for the game.
    /// Singleton pattern ensures only one instance exists.
    /// </summary>
    public class SoundManager : MonoSingleton<SoundManager>
    {
        /// <summary>
        /// Reference to the ScriptableObject containing all audio clips.
        /// </summary>
        [SerializeField]
        private Data.GameSoundsSo gameSoundsSo;

        /// <summary>
        /// Audio source for playing short sound effects.
        /// </summary>
        [SerializeField]
        private AudioSource audioSource;

        /// <summary>
        /// Audio source dedicated to background music playback.
        /// </summary>
        [SerializeField]
        private AudioSource backgroundMusic;

        /// <summary>
        /// Whether to automatically start background music on Awake.
        /// </summary>
        [SerializeField]
        private bool startWithBackgroundMusic;

        /// <summary>
        /// Called when the object is initialized. Plays background music if enabled.
        /// </summary>
        void Awake()
        {
            if (startWithBackgroundMusic)
            {
                PlayBackgroundMusic();
            }
        }

        /// <summary>
        /// Plays the background music on a loop.
        /// </summary>
        public void PlayBackgroundMusic()
        {
            if (backgroundMusic != null)
            {
                backgroundMusic.loop = true;
                backgroundMusic.Play();
            }
        }

        /// <summary>
        /// Plays a sound effect based on the audio type defined in the GameSoundsSO.
        /// </summary>
        /// <param name="audioType">The type of sound to play.</param>
        public void PlaySoundByAudioType(Data.GameSoundsSo.AudioType audioType)
        {
            AudioClip clip = gameSoundsSo.GetClip(audioType);
            if (clip != null)
            {
                audioSource.PlayOneShot(clip);
            }
            else
            {
                Debug.LogWarning($"Sound {audioType} not found!");
            }
        }

        /// <summary>
        /// Plays a given AudioSource with optional volume control.
        /// </summary>
        /// <param name="audioSource">The AudioSource to play.</param>
        /// <param name="volume">The volume (default is 0.8).</param>
        public void PlaySound(AudioSource audioSource, float volume = 0.8f)
        {
            audioSource.volume = volume;
            audioSource.Play();
        }

        /// <summary>
        /// Sets the volume for background music.
        /// </summary>
        /// <param name="volume">Volume value between 0 and 1.</param>
        public void SetBackgroundMusicVolume(float volume)
        {
            if (backgroundMusic != null)
            {
                backgroundMusic.volume = Mathf.Clamp01(volume);
            }
        }

        /// <summary>
        /// Sets the volume for sound effects.
        /// </summary>
        /// <param name="volume">Volume value between 0 and 1.</param>
        public void SetSFXVolume(float volume)
        {
            if (audioSource != null)
            {
                audioSource.volume = Mathf.Clamp01(volume);
            }
        }

        /// <summary>
        /// Pauses the currently playing background music.
        /// </summary>
        public void PauseBackgroundMusic()
        {
            if (backgroundMusic.isPlaying)
            {
                backgroundMusic.Pause();
            }
        }

        /// <summary>
        /// Resumes the background music if it was paused.
        /// </summary>
        public void ResumeBackgroundMusic()
        {
            if (!backgroundMusic.isPlaying)
            {
                backgroundMusic.UnPause();
            }
        }

        /// <summary>
        /// Stops the background music completely.
        /// </summary>
        public void StopBackgroundMusic()
        {
            if (backgroundMusic.isPlaying)
            {
                backgroundMusic.Stop();
            }
        }

        /// <summary>
        /// Fades out the background music over the given duration.
        /// </summary>
        /// <param name="duration">Duration of the fade in seconds.</param>
        public void FadeOutBackground(float duration)
        {
            StartCoroutine(FadeOutCoroutine(duration));
        }

        /// <summary>
        /// Coroutine for fading out the background music.
        /// </summary>
        /// <param name="duration">Duration of the fade.</param>
        private IEnumerator FadeOutCoroutine(float duration)
        {
            float startVolume = backgroundMusic.volume;

            while (backgroundMusic.volume > 0)
            {
                backgroundMusic.volume -= startVolume * Time.deltaTime / duration;
                yield return null;
            }

            backgroundMusic.Stop();
            backgroundMusic.volume = startVolume;
        }

        /// <summary>
        /// Changes the background music to a new clip and plays it.
        /// </summary>
        /// <param name="newClip">The new AudioClip to play.</param>
        public void ChangeBackgroundMusic(AudioClip newClip)
        {
            if (backgroundMusic.clip != newClip)
            {
                backgroundMusic.clip = newClip;
                backgroundMusic.Play();
            }
        }
    }
}
