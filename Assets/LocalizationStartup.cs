using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LocalizationStartup : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //Si el idioma del sistema del usuario es español o catalán, cargamos los textos en español
        if(Application.systemLanguage == SystemLanguage.Spanish || Application.systemLanguage == SystemLanguage.Catalan)
        {
            LocalizationManager.instance.LoadLocalizedText("localizationText_es.json");
            Debug.Log("Español");
        }
        else
        {
            LocalizationManager.instance.LoadLocalizedText("localizationText_en.json");
            Debug.Log("English");
        }

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
