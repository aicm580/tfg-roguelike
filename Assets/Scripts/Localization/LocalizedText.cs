using UnityEngine;
using UnityEngine.UI;

public class LocalizedText : MonoBehaviour
{
    public string key;
    private Text text;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>(); //cogemos el texto del objeto
        text.text = LocalizationManager.localizationInstance.GetLocalizedValue(key); //cambiamos el texto
    }

    public void UpdateText()
    {
        text.text = LocalizationManager.localizationInstance.GetLocalizedValue(key); 
    }
}
