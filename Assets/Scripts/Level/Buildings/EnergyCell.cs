using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnergyCell : MonoBehaviour
{
	[SerializeField][Range(1f, 10f)] private int _resourcesIncomSpeed = 1;
	[SerializeField] private float _delayBeforeNextMine = 2f;

	[field: SerializeField] public float BuildingPrice { get; private set; } = 30f;
	private void Awake()
	{
		StartCoroutine(MineResources(_delayBeforeNextMine));
	}

	private IEnumerator MineResources(float delay)
	{
		yield return new WaitForSeconds(delay);

		ResourcesController.Instance.Resources += _resourcesIncomSpeed;

		StartCoroutine(MineResources(delay));
	}
}
