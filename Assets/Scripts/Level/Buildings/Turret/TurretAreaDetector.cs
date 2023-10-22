using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretAreaDetector : MonoBehaviour
{
	private TurretLogic _myTurret;


	private void Start()
	{
		_myTurret = GetComponentInParent<TurretLogic>();
	}

	private void OnTriggerStay(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			_myTurret.OnTargetStay(other.transform);
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			_myTurret.OnTargetEnter(other.transform);
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			_myTurret.OnTargetExit(other.transform);
		}
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;

		Gizmos.DrawWireSphere(transform.position, GetComponent<SphereCollider>().radius);
	}
}
