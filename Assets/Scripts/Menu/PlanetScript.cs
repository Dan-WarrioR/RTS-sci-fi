using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlanetScript : MonoBehaviour
{
    [SerializeField] private PlanetData planetData;
	[SerializeField] private GameObject _planetUI;

	[Header("Planet description")]

	[SerializeField] private TextMeshProUGUI _planetName;
	[SerializeField] private Image _planetImage;
	[SerializeField] private TextMeshProUGUI _planetDescription;

	private void Start()
	{
		_planetUI.SetActive(false);
	}

	private void OnMouseDown()
	{
		_planetUI.SetActive(true);

		_planetName.text = planetData.PlanetName;

		_planetImage.sprite = planetData.PlanetTerrainImage;
		
		_planetDescription.text = planetData.PlanetDescription;
	}
}
