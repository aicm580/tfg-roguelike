using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

public class MenuManager : MonoBehaviour
{
    public Toggle timerToogle;
    public Toggle fullscreenToggle;
    public Dropdown resolutionDropdown;
    public Slider musicSlider;
    public Slider sfxSlider;
    public Text languageText;
    public AudioMixer audioMixer;
    public GameObject optionsPanel;

    private Resolution[] resolutions;
    private string[] languages = { "español", "english" };
    private int selectedLang = 0;

    private float musicVol;
    private float sfxVol;
    

    private void Start()
    {
        CursorManager.cursorInstance.SetCursor(CursorManager.cursorInstance.basicCursor);

        //APARTADO "TIMER ACTIVE"
        if (PlayerPrefs.HasKey("TimerActive"))
        {
            timerToogle.isOn = PlayerPrefs.GetInt("TimerActive") == 1 ? true : false;
        }
        else
        {
            timerToogle.isOn = false;
        }
        
        //APARTADO "FULL SCREEN"
        fullscreenToggle.isOn = Screen.fullScreen;

        //APARTADO "RESOLUCIONES"
        resolutions = Screen.resolutions.Select(resolution => new Resolution
        {
            width = resolution.width,
            height = resolution.height
        }).Where(resolution => resolution.width > 400 && resolution.height > 350).Distinct().ToArray(); //esto evitará que aparezcan resoluciones duplicadas

        resolutionDropdown.ClearOptions();
                
        List<string> resolutionOptions = new List<string>(); //Debemos crear una lista de strings, porque el dropdown solo acepta strings

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

        //APARTADO "SONIDO"
        audioMixer.GetFloat("MusicVol", out musicVol);
        musicSlider.value = Mathf.Pow(10.0f, musicVol / 20.0f);
        audioMixer.GetFloat("SfxVol", out sfxVol);
        sfxSlider.value = Mathf.Pow(10.0f, sfxVol / 20.0f);
    }


    public void StartNewLevel()
    {
        SceneManager.LoadScene("LoadScene");
    }
       
    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
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
            LocalizationManager.localizationInstance.LoadLocalizedText(languages[selectedLang]);
            PlayerPrefs.SetString("UserLanguage", languages[selectedLang]);

            Text[] sceneTexts = FindObjectsOfType<Text>();
            foreach (Text text in sceneTexts)
            {
                if (text.GetComponent<LocalizedText>() != null)
                {
                    text.GetComponent<LocalizedText>().UpdateText();
                }
            }
        }
    }

    public void SetMusicVolume(float sliderValue)
    {
        musicVol = Mathf.Log10(sliderValue) * 20;
        audioMixer.SetFloat("MusicVol", musicVol);
    }

    public void SetSfxVolume(float sliderValue)
    {
        sfxVol = Mathf.Log10(sliderValue) * 20;
        audioMixer.SetFloat("SfxVol", sfxVol);
    }

    public void SaveOptions()
    {
        ChangeLanguage();

        PlayerPrefs.SetFloat("MusicVol", musicVol);
        PlayerPrefs.SetFloat("SfxVol", sfxVol);

        PlayerPrefs.SetInt("FullScreen", fullscreenToggle.isOn ? 1 : 0);
        PlayerPrefs.SetInt("ScreenWidth", Screen.width);
        PlayerPrefs.SetInt("ScreenHeight", Screen.height);

        PlayerPrefs.SetInt("TimerActive", timerToogle.isOn ? 1 : 0);

        optionsPanel.SetActive(false);
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }
}
