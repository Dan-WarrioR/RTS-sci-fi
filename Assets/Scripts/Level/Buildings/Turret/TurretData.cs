using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class TurretData : ScriptableObject
{
	[field: SerializeField] public float Heatlh { get; set; } = 100f;
	[field: SerializeField] public float Shield { get; set; } = 100f;
	[field: SerializeField] public float Damage { get; private set; } = 5f;
}
