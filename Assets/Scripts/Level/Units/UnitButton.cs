using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class UnitButton : MonoBehaviour
{
	[SerializeField] private UnitData _unitData;

	[SerializeField] private EntityInformationWindow InformationWindow;
	[SerializeField] private GameObject InformationWindowObject;

	public void ShowInfo()
	{
		if (!InformationWindowObject)
			return;

		InformationWindowObject.gameObject.SetActive(true);

		InformationWindow.EntityImage.sprite = _unitData.EntityImage;

		InformationWindow.EntityName.text = _unitData.Name;

		InformationWindow.HealthBar.value = _unitData.Health;
		InformationWindow.HealthBarText.text = _unitData.Health.ToString();

		InformationWindow.ShieldBar.value = _unitData.Shield;
		InformationWindow.ShieldBarText.text = _unitData.Shield.ToString();

		InformationWindow.DamageBar.value = _unitData.Damage;
		InformationWindow.DamageBarText.text = _unitData.Damage.ToString();

		InformationWindow.PriceBar.value = _unitData.UnitResourceCost;
		InformationWindow.PriceBarText.text = _unitData.UnitResourceCost.ToString();

		InformationWindow.BarracUnitSize.value = _unitData.UnitCost;
		InformationWindow.BarracBarText.text = _unitData.UnitCost.ToString();
	}

	public void HideInfo()
	{
		if (!InformationWindowObject)
			return;

		InformationWindowObject.gameObject.SetActive(false);
	}
}
