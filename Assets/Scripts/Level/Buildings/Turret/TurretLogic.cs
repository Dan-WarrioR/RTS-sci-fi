using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretLogic : MonoBehaviour
{
	[Header("TurretData")]
	[SerializeField] public TurretData turretData;

	public float Health
	{
		get
		{
			return _health;
		}
	}
	private float _health;

	public float Shield
	{
		get
		{
			return _shield;
		}
	}
	private float _shield;

	[Header("TurretSettings")]

	[SerializeField] private float _firingRange = 5f;
	[SerializeField] private float _turretRotationSpeed = 0.2f;
	[SerializeField] private float _patrolTurningSpeed = 10f;
	[SerializeField] private string _targetTag;

	[SerializeField][Range(0.1f, 1f)] private float _coefficientBetweenTurretDuration = 0.2f;

	[SerializeField][Range(_minPatrolRange, 100)] private float _patrolRange;

	[Header("TurretParts")]

	[SerializeField] private Transform _turretRotationBase;
	[SerializeField] private Transform _turretHead;
	[SerializeField] private Transform _turretBarrel;
	[SerializeField] private SphereCollider _turretDetectionArea;

	[SerializeField] private AnimationClip _deactivationCLip;

	[Space(10)]

	[SerializeField] private GameObject _shootingEffect;

	private List<Transform> _availableTargets = new();
	private Transform _currentTarget;

	private Animation _animations;

	private Vector3 _startedTurretPosition;

	private bool _isPatrolling = true;
	private bool _canFire = false;
	private bool _isAlredyDied = false;

	private const float _minPatrolRange = 5;

	[SerializeField] public bool IsAlive { get; private set; } = true;

	private void Update()
	{
		//Debug.Log($"{Health} - health; {Shield} - shield");

		if (_health <= 0 && IsAlive)
			PlayDeath();
	}

	private void Start()
	{
		_health = turretData.Heatlh;

		_shield = turretData.Shield;

		_turretDetectionArea.radius = _firingRange;

		_startedTurretPosition = transform.position;

		_shootingEffect.SetActive(false);

		_animations = GetComponent<Animation>();
		_animations.clip = _deactivationCLip;

		StartCoroutine(StartPatrol());
	}

	public void OnTargetEnter(Transform transform)
	{
		if (!IsAlive)
			return;

		if (!transform.CompareTag(_targetTag))
			return;

		if (_availableTargets.Count <= 0)
			_currentTarget = transform;

		_availableTargets.Add(transform);

		_canFire = true;
		_isPatrolling = false;
	}

	public void OnTargetStay(Transform transform)
	{
		if (!IsAlive)
			return;

		if (!transform.CompareTag(_targetTag))
			return;

		AimToTarget();
	}

	public void OnTargetExit(Transform transform)
	{
		if (!IsAlive)
			return;

		if (!transform.CompareTag(_targetTag))
			return;

		if (_availableTargets.Contains(transform))
			_availableTargets.Remove(transform);

		if (_availableTargets.Count <= 0)
		{
			EnablePatrol();
		}
		else
		{
			AimToTarget();
		}
	}

	private void EnablePatrol()
	{
		_canFire = false;
		_isPatrolling = true;

		_shootingEffect.SetActive(false);

		StartCoroutine(StartPatrol());
	}

	private void AimToTarget()
	{
		if (!_canFire)
			return;

		if (!_currentTarget)
			return;

		transform.DOKill();

		_turretRotationBase.DOLookAt(new Vector3(_currentTarget.position.x, transform.position.y, _currentTarget.position.z), _turretRotationSpeed, AxisConstraint.Y);

		//_turretHead.DOLookAt(new Vector3(_target.position.x, _turretHead.position.y, _turretHead.position.z), _turretRotationSpeed * Time.deltaTime);

		if (Physics.Raycast(_turretBarrel.position, _turretBarrel.forward, out RaycastHit hit)) //Check if turret alredy looks on target, if yes we can shoot
		{
			Debug.DrawLine(_turretBarrel.position, hit.point, Color.red);		

			if (hit.transform.CompareTag(_targetTag))
				Shoot();
		}
	}

	private void Shoot()
	{
		if (!_canFire)
		{
			if (_shootingEffect.activeInHierarchy)
			    _shootingEffect.SetActive(false);

			StopAllCoroutines(); //наче не надо

			return;
		}

		//_turretBarrel.Rotate(0, 0, _barrelRotationSpeed * Time.deltaTime);

		//if (!_shootingEffect.isPlaying)
		//	_shootingEffect.Play();

		//StartCoroutine(TryShoot(2f));

		if (!_shootingEffect.activeInHierarchy)
			_shootingEffect.SetActive(true);

		if (_canFire && _currentTarget.TryGetComponent(out Unit unit))
		{
			if (!unit.IsAlive)
			{
				if (_availableTargets.Contains(unit.transform))
					_availableTargets.Remove(unit.transform);

				if (_availableTargets.Count > 0)
				{
					_currentTarget = _availableTargets[_availableTargets.Count - 1];
				}
				else
				{
					EnablePatrol();
				}
			}

			unit.TakeDamage(turretData.Damage * Time.deltaTime);
		}
	}

	public void TakeDamage(float damage)
	{
		if (damage <= 0)
			return;

		if (_shield > 0 && damage <= _shield)
		{
			_shield -= damage;
		}
		else if (_shield > 0 && damage > _shield)
		{
			damage -= _shield;

			_shield = 0;

			_health -= damage;
		}
		else
		{
			_health -= damage;
		}
	}

	private IEnumerator StartPatrol()
	{
		while (_isPatrolling)
		{
			//Vector3 nextPoint = CreateNextPointToPatrol();

			//float lengthbetweenPoints = (transform.position - nextPoint).magnitude;

			//float duration = lengthbetweenPoints * _coefficientBetweenTurretDuration + 0.5f;

			//transform.DOMove(nextPoint, duration);

			_turretRotationBase.DORotate(new Vector3(0, UnityEngine.Random.Range(0, 360), 0), _patrolTurningSpeed);

			yield return new WaitForSeconds(_patrolTurningSpeed);
		}
	}

	public void PlayDeath()
	{
		if (_isAlredyDied)
			return;

		_isAlredyDied = true;
		IsAlive = false;

		if (_currentTarget)
		{
			if (_currentTarget.TryGetComponent(out Unit unit))
				unit.EnemyWasDied(this);
		}	

		if (_shootingEffect.activeInHierarchy)
			_shootingEffect.SetActive(false);

		gameObject.tag = "Untagged";

		_animations.Play();
	}

	private Vector3 CreateNextPointToPatrol()
		=> new Vector3(_startedTurretPosition.x + UnityEngine.Random.Range(-_patrolRange, _patrolRange), transform.position.y, _startedTurretPosition.z + UnityEngine.Random.Range(-_patrolRange, _patrolRange));
}
