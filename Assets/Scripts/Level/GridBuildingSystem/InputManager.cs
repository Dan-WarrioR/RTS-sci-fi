using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
    [SerializeField] private Camera _camera;

    [SerializeField] private LayerMask _placementLayerMask;

    private Vector3 _lastPosition;

    public event Action OnClicked, OnExit;

    [Header("SelectionUnits")]

	[SerializeField] private RectTransform _selectionBox;

	[SerializeField] private LayerMask _unitLayers;
	[SerializeField] private LayerMask _groundLayers;

    [SerializeField] private float _dragDelay = 0.1f;
    [SerializeField] private float[] _unitPointRings = { 3f, 6f, 9f };
    [SerializeField] private int[] _unitPointRingCount = { 5, 10, 20 };

    private bool _unitCanShoot = false;

    private float _mouseDownTime;
    private Vector2 _startedMousePosition;

	private void Update()
	{
        HandleInputForBuildSystem();
         
        HandleSelectionInputs();
        HandleMovementInputs();
	}

	#region SelectionBoxAndUnitMovement

	private void HandleMovementInputs()
	{
        if (Input.GetKeyUp(KeyCode.Mouse1) && SelectionManager.Instance.SelectedUnits.Count > 0)
        {
            if (Physics.Raycast(_camera.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, _groundLayers))
            {

                Vector3 moveUnitToPosition = hit.point;

                List<Vector3> targetPositionList = GetPositionListAround(moveUnitToPosition, _unitPointRings, _unitPointRingCount);

                int targetPositionListIndex = 0;

                if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) //queue
                {
                    foreach (Unit unit in SelectionManager.Instance.SelectedUnits)
                    {
                        unit.AddNewPointForUnit(targetPositionList[targetPositionListIndex]);

                        targetPositionListIndex = (targetPositionListIndex + 1) % targetPositionList.Count;
                    }
                }
                else
                {
                    foreach (Unit unit in SelectionManager.Instance.SelectedUnits)
                    {
                        unit.MoveTo(targetPositionList[targetPositionListIndex]);

                        targetPositionListIndex = (targetPositionListIndex + 1) % targetPositionList.Count;
                    }
                }
            }
            
		}
		else if (Input.GetKeyUp(KeyCode.E) && SelectionManager.Instance.SelectedUnits.Count > 0)
		{
			foreach (Unit unit in SelectionManager.Instance.SelectedUnits)
            {
				unit.CanShoot = !unit.CanShoot;
                unit.OnUnitShowIndicator();
			}
		}

	}

    #region DynamicallyUnitPositionCalculate

    private List<Vector3> GetPositionListAround(Vector3 startPosition, float[] ringDistanceArray, int[] ringPositionCountArray)
    {
		List<Vector3> positionList = new();

        positionList.Add(startPosition);

		for (int i = 0; i < ringDistanceArray.Length; i++)
		{
            positionList.AddRange(GetPositionListAround(startPosition, ringDistanceArray[i], ringPositionCountArray[i]));
		}

		return positionList;
	}

    private List<Vector3> GetPositionListAround(Vector3 startPosition, float distance, int positionCount)
    {
        List<Vector3> positionList = new();

        for (int i = 0; i < positionCount; i++)
        {
            float angle = i * (360f / positionCount);

            Vector3 direction = ApplyRotationToVector(new Vector3(1, 0, 0), angle);

            Vector3 position = startPosition + direction * distance;

            positionList.Add(position);
		}

        return positionList;
    }

    private Vector3 ApplyRotationToVector(Vector3 vector, float angle)
    {
        return Quaternion.Euler(0, angle, 0) * vector;
    }

	#endregion

	private void HandleSelectionInputs()
	{
		if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            _selectionBox.sizeDelta = Vector3.zero;

			_selectionBox.gameObject.SetActive(true);

            _startedMousePosition = Input.mousePosition;

            _mouseDownTime = Time.time;
		}
        else if (Input.GetKey(KeyCode.Mouse0) && _mouseDownTime + _dragDelay < Time.time)
        {
            ResizeSelectionBox();
        }
        else if (Input.GetKeyUp(KeyCode.Mouse0))
        {
			_selectionBox.sizeDelta = Vector3.zero;

			_selectionBox.gameObject.SetActive(false);

            if (Physics.Raycast(_camera.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, _unitLayers) && hit.collider.TryGetComponent<Unit>(out Unit unit))
            {
                if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                {
                    if (SelectionManager.Instance.IsSelected(unit))
                    {
                        SelectionManager.Instance.Deselect(unit);
					}
                    else
                    {
						SelectionManager.Instance.Select(unit);
					}
				}
                else
                {
                    SelectionManager.Instance.DeselectAll();

					SelectionManager.Instance.Select(unit);
				}
            }
            else if (_mouseDownTime + _dragDelay > Time.time)
            {
				SelectionManager.Instance.DeselectAll();
			}

            _mouseDownTime = 0f;
		}
	}

	private void ResizeSelectionBox()
	{
		float width = Input.mousePosition.x - _startedMousePosition.x;

        float height = Input.mousePosition.y - _startedMousePosition.y;

        _selectionBox.anchoredPosition = _startedMousePosition + new Vector2(width / 2f, height / 2f);

        _selectionBox.sizeDelta = new Vector2(Mathf.Abs(width), Mathf.Abs(height));

        Bounds bounds = new Bounds(_selectionBox.anchoredPosition, _selectionBox.sizeDelta);

        for (int i = 0; i < SelectionManager.Instance.AvailableUnits.Count; i++)
        {
			if (UnitIsInSelectionBox(_camera.WorldToScreenPoint(SelectionManager.Instance.AvailableUnits[i].transform.position), bounds))
            {
                SelectionManager.Instance.Select(SelectionManager.Instance.AvailableUnits[i]);
            }
            else
            {
                SelectionManager.Instance.Deselect(SelectionManager.Instance.AvailableUnits[i]);
            }
        }
	}

	private bool UnitIsInSelectionBox(Vector2 position, Bounds bounds)	
        => position.x > bounds.min.x && position.x < bounds.max.x && position.y > bounds.min.y && position.y < bounds.max.y;	

	#endregion

	#region BuildingSystem

	public bool IsPointerOverUI()
        => EventSystem.current.IsPointerOverGameObject();

	public Vector3 GetSelectedMapPosition()
    {
        Vector3 mousePosition = Input.mousePosition;

        mousePosition.z = _camera.nearClipPlane;

        Ray ray = _camera.ScreenPointToRay(mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 100f, _placementLayerMask))
            _lastPosition = hit.point;

        return _lastPosition;
    }

    private void HandleInputForBuildSystem()
    {
		if (Input.GetMouseButtonDown(0))
			OnClicked?.Invoke();

		if (Input.GetKeyDown(KeyCode.Escape))
			OnExit?.Invoke();
	}

    #endregion
}
