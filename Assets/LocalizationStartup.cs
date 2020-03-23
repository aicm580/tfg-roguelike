using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LocalizationStartup : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        

        StartCoroutine(StartIntro());
    }

    private IEnumerator StartIntro()
    {
        while (!LocalizationManager.instance.GetIsReady())
        {
            yield return null;
        }

        SceneManager.LoadScene("MenuScene");
    }
    
}
