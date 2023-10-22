using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridData 
{
    Dictionary<Vector3Int, PlacementData> placedObjects = new();

	public void AddObjectAt(Vector3Int gridposition, Vector2Int objectSize, int id, int placedObjectIndex)
	{
		List<Vector3Int> positionsToOccupy = CalculatePositions(gridposition, objectSize);

		PlacementData data = new(positionsToOccupy, id, placedObjectIndex);

		foreach (var position in positionsToOccupy)
		{
			if (placedObjects.ContainsKey(position))
				throw new Exception($"Dictionary already contains this cell position {position}");

			placedObjects[position] = data;
		}
	}

	private List<Vector3Int> CalculatePositions(Vector3Int gridposition, Vector2Int objectSize)
	{
		List<Vector3Int> returnValues = new();

		for (int x = 0; x < objectSize.x; x++)
		{
			for (int y = 0; y < objectSize.y; y++)
			{
				returnValues.Add(gridposition + new Vector3Int(x, 0, y));
			}
		}

		return returnValues;
	}

	public bool CanPlaceObjectAt(Vector3Int gridposition, Vector2Int objectSize)
	{
		List<Vector3Int> positionsToOccupy = CalculatePositions(gridposition, objectSize);

		foreach (var position in positionsToOccupy)
		{
			if (placedObjects.ContainsKey(position))
				return false;
		}

		return true;
	}
}

public class PlacementData
{
    public List<Vector3Int> occupiedPositions;
	public int ID { get; private set; }
	public int PlacedObjectIndex { get; private set; }

	public PlacementData(List<Vector3Int> occupiedPositions, int iD, int placedObjectIndex)
	{
		this.occupiedPositions = occupiedPositions;
		ID = iD;
		PlacedObjectIndex = placedObjectIndex;
	}	
}
