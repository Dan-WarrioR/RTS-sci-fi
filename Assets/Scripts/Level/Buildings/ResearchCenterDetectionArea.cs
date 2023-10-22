using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResearcCenterDetectionArea : MonoBehaviour
{
	private ResearchCenter _myResearchCenter;

	private void Start()
	{
		_myResearchCenter = GetComponentInParent<ResearchCenter>();
	}

	private void OnTriggerStay(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			_myResearchCenter.OnPlayerStay(other.transform);
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			_myResearchCenter.OnPlayerEnter(other.transform);
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			_myResearchCenter.OnPlayerExit(other.transform);
		}
	}
}
