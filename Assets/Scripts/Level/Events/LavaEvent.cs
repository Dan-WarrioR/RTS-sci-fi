using DG.Tweening;
using System.Collections;
using UnityEngine;

public class LavaEvent : MonoBehaviour
{
	[Header("KillingSettings")]
	[SerializeField] private float _lavaDamage = 100f;

	[Header("LavaPositionsPerPhase")]

	[SerializeField] private float _startedPositionY;
	[SerializeField] private float _preLaunchPhaseY;
	[SerializeField] private float _disasterPhaseY;

	[Header("DelayPerPhasesInSeconds")]

	[SerializeField] private float _delayFromStartedToPreLaunch = 60f;
	[SerializeField] private float _delayFromPreLaunchToDisaster = 10f;
	[SerializeField] private float _delayFromDisasterToStarted = 20f;

	[Header("PhaseSounds")]

	[SerializeField] private AudioSource _burnSound;
	[SerializeField] private AudioSource _lavaSound;

	public enum Phases
	{
		Started,
		PreLaunch,
		Disaster,
	}

	private Phases _currentPhase = Phases.Started;

	private bool _isTargetInZone = false;

	private void Start()
	{
		_lavaSound.mute = true;

		StartCoroutine(LaunchDisasterPhase(GetNextPhasePosition(_currentPhase), GetDelayToNextPhase(_currentPhase), 5f));
	}

	private IEnumerator LaunchDisasterPhase(float nextYPosition, float delayBeforeStarting, float animationDelay)
	{
		yield return new WaitForSeconds(delayBeforeStarting);

		PlaySoundOnPhase(_currentPhase);

		transform.DOMoveY(nextYPosition, animationDelay);
		yield return new WaitForSeconds(animationDelay);

		_currentPhase++;

		StartCoroutine(LaunchDisasterPhase(GetNextPhasePosition(_currentPhase), GetDelayToNextPhase(_currentPhase), animationDelay));
	}

	private float GetNextPhasePosition(Phases currentPhase)
	{
		switch (currentPhase)
		{
			case Phases.Started:
				return _preLaunchPhaseY;

			case Phases.PreLaunch:
				return _disasterPhaseY;

			case Phases.Disaster:
				return _startedPositionY;

			default:

				_currentPhase = Phases.Started;
				goto case Phases.Started;
		}
	}

	private void PlaySoundOnPhase(Phases phase)
	{
		switch (phase) //sounds to next phase
		{
			case Phases.Started:
			default:

				_burnSound.Play();
				_lavaSound.mute = false;
				break;

			case Phases.PreLaunch:

				_burnSound.Play();
				_lavaSound.mute = false;
				break;

			case Phases.Disaster:
				
				_burnSound.Play();
				_lavaSound.mute = true;
				break;		
		}
	}

	private float GetDelayToNextPhase(Phases phase)
	{
		switch (phase)
		{
			case Phases.Started:
				return _delayFromStartedToPreLaunch;

			case Phases.PreLaunch:
				return _delayFromPreLaunchToDisaster;

			case Phases.Disaster:
				return _delayFromDisasterToStarted;

			default:
				goto case Phases.Started;
		}
	}

	private void OnTriggerStay(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			_isTargetInZone = true;

			TryDamageTarget(other.transform);
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Player"))
			_isTargetInZone = false;
		
	}

	private void TryDamageTarget(Transform target)
	{
		if (!_isTargetInZone)
			return;

		if (target.TryGetComponent(out Unit unit))
		{
			if (unit.IsAlive)
				unit.TakeDamage(_lavaDamage * Time.deltaTime);
		}
	}
}
