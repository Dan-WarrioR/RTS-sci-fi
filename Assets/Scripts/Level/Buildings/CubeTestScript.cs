using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeTestScript : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
			Debug.Log("Player Enter");
	}

	private void OnTriggerStay(Collider other)
	{
		if (other.CompareTag("Player"))
			Debug.Log("Player Stay");
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Player"))
			Debug.Log("Player Exit");
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;

		BoxCollider boxCollider= GetComponent<BoxCollider>();

		Gizmos.DrawWireCube(transform.position, boxCollider.size);
	}
}
