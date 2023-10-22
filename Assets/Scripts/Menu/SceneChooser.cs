using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChooser : MonoBehaviour
{

    public void LoadLevel(int levelID)
    {
        SceneManager.LoadScene(levelID);
    }
}
