using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSpawner : MonoBehaviour
{
    [SerializeField] private Terrain _terrain;
    [SerializeField] private Vector2 _spawnZone;

    [SerializeField] private Transform _unitParent;

	public void SpawnUnitOnZone(GameObject unit)
    {
		if (!unit.GetComponent<Unit>())
			return;

		var unitData = unit.GetComponent<Unit>().UnitData;

		if (ResourcesController.Instance.Army >= ResourcesController.Instance.MaxArmyCapacity || ResourcesController.Instance.Army + unitData.UnitCost >= ResourcesController.Instance.MaxArmyCapacity || ResourcesController.Instance.Resources < unitData.UnitResourceCost)
            return;

		ResourcesController.Instance.Resources -= unitData.UnitResourceCost; //make method

		ResourcesController.Instance.Army += unitData.UnitCost; //make method

		Vector3 middlePosition = new(Random.Range(transform.position.x - _spawnZone.x, transform.position.x + _spawnZone.x), 30f, Random.Range(transform.position.z - _spawnZone.y, transform.position.z + _spawnZone.y)); 

        Vector3 spawnPoint = new(middlePosition.x, _terrain.SampleHeight(middlePosition), middlePosition.z);

        Instantiate(unit, spawnPoint, Quaternion.identity, _unitParent);
    }

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.green;

        Gizmos.DrawWireCube(transform.position, new Vector3(_spawnZone.x, 0, _spawnZone.y));
	}
}
