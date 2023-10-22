using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class UnitData : ScriptableObject
{
	[field: SerializeField] public string Name { get; private set; }

	[field: SerializeField] public float Health { get; set; } = 100f; //wrong

	[field: SerializeField] public float Shield { get; set; } = 100f; //wrong

	[field: SerializeField] public float Damage { get; private set; } = 10f;

	[field: SerializeField] public int UnitResourceCost { get; private set; } = 1;

	[field: SerializeField] public int UnitCost { get; private set; } = 1;
}
