using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Unit : MonoBehaviour
{
	[field: SerializeField] public UnitData UnitData { get; private set; }

	[SerializeField] private Transform _shootingPlace;

	[Header("Visual")]

	[SerializeField] private Transform _selectionSprite;
	[SerializeField] private Transform _canShootIndicator;
	[SerializeField] private AudioSource _shootingSound;

	[SerializeField] private float _timeBeforeDeath = 2f;

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

	public Action OnUnitFinishedWalking;
	public Action OnUnitCanStopShooting;

	private NavMeshAgent _agent;

	private Vector3 _currentPointToMove = Vector3.zero;
	private Queue<Vector3> _availablePointsToMove = new();

	private List<Transform> _availableTargets = new();
	private Transform _currentTarget;

	private Animator _animator;

	public bool CanShoot { get; set; } = false;
	public bool IsAlive { get; private set; } = true;

	private void Start()
	{
		_health = UnitData.Health;
		_shield = UnitData.Shield;

		_canShootIndicator.gameObject.SetActive(CanShoot);
		_selectionSprite.gameObject.SetActive(false);

		_animator = GetComponent<Animator>();

		_agent = GetComponent<NavMeshAgent>();

		SelectionManager.Instance.AvailableUnits.Add(this);

		OnUnitFinishedWalking += StopRunningAnimation;

		OnUnitCanStopShooting += StopShooting;

		_currentPointToMove = transform.position;
	}

	private void Update()
	{
		if (!IsAlive)
			return;

		//Debug.Log(_availableTargets.Count);

		MoveTo();

		if (CheckIfUnitGetToPoint(_currentPointToMove))
			OnUnitFinishedWalking.Invoke();

		if (_health <= 0)
		{
			PlayDeath();
		}

		if (!CanShoot)
		{
			if (_shootingSound.isPlaying)
				_shootingSound.Stop();
		}

		//Debug.Log($"{_availablePointsToMove.Count} - точок маршруту");
	}

	#region Death
	private void PlayDeath()
	{
		IsAlive = false;

		ResourcesController.Instance.Army -= UnitData.UnitCost;

		StopRunningAnimation();
		StopShooting();
		_animator.SetTrigger("Die");
		_agent.ResetPath();
		_agent.isStopped = true;

		_canShootIndicator.gameObject.SetActive(false);

		SelectionManager.Instance.Deselect(this);

		SelectionManager.Instance.AvailableUnits.Remove(this);

		StartCoroutine(PlayDeathAnimation(_timeBeforeDeath));
	}

	private IEnumerator PlayDeathAnimation(float delay)
	{
		yield return new WaitForSeconds(delay);

		//transform.DOScale(Vector3.zero, _timeBeforeDeath);
		transform.DOMove(new Vector3(transform.position.x, transform.position.y - 10f, transform.position.z), delay);

		Destroy(transform.gameObject, delay);
	}

	#endregion

	#region UnitMovement
	private void StopRunningAnimation() //вынести в отдельный скрипт
	{
		_animator.SetBool("IsRun", false);
	}

	public void MoveTo(Vector3 position)
	{
		if (!IsAlive)
			return;

		OnUnitCanStopShooting.Invoke();

		_availablePointsToMove.Clear();

		_currentPointToMove = position;

		if (_agent.SetDestination(_currentPointToMove))
			_animator.SetBool("IsRun", true);
	}

	private void MoveTo()
	{
		//if (_shootingSound.isPlaying)
		//	_shootingSound.Stop();

		if (!IsAlive)
			return;

		if (_availablePointsToMove.Count <= 0 || _availablePointsToMove.Peek() == _currentPointToMove || !CheckIfUnitGetToPoint(_currentPointToMove))
			return;

		//OnUnitCanStopShooting.Invoke();

		Vector3 nextPoint = _availablePointsToMove.Dequeue();

		_currentPointToMove = nextPoint;

		if (_agent.SetDestination(nextPoint))
			_animator.SetBool("IsRun", true);
	}

	private bool CheckIfUnitGetToPoint(Vector3 point)
	{
		if (Approximately(transform.position.x, point.x) && Approximately(transform.position.z, point.z))
			return true;

		return false;
	}

	private bool Approximately(float a, float b, float delta = 1.5f)
		=> MathF.Abs(a - b) < delta;

	public void AddNewPointForUnit(Vector3 pointToMove)
	{
		if (!IsAlive)
			return;

		_availablePointsToMove.Enqueue(pointToMove);

		MoveTo();
	}

	#endregion

	#region UnitSelection

	public void OnSelected()
	{
		if (!IsAlive)
			return;

		_selectionSprite.gameObject.SetActive(true);
	}

	public void OnDeselected()
	{
		_selectionSprite.gameObject.SetActive(false);
	}

	#endregion

	#region Shooting

	private void StopShooting()
	{
		_agent.isStopped = false;

		_animator.SetBool("Shoot", false);

		CanShoot = false;

		_canShootIndicator.gameObject.SetActive(false);
	}

	private void AimToTarget()
	{
		if (!CanShoot || !IsAlive)
		{
			OnUnitCanStopShooting.Invoke();

			//_canShootIndicator.gameObject.SetActive(false);

			return;
		}

		//_canShootIndicator.gameObject.SetActive(true);

		OnUnitFinishedWalking.Invoke();

		//_agent.ResetPath();
		_agent.isStopped = true;

		_animator.SetBool("Shoot", true);

		if (_currentTarget)
			transform.LookAt(new Vector3(_currentTarget.position.x, transform.position.y, _currentTarget.position.z));

		if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit)) //Check if we alredy look on target, if yes we can shoot
		{
			Debug.DrawLine(transform.position, hit.point, Color.red);

			if (hit.transform.CompareTag("Enemy"))
				Shoot();
		}
	}

	private void Shoot()
	{
		if (!CanShoot)
			return;

		if (!_shootingSound.isPlaying)
			_shootingSound.Play();

		if (CanShoot && _currentTarget.TryGetComponent(out TurretLogic turret))
		{
			if (!turret.IsAlive)
			{
				if (_availableTargets.Contains(turret.transform))
					_availableTargets.Remove(turret.transform);

				if (_availableTargets.Count > 0)
				{
					_currentTarget = _availableTargets[_availableTargets.Count - 1];
					Shoot();
				}
				else
				{
					StopShooting();
					MoveTo();
				}
			}

			turret.TakeDamage(UnitData.Damage * Time.deltaTime);
		}
	}

	public void EnemyWasDied(TurretLogic turret)
	{
		if (_availableTargets.Contains(turret.transform))
			_availableTargets.Remove(turret.transform);

		if (_availableTargets.Count > 0)
		{
			_currentTarget = _availableTargets[_availableTargets.Count - 1];
			Shoot();
		}
		else
		{
			_agent.isStopped = false;

			_animator.SetBool("Shoot", false);

			_animator.SetBool("IsRun", true);

			if (_shootingSound.isPlaying)
				_shootingSound.Stop();

			MoveTo();
		}
	}

	public void OnTargetEnter(Transform target)
	{
		if (_availableTargets.Count <= 0)
			_currentTarget = target.transform;

		_availableTargets.Add(target.transform);
	}

	public void OnTargetExit(Transform target)
	{
		if (_availableTargets.Contains(target.transform))
			_availableTargets.Remove(target.transform);

		if (!CanShoot)
			return;

		if (_availableTargets.Count <= 0)
		{
			//OnUnitCanStopShooting.Invoke();

			_animator.SetBool("IsRun", true);
			_animator.SetBool("Shoot", false);
			_agent.isStopped = false;

			MoveTo();
		}
		else
		{
			AimToTarget();
		}
	}

	public void OnTargetStay(Transform target)
	{
		if (!CanShoot)
			return;

		AimToTarget();
	}

	public void OnUnitShowIndicator()
	{
		_canShootIndicator.gameObject.SetActive(CanShoot);
	}

	#endregion

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

	private void OnDestroy()
	{
		OnUnitFinishedWalking -= StopRunningAnimation;

		OnUnitCanStopShooting -= StopShooting;
	}
}
