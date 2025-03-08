using System.Collections.Generic;
using System;
using UnityEngine;

namespace Training
{
	[CreateAssetMenu(fileName = "MusicData", menuName = "Audio/MusicData")]
	public class MusicData : ScriptableObject
    {
		[Serializable]
		private class TrackInfo
		{
			public AudioClip clip;
			public string author;
		}

		private string NoAuthorHolder = "no author";

		[SerializeField] 
		private List<TrackInfo> tracks = new();

		private Dictionary<AudioClip, string> _trackDictionary;

		private void OnEnable()
		{
			_trackDictionary = new();
			
			foreach (var track in tracks)
			{
				if (track.clip != null && !_trackDictionary.ContainsKey(track.clip))
				{
					_trackDictionary.Add(track.clip, track.author);
				}
			}
		}

		public string GetAuthor(AudioClip clip)
		{
			return _trackDictionary.TryGetValue(clip, out string author) ? author : NoAuthorHolder;
		}
	}
}