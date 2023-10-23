using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSpawner : MonoBehaviour
{
    [SerializeField] private Terrain _terrain;
    [SerializeField] private Vector2 _spawnZone;

    [SerializeField] private Transform _unitParent;

	[SerializeField] private EntityInformationWindow InformationWindow;
	[SerializeField] private GameObject InformationWindowObject;

	public void SpawnUnitOnZone(GameObject unit)
    {
		if (!unit.GetComponent<Unit>())
			return;

		var unitData = unit.GetComponent<Unit>().UnitData;

		if (ResourcesController.Instance.ArmyCapacity > ResourcesController.Instance.ArmyCapacity || ResourcesController.Instance.ArmyUnitCount + unitData.UnitCost > ResourcesController.Instance.ArmyCapacity || ResourcesController.Instance.Resources < unitData.UnitResourceCost)
            return;

		ResourcesController.Instance.Resources -= unitData.UnitResourceCost; //make method

		ResourcesController.Instance.ArmyUnitCount += unitData.UnitCost; //make method

		Vector3 middlePosition = new(Random.Range(transform.position.x - _spawnZone.x, transform.position.x + _spawnZone.x), 30f, Random.Range(transform.position.z - _spawnZone.y, transform.position.z + _spawnZone.y)); 

        Vector3 spawnPoint = new(middlePosition.x, _terrain.SampleHeight(middlePosition), middlePosition.z);

        GameObject unitObject = Instantiate(unit, spawnPoint, Quaternion.identity, _unitParent);

		if (unitObject.TryGetComponent(out Unit unitScript))
		{
			unitScript.InformationWindow = InformationWindow;

			unitScript.InformationWindowObject = InformationWindowObject;
		}
    }

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.green;

        Gizmos.DrawWireCube(transform.position, new Vector3(_spawnZone.x, 0, _spawnZone.y));
	}
}
