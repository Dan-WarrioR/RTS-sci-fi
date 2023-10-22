using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetectionArea : MonoBehaviour
{
	private Unit _myUnit;

	private void Start()
	{
		_myUnit = GetComponentInParent<Unit>();
	}

	private void OnTriggerStay(Collider other)
	{
		if (!other.CompareTag("Enemy"))
			return;     

		_myUnit.OnTargetStay(other.transform);       
	}

	private void OnTriggerEnter(Collider other)
	{
		if (!other.CompareTag("Enemy"))
			return;

		_myUnit.OnTargetEnter(other.transform);
	}

	private void OnTriggerExit(Collider other)
	{
		if (!other.CompareTag("Enemy"))
			return;

		_myUnit.OnTargetExit(other.transform);
	}
}
