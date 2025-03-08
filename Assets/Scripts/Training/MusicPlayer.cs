using UnityEngine;

namespace Training
{
	[RequireComponent(typeof(MusicPlayerModel))]
    public class MusicPlayer : MonoBehaviour
    {
        private AudioManager AudioManager => _audioManager ??= Dependency.Get<AudioManager>();
        private AudioManager _audioManager;

		[SerializeField] 
		private MusicData musicData;
		[SerializeField]
		private MusicPlayerModel musicPlayerModel;



		private void OnValidate()
		{
			musicPlayerModel ??= GetComponent<MusicPlayerModel>();
		}

		private void Start()
		{
			AudioManager.OnTrackChanged += OnTrackChanged;
		}

		private void Destroy()
		{
			AudioManager.OnTrackChanged -= OnTrackChanged;
		}



		public void SetVolume(float volume)
		{
			AudioManager.SetVolume(volume);
		}

		public void PlayNext()
        {
            AudioManager.PlayNextTrack();
        }

		public void PreviousTrack()
		{
			AudioManager.PlayPreviousTrack();
		}

		public void SetPaused(bool isPaused)
		{
			if (isPaused)
			{
				AudioManager.Pause();
			}
			else
			{
				AudioManager.Play();
			}
		}

		public void PlayRandom()
		{
			AudioManager.PlayRandom();
		}

		private void OnTrackChanged(AudioClip clip)
		{
			if (clip == null)
			{
				return;
			}

			string trackTitle = clip.name;
			string author = musicData.GetAuthor(clip);
			musicPlayerModel.UpdateTrackInfo(trackTitle, author);
		}
	}
}