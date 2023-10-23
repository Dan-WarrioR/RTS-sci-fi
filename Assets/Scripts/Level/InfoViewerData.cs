using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InfoViewerData
{
	private static ResourcesController _instance;

	public static ResourcesController Instance
	{
		get
		{
			if (_instance == null)
				_instance = new ResourcesController();

			return _instance;
		}

		private set
		{
			_instance = value;
		}
	}

	[field: SerializeField] public Image EntityImage { get; set; }

    [field: SerializeField] public TextMeshProUGUI EntityName { get; set; }

    [field: SerializeField] public Slider HealthBar { get; set; }
    [field: SerializeField] public Slider ShieldBar { get; set; }
    [field: SerializeField] public Slider DamageBar { get; set; }

    [field: SerializeField] public Slider PriceBar { get; set; }
    [field: SerializeField] public Slider BarracUnitSize { get; set; }
}
