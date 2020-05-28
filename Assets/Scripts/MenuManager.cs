using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

public class MenuManager : MonoBehaviour
{
    public Toggle timerToogle;
    public Toggle statsToogle;
    public Toggle fullscreenToggle;
    public Dropdown resolutionDropdown;
    public Slider musicSlider;
    public Slider sfxSlider;
    public Text languageText;
    public AudioMixer audioMixer;
    public GameObject optionsPanel;

    private Resolution[] resolutions;
    private int currentResolutionIndex;

    private string[] languages = { "español", "english" };
    private int selectedLang = 0;

    private float musicVol;
    private float musicSliderValue;
    private float sfxVol;
    private float sfxSliderValue;
    

    private void Start()
    {
        CursorManager.cursorInstance.SetCursor(CursorManager.cursorInstance.basicCursor);

        //APARTADO "RESOLUCIONES"
        resolutions = Screen.resolutions.Select(resolution => new Resolution
        {
            width = resolution.width,
            height = resolution.height
        }).Where(resolution => resolution.width > 450 && resolution.height > 350).Distinct().ToArray(); //Distinct() evitará que aparezcan resoluciones duplicadas

        resolutionDropdown.ClearOptions();

        List<string> resolutionOptions = new List<string>(); //Debemos crear una lista de strings, porque el dropdown solo acepta strings

        currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            resolutionOptions.Add(option);
        }
        resolutionDropdown.AddOptions(resolutionOptions);

        SetOptionsPanelValues();
    }

    public void SetOptionsPanelValues()
    {
        //APARTADO "SHOW TIMER"
        if (PlayerPrefs.HasKey("TimerActive"))
            timerToogle.isOn = PlayerPrefs.GetInt("TimerActive") == 1 ? true : false;
        else
            timerToogle.isOn = false;

        //APARTADO "SHOW GAME STATS"
        if (PlayerPrefs.HasKey("GameStatsActive"))
            statsToogle.isOn = PlayerPrefs.GetInt("GameStatsActive") == 1 ? true : false;
        else
            statsToogle.isOn = false;

        //APARTADO "FULL SCREEN"
        fullscreenToggle.isOn = Screen.fullScreen;

        //APARTADO "RESOLUTION"
        for (int i = 0; i < resolutions.Length; i++)
        {
            if (resolutions[i].width == Screen.width &&
                resolutions[i].height == Screen.height)
            {
                currentResolutionIndex = i;
            }
        }
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        //APARTADO "IDIOMAS"
        if (PlayerPrefs.HasKey("UserLanguage"))
            selectedLang = System.Array.IndexOf(languages, PlayerPrefs.GetString("UserLanguage"));
        languageText.text = languages[selectedLang];

        //APARTADO "SONIDO"
        audioMixer.GetFloat("MusicVol", out musicVol);
        musicSlider.value = Mathf.Pow(10.0f, musicVol / 20.0f);
        musicSliderValue = musicSlider.value;
        audioMixer.GetFloat("SfxVol", out sfxVol);
        sfxSlider.value = Mathf.Pow(10.0f, sfxVol / 20.0f);
        sfxSliderValue = sfxSlider.value;
    }

    public void StartNewLevel()
    {
        SceneManager.LoadScene("LoadScene");
    }
       
    private void SetResolution(int resolutionIndex)
    {
        if (resolutionDropdown.value != currentResolutionIndex)
        {
            Resolution resolution = resolutions[resolutionIndex];
            Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
            currentResolutionIndex = resolutionDropdown.value;
        }
    }

    private void ApplyFullScreen()
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
            selectedLang--;
        else
            selectedLang = languages.Length - 1; 

        languageText.text = languages[selectedLang];
    }

    public void LanguageRightArrow()
    {
        if (selectedLang < languages.Length - 1)
            selectedLang++;
        else
            selectedLang = 0;

        languageText.text = languages[selectedLang];
    }

    private void SetLanguage()
    {
        if (PlayerPrefs.GetString("UserLanguage") != languages[selectedLang])
        {
            LocalizationManager.localizationInstance.LoadLocalizedText(languages[selectedLang]);
            PlayerPrefs.SetString("UserLanguage", languages[selectedLang]);

            Text[] sceneTexts = FindObjectsOfType<Text>();
            foreach (Text text in sceneTexts)
            {
                if (text.GetComponent<LocalizedText>() != null)
                    text.GetComponent<LocalizedText>().UpdateText();
            }
        }
    }

    private void SetMusicVolume()
    {
        if (musicSliderValue != musicSlider.value)
        {
            musicSliderValue = musicSlider.value;
            musicVol = Mathf.Log10(musicSlider.value) * 20;
            audioMixer.SetFloat("MusicVol", musicVol);
        }
    }

    private void SetSfxVolume()
    {
        if (sfxSliderValue != sfxSlider.value)
        {
            sfxSliderValue = sfxSlider.value;
            sfxVol = Mathf.Log10(sfxSlider.value) * 20;
            audioMixer.SetFloat("SfxVol", sfxVol);
        }
    }

    public void SaveOptions()
    {
        SetLanguage();

        SetMusicVolume();
        PlayerPrefs.SetFloat("MusicVol", musicVol);
        SetSfxVolume();
        PlayerPrefs.SetFloat("SfxVol", sfxVol);

        if (Screen.fullScreen != fullscreenToggle.isOn)
        {
            Debug.Log("NECESITAS CAMBIAR FULLSCREEN");
            ApplyFullScreen();
            PlayerPrefs.SetInt("FullScreen", fullscreenToggle.isOn ? 1 : 0);
        }
            
        SetResolution(resolutionDropdown.value);  
        PlayerPrefs.SetInt("ScreenWidth", Screen.width);
        PlayerPrefs.SetInt("ScreenHeight", Screen.height);

        if (PlayerPrefs.GetInt("TimerActive") != (timerToogle.isOn ? 1 : 0) || !PlayerPrefs.HasKey("TimerActive"))
            PlayerPrefs.SetInt("TimerActive", timerToogle.isOn ? 1 : 0);

        if (PlayerPrefs.GetInt("GameStatsActive") != (statsToogle.isOn ? 1 : 0) || !PlayerPrefs.HasKey("GameStatsActive")) 
            PlayerPrefs.SetInt("GameStatsActive", statsToogle.isOn ? 1 : 0);
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }
}
