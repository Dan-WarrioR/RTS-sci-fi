using System.Collections;
using UnityEngine;

public class EnergyCell : MonoBehaviour
{
	[SerializeField][Range(1f, 10f)] private int _resourcesIncomSpeed = 1;
	[SerializeField] private float _delayBeforeNextMine = 2f;

	[field: SerializeField] public BuildingData BuildingData { get; private set; }

	public int BuildingResourceCost { get; set; }

	[field: SerializeField] public EntityInformationWindow InformationWindow { get; set; }
	[field: SerializeField] public GameObject InformationWindowObject { get; set; }
	private void Awake()
	{
		StartCoroutine(MineResources(_delayBeforeNextMine));
	}

	private void OnMouseOver()
	{
		if (!InformationWindowObject)
			return;

		InformationWindowObject.gameObject.SetActive(true);

		InformationWindow.DamageBar.gameObject.SetActive(false);
		InformationWindow.BarracUnitSize.gameObject.SetActive(false);

		InformationWindow.EntityImage.sprite = BuildingData.EntityImage;

		InformationWindow.EntityName.text = BuildingData.Name;

		InformationWindow.HealthBar.value = BuildingData.Health;
		InformationWindow.HealthBarText.text = BuildingData.Health.ToString();

		InformationWindow.ShieldBar.value = BuildingData.Shield;
		InformationWindow.ShieldBarText.text = BuildingData.Shield.ToString();

		InformationWindow.PriceBar.value = BuildingData.BuildingResourceCost;
		InformationWindow.PriceBarText.text = BuildingData.BuildingResourceCost.ToString();
	}

	private void OnMouseExit()
	{
		if (!InformationWindowObject)
			return;

		InformationWindowObject.gameObject.SetActive(false);

		InformationWindow.DamageBar.gameObject.SetActive(true);
		InformationWindow.BarracUnitSize.gameObject.SetActive(true);
	}

	private IEnumerator MineResources(float delay)
	{
		yield return new WaitForSeconds(delay);

		ResourcesController.Instance.Resources += _resourcesIncomSpeed;

		StartCoroutine(MineResources(delay));
	}


}
