using UnityEngine;

[CreateAssetMenu]
public class BuildingData : ScriptableObject, IEntity
{
	[field: SerializeField] public string Name { get; private set; }

	[field: SerializeField] public Sprite EntityImage { get; private set; }

	[field: SerializeField] public float Health { get; set; } = 100f; //wrong

	[field: SerializeField] public float Shield { get; set; } = 100f; //wrong

	[field: SerializeField] public int BuildingResourceCost { get; set; } = 1; //VERY WRONG
}
