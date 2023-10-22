using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChooser : MonoBehaviour
{

    static public void LoadLevel(int levelID)
    {
        SceneManager.LoadScene(levelID);
    }
}
