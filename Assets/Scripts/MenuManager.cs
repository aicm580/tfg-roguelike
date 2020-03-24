using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;
using System.Linq;

public class MenuManager : MonoBehaviour
{
    public Toggle fullscreenToggle;
    public Dropdown resolutionDropdown;
    public Text languageText;

    private Resolution[] resolutions;
    private string[] languages = {"español", "english"};
    private int selectedLang = 0;

    void Start()
    {
        //APARTADO "RESOLUCIONES"
        resolutions = Screen.resolutions.Select(resolution => new Resolution
        {
            width = resolution.width,
            height = resolution.height
        }).Where(resolution => resolution.width > 400 && resolution.height > 350).Distinct().ToArray(); //esto evitará que aparezcan resoluciones duplicadas

        resolutionDropdown.ClearOptions();

        //Debemos crear una lista de strings, porque el dropdown solo acepta strings
        List<string> resolutionOptions = new List<string>();

        int currentResolutionIndex = 0; 
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            resolutionOptions.Add(option);

            if (resolutions[i].width == Screen.width &&
                resolutions[i].height == Screen.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(resolutionOptions);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        //APARTADO "IDIOMAS"
        if (PlayerPrefs.HasKey("UserLanguage"))
        {
            selectedLang = System.Array.IndexOf(languages, PlayerPrefs.GetString("UserLanguage"));
        }

        languageText.text = languages[selectedLang];
    }

    public void StartNewLevel()
    {
        SceneManager.LoadScene("LoadScene");
    }

    public void ApplyFullScreen()
    {
        if (fullscreenToggle.isOn)
        {
            Screen.fullScreen = true;
        }
        else
        {
            Screen.fullScreen = false;
        }
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void LanguageLeftArrow()
    {
        if (selectedLang > 0)
        {
            selectedLang--;
        }
        else
        {
            selectedLang = languages.Length - 1; 
        }

        languageText.text = languages[selectedLang];
    }

    public void LanguageRightArrow()
    {
        if (selectedLang < languages.Length - 1)
        {
            selectedLang++;
        }
        else
        {
            selectedLang = 0;
        }

        languageText.text = languages[selectedLang];
    }

    public void ChangeLanguage()
    {
        if (PlayerPrefs.GetString("UserLanguage") != languages[selectedLang])
        {
            LocalizationManager.instance.LoadLocalizedText(languages[selectedLang]);
            PlayerPrefs.SetString("UserLanguage", languages[selectedLang]);
        }
    }

    public void SetMusicVolume()
    {

    }

    public void SetSfxVolume()
    {

    }
}
