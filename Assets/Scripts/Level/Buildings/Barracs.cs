using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barracs : MonoBehaviour
{
	[SerializeField][Range(1f, 10f)] private int _armyIncreaseNumber = 5;

	[field: SerializeField] public float BuildingPrice { get; private set; } = 20f;

	private void Awake()
	{
		ResourcesController.Instance.Army += _armyIncreaseNumber;
	}
}
