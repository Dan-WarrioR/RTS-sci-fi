using UnityEngine;

public class BuildingButton : MonoBehaviour
{
	[SerializeField] private BuildingData BuildingData;

	[SerializeField] private EntityInformationWindow InformationWindow;
	[SerializeField] private GameObject InformationWindowObject;

	public void ShowInfo()
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

	public void HideInfo()
	{
		if (!InformationWindowObject)
			return;

		InformationWindowObject.gameObject.SetActive(false);

		InformationWindow.DamageBar.gameObject.SetActive(true);
		InformationWindow.BarracUnitSize.gameObject.SetActive(true);
	}
}
