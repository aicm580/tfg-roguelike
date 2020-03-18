using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour
{
    public Text progressText;

    public void Start()
    {
        StartCoroutine(LoadGameAsync());
    }

    IEnumerator LoadGameAsync()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync("MainScene");

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f); //hacemos que en vez de 0.9, acabe en 1
            
            progressText.text = progress * 100f + "%";

            yield return null;
        }
    }
}
