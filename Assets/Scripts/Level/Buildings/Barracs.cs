using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class Barracs : MonoBehaviour
{
	[SerializeField] private int _armyIncreaseNumber = 10;

	[field: SerializeField] public BuildingData BuildingData { get; private set; }

	[field: SerializeField] public EntityInformationWindow InformationWindow { get; set; }
	[field: SerializeField] public GameObject InformationWindowObject { get; set; }

	public void IncreaseArmyCapacity()
	{
		ResourcesController.Instance.ArmyCapacity += _armyIncreaseNumber;
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
}
