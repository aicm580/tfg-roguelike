using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocalizedText : MonoBehaviour
{
    public string key; 

    // Start is called before the first frame update
    void Start()
    {
        Text text = GetComponent<Text>(); //cogemos el texto del objeto
        text.text = LocalizationManager.localizationInstance.GetLocalizedValue(key); //cambiamos el texto
    }
}
