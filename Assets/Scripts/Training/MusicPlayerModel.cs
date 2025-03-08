using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Training
{
    public class MusicPlayerModel : MonoBehaviour
    {
		[Header("Animation")]
		[SerializeField]
		private float transitionDuration = 0.5f;
		[SerializeField]
		private float autoHideDelay = 3f;
		[SerializeField]
		private Ease easeType = Ease.OutQuart;
		[SerializeField]
		private Transform hiddenPoint;
		[SerializeField]
		private Transform shownPoint;

		[Header("Player objects")]
		[SerializeField] 
		private TextMeshProUGUI trackText;
		[SerializeField] 
		private TextMeshProUGUI authorText;

		private bool _isShown = false;
		private Tweener _moveTween;
		private Tween _autoHideTween;

		public void UpdateTrackInfo(string title, string author)
		{
			trackText.text = title;
			authorText.text = author;
			ShowTemporarily();
		}

		private void ShowTemporarily()
		{
			if (_isShown)
			{
				return;
			}

			SetShowState(true);

			_autoHideTween?.Kill();
			_autoHideTween = DOVirtual.DelayedCall(autoHideDelay, () => SetShowState(false));
		}

		public void ToggleShowState()
		{
			SetShowState(!_isShown);
		}

		public void SetShowState(bool show)
		{
			if (_isShown == show)
			{
				return;
			}

			_isShown = show;
			Vector3 targetPosition = _isShown ? shownPoint.position : hiddenPoint.position;

			_moveTween?.Kill();
			_moveTween = transform.DOMove(targetPosition, transitionDuration).SetEase(easeType);
		}
	}
}