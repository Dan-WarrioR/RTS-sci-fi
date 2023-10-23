using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EntityInformationWindow : MonoBehaviour
{
	[field: SerializeField] public Image EntityImage { get; set; }

	[field: SerializeField] public TextMeshProUGUI EntityName { get; set; }

	[field: SerializeField] public Slider HealthBar { get; set; }
	[field: SerializeField] public TextMeshProUGUI HealthBarText { get; set; }
	
	[field: SerializeField] public Slider ShieldBar { get; set; }
	[field: SerializeField] public TextMeshProUGUI ShieldBarText { get; set; }

	[field: SerializeField] public Slider DamageBar { get; set; }
	[field: SerializeField] public TextMeshProUGUI DamageBarText { get; set; }

	[field: SerializeField] public Slider PriceBar { get; set; }
	[field: SerializeField] public TextMeshProUGUI PriceBarText { get; set; }

	[field: SerializeField] public Slider BarracUnitSize { get; set; }
	[field: SerializeField] public TextMeshProUGUI BarracBarText { get; set; }


	private void Start()
	{
		gameObject.SetActive(false);
	}
}
