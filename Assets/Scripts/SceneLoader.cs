using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void StartNewLevel()
    {
        SceneManager.LoadScene("LoadScene");
    }
}
