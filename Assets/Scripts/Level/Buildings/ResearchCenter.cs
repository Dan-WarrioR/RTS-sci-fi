using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResearchCenter : MonoBehaviour
{
	[SerializeField] private GameObject[] _turrets;

	[SerializeField][Range(1, 10)] private float _loadingSpeedSlowdown = 1f;

	[SerializeField] private Slider ScanningBar;

	[SerializeField] private AudioSource _activationSound;

	private SphereCollider _sphereCollider;

	public float LoadingProgressAmount { get; private set; } = MinLoadingProgressNumber; 

	private const int NumberToStartAnimation = 20;

	private const int MaxLoadingProgressNumber = 20; 
	private const int MinLoadingProgressNumber = 0;

	private bool _isPlayerUnitInArea = false;
	private bool _isCharged = false;

	private Animator _animator;

	void Start()
	{
		_animator = GetComponent<Animator>();
	}

	void Update()
	{
		if (LoadingProgressAmount >= NumberToStartAnimation && !_isCharged)
			FinishCapturing();


		StartCapturingProgress();	
	}

	public void OnPlayerStay(Transform player)
	{

	}

	public void OnPlayerEnter(Transform player)
	{
		_isPlayerUnitInArea = true;
	}

	public void OnPlayerExit(Transform player)
	{
		_isPlayerUnitInArea = false;
	}

	private void StartCapturingProgress()
	{
		if (!_isPlayerUnitInArea)
			return;

		float newValue = Time.deltaTime / _loadingSpeedSlowdown;

		if (!_isCharged)
		{
			LoadingProgressAmount += Mathf.Min(newValue, MaxLoadingProgressNumber);

			ScanningBar.value += Mathf.Min(newValue, MaxLoadingProgressNumber);
		}	
	}

	private float GetBarNormalized()
	{
		return LoadingProgressAmount / MaxLoadingProgressNumber;
	}

	private void FinishCapturing()
	{
		_isCharged = true;

		_animator.SetTrigger("StartScan");

		_activationSound.Play();

		foreach (var turret in _turrets)
		{
			turret.TryGetComponent<TurretLogic>(out TurretLogic turretScript);

			turretScript.PlayDeath();
		}
	}

	private void OnValidate()
	{
		_sphereCollider = GetComponentInChildren<SphereCollider>();
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;

		Gizmos.DrawWireSphere(transform.position, _sphereCollider.radius / 10f);
	}
}
