using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StatsContoroller : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject _victoryScreen;
    [SerializeField] private GameObject _lowerHUD;

    [SerializeField] private GameObject _pauseMenu;

    [SerializeField] private Slider _missionProgressBar;

    [SerializeField] private TMP_Text[] _tasks;

    [SerializeField] private TMP_Text _resourcesBar;
    [SerializeField] private TMP_Text _armyBar;

	private bool _isPlayerWin = false;
    private bool _isPauseMenuOn = false;

	void Start()
    {
		_victoryScreen.SetActive(false);

		_pauseMenu.SetActive(false);	
	}

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
			OnPauseMenu();

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

    public void OnPauseMenu()
    {
        _isPauseMenuOn = !_isPauseMenuOn;

        Time.timeScale = _isPauseMenuOn ? 0f: 1f;

		_lowerHUD.SetActive(!_isPauseMenuOn);

		_pauseMenu.SetActive(_isPauseMenuOn);
	}

	public void ResetTimeScaleToDeffault() //wrong
	{
        Time.timeScale = 1f;
	}

    public void QuitGame() //wrong
    {
        Application.Quit();
    }
}
