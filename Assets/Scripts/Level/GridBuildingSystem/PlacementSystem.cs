using System;
using System.Collections.Generic;
using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
	[Header("GridSystem")]

    [SerializeField] private InputManager _inputManager;

	[SerializeField] private Grid _grid;

	[Header("BuildingSystem")]
	[SerializeField] private BuildingsDatabaseSO _buildingsDatabase;

	private int _selectedObjectIndex = -1;

	[SerializeField] private Transform _gridVisualization;

	[SerializeField] private PreviewSystem _preview;

	[SerializeField] private Terrain _terrain;

	private Vector3Int _lastDetectedPosition = Vector3Int.zero;

	[SerializeField] private AudioSource _placementSound;

	private GridData _buildingData;

	private List<GameObject> _placedGameObjects = new();

	private void Start()
	{
		StopPlacement();

		_buildingData = new();
	}

	public void StartPlacement(int id)
	{
		StopPlacement();

		_selectedObjectIndex = _buildingsDatabase.ObjectData.FindIndex(data => data.ID == id);

		if (_selectedObjectIndex < 0)
		{
			Debug.LogError($"ќб'Їкта з ID не знайдено {id}");
			return;
		}

		_gridVisualization.gameObject.SetActive(true);

		_preview.StartShowingPlacementPreview(_buildingsDatabase.ObjectData[_selectedObjectIndex].Prefab, _buildingsDatabase.ObjectData[_selectedObjectIndex].Size);

		_inputManager.OnClicked += PlaceStructure;

		_inputManager.OnExit += StopPlacement;
	}

	private void PlaceStructure()
	{
		if (_inputManager.IsPointerOverUI())
			return;	

		Vector3 mousePosition = _inputManager.GetSelectedMapPosition();

		Vector3Int gridPosition = _grid.WorldToCell(mousePosition);		

		bool placementValidity = CheckPlacementValidity(gridPosition, _selectedObjectIndex);

		if (!placementValidity)
			return;

		ResourcesController.Instance.Resources -= _buildingsDatabase.ObjectData[_selectedObjectIndex].BuildingPrice;

		_placementSound.Play();

		GameObject newObject = Instantiate(_buildingsDatabase.ObjectData[_selectedObjectIndex].Prefab);

		newObject.transform.position = _grid.CellToWorld(gridPosition);

		_placedGameObjects.Add(newObject);

		//GridData selectedData = _buildingsDatabase.ObjectData[selectedObjectIndex].ID == 0 ? floorData : furnitureData;

		GridData selectedData = _buildingData;

		selectedData.AddObjectAt(gridPosition, _buildingsDatabase.ObjectData[_selectedObjectIndex].Size, _buildingsDatabase.ObjectData[_selectedObjectIndex].ID, _placedGameObjects.Count - 1);

		_preview.UpdatePosition(_grid.CellToWorld(gridPosition), false);
	}

	private bool CheckPlacementValidity(Vector3Int gridPosition, int selectedObjectIndex)
	{
		//GridData selectedData = _buildingsDatabase.ObjectData[selectedObjectIndex].ID == 0 ? floorData : furnitureData;

		GridData selectedData = _buildingData;

		if (_terrain.SampleHeight(_grid.CellToWorld(gridPosition)) <= _gridVisualization.transform.position.y - 1f)
			return false;

		if (ResourcesController.Instance.Resources + _buildingsDatabase.ObjectData[selectedObjectIndex].BuildingPrice > ResourcesController.Instance.MaxResourcesAmount || 
			ResourcesController.Instance.Resources - _buildingsDatabase.ObjectData[selectedObjectIndex].BuildingPrice < 0)
			return false;

		return selectedData.CanPlaceObjectAt(gridPosition, _buildingsDatabase.ObjectData[selectedObjectIndex].Size);
	}

	private void StopPlacement()
	{
		_selectedObjectIndex = -1;

		_gridVisualization.gameObject.SetActive(false);

		_preview.StopShowingPreview();

		_inputManager.OnClicked -= PlaceStructure;

		_inputManager.OnExit -= StopPlacement;

		_lastDetectedPosition = Vector3Int.zero;
	}

	private void Update()
	{
		if (_selectedObjectIndex < 0)
			return;

		Vector3 mousePosition = _inputManager.GetSelectedMapPosition();		

		Vector3Int gridPosition = _grid.WorldToCell(mousePosition);

		if (_lastDetectedPosition != gridPosition)
		{
			bool placementValidity = CheckPlacementValidity(gridPosition, _selectedObjectIndex);

			_preview.UpdatePosition(_grid.CellToWorld(gridPosition), placementValidity);

			_lastDetectedPosition = gridPosition;
		}	
	}
}
