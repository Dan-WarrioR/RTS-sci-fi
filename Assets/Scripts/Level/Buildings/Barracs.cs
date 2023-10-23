using UnityEngine;

public class Barracs : MonoBehaviour
{
	[SerializeField] private int _armyIncreaseNumber = 10;

	[field: SerializeField] public float BuildingPrice { get; private set; } = 20f;

	public void IncreaseArmyCapacity()
	{
		ResourcesController.Instance.ArmyCapacity += _armyIncreaseNumber;
	}
}
