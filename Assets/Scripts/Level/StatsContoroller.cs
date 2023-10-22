using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatsContoroller : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject _victoryScreen;
    [SerializeField] private GameObject _lowerHUD;
    [SerializeField] private GameObject _toMenuButton;

    [SerializeField] private Slider _missionProgressBar;

    [SerializeField] private TMP_Text[] _tasks;

    [SerializeField] private TMP_Text _resourcesBar;
    [SerializeField] private TMP_Text _armyBar;

	private bool _isPlayerWin = false;

	void Start()
    {
		_victoryScreen.SetActive(false);

		_toMenuButton.SetActive(false);

		
	}

    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.K))
        //    ShowVictoryScreen();

        if (_missionProgressBar.value >= _missionProgressBar.maxValue)
            ShowVictoryScreen();

		_resourcesBar.text = $"{ResourcesController.Instance.Resources}/{ResourcesController.Instance.MaxResourcesAmount}";

		_armyBar.text = $"{ResourcesController.Instance.Army}/{ResourcesController.Instance.MaxArmyCapacity}"; 
	}

    public void ShowVictoryScreen()
    {
        if (_isPlayerWin)
            return;

        _isPlayerWin = true;

		foreach (var text in _tasks)
        {
            text.fontStyle = (int)FontStyles.Strikethrough + FontStyles.Bold;

            text.color = Color.gray;
        }

		_lowerHUD.SetActive(false);

		_victoryScreen.SetActive(true);	
	}

    public static void UpdateBars()
    {
		StatsContoroller stats = new StatsContoroller();

		//stats._resourcesBar.text = $"{(int)ResorcesController.Instance.Resources}/{ResorcesController.Instance.MaxResourcesAmount}";

		//stats._armyBar.text = $"{(int)ResorcesController.Instance.Army}/{ResorcesController.Instance.MaxArmyCapacity}";
	}
}
