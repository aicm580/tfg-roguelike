using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO; //lo necesitamos porque trabajaremos con archivos y su path

public class LocalizationManager : MonoBehaviour
{
    public static LocalizationManager instance; 
    
    private Dictionary<string, string> localizedText;
    private string missingText = "Key not found";
    private bool isReady = false;

    private void Awake()
    {
        //Nos aseguramos de que solo haya 1 LocalizationManager
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    public void  LoadLocalizedText(string fileName)
    {
        localizedText = new Dictionary<string, string>();

        //Combinamos el path de la carpeta StreamingAssets con el nombre del archivo
        string filePath = Path.Combine(Application.streamingAssetsPath, fileName);
        Debug.Log(filePath);

        if(File.Exists(filePath))
        {
            string dataAsJson = File.ReadAllText(filePath);
            LocalizationData loadedData = JsonUtility.FromJson<LocalizationData>(dataAsJson);

            for (int i = 0; i < loadedData.items.Length; i++)
            {
                localizedText.Add(loadedData.items[i].key, loadedData.items[i].value);
            }
        }
        else
        {
            Debug.LogError("Cannot find localization file!");
        }

        isReady = true;
    }

    public string GetLocalizedValue(string key)
    {
        string result = missingText;

        if (localizedText.ContainsKey(key))
        {
            result = localizedText[key];
        }

        return result;
    }

    public bool GetIsReady()
    {
        return isReady;
    }
}
