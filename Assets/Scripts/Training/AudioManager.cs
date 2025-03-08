using System.Collections.Generic;
using UnityEngine;

namespace Training
{
    public class AudioManager : MonoBehaviour
    {
		[SerializeField] 
		private AudioSource audioSource;
		[SerializeField] 
		private List<AudioClip> tracks;

		public event System.Action<AudioClip> OnTrackChanged;

		private int currentTrackIndex = 0;
		private bool _isPaused = false;

		

		private void Awake()
		{
			Dependency.Register(this);
		}

		private void OnDestroy()
		{
			Dependency.Unregister(this);
		}

		private void Update()
		{
			if (!audioSource.isPlaying && !_isPaused)
			{
				PlayNextTrack();
			}
		}



		public void PlayRandom()
		{
			int index = Random.Range(0, tracks.Count);
			PlayTrack(index);
		}

		public void PlayTrack(int index)
		{
			if (index < 0 || index >= tracks.Count)
			{
				return;
			}

			var clip = tracks[index];
			currentTrackIndex = index;
			audioSource.clip = clip;
			audioSource.Play();

			OnTrackChanged?.Invoke(clip);
		}

		public void PlayNextTrack()
		{
			PlayTrack((currentTrackIndex + 1) % tracks.Count);
		}

		public void PlayPreviousTrack()
		{
			PlayTrack((currentTrackIndex - 1 + tracks.Count) % tracks.Count);
		}

		public void Pause()
		{
			audioSource.Pause();
			_isPaused = true;
		}

		public void Play()
		{
			audioSource.Play();
			_isPaused = false;
		}

		public void SetVolume(float volume)
		{
			audioSource.volume = Mathf.Clamp01(volume);
		}
	}
}