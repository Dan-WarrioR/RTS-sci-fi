using UnityEngine;

public class PreviewSystem : MonoBehaviour
{
	[SerializeField] private float _previewYOffset = 0.06f;

	[SerializeField] private Grid _grid;

	[SerializeField] private Transform _cellIndicator;
	private GameObject _previewObject;

	[SerializeField] private Material _previewMaterialPrefab;
	private Material _previewMaterialInstance;

	private Renderer _cellIndicatorRenderer;

	private void Start()
	{
		_previewMaterialInstance = new(_previewMaterialPrefab);

		_cellIndicator.gameObject.SetActive(false);

		_cellIndicatorRenderer = _cellIndicator.GetComponentInChildren<Renderer>();
	}

	public void StartShowingPlacementPreview(GameObject prefab, Vector2Int size)
	{
		_previewObject = Instantiate(prefab);

		PreparePreaview(_previewObject);

		PrepareCursor(size);

		_cellIndicator.gameObject.SetActive(true);
	}

	public void StopShowingPreview()
	{
		_cellIndicator.gameObject.SetActive(false);

		Destroy(_previewObject);
	}

	public void UpdatePosition(Vector3 position, bool validity)
	{
		MovePreview(position);

		MoveCursor(position);

		ApplyFeedback(validity);
	}

	private void ApplyFeedback(bool validity)
	{
		Color color = validity ? Color.white : Color.red;	

		color.a = 0.5f;

		_cellIndicatorRenderer.material.color = color;

		_previewMaterialInstance.color = color;
	}

	private void MoveCursor(Vector3 position)
		=> _cellIndicator.transform.position = position;

	private void MovePreview(Vector3 position)
		=> _previewObject.transform.position = new Vector3(position.x, position.y + _previewYOffset, position.z);

	private void PrepareCursor(Vector2Int size)
	{
		if (size.x > 0 || size.y > 0)
		{
			_cellIndicator.localScale = new Vector3(size.x * _grid.cellSize.x, 1, size.y * _grid.cellSize.z);

			_cellIndicatorRenderer.material.mainTextureScale = size;
		}
	}

	private void PreparePreaview(GameObject previewObject)
	{
		Renderer[] renderers = previewObject.GetComponentsInChildren<Renderer>();

		foreach (Renderer renderer in renderers)
		{
			Material[] materials = renderer.materials;

			for (int i = 0; i < materials.Length; i++)
				materials[i] = _previewMaterialInstance;		

			renderer.materials = materials;
		}
	}
}
