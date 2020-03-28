using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class StartupManager : MonoBehaviour
{
    public AudioMixer audioMixer;

    private float musicVol;
    private float sfxVol;
    private int screenWidth;
    private int screenHeight;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("MusicVol"))
        {
            musicVol = PlayerPrefs.GetFloat("MusicVol");
            audioMixer.SetFloat("MusicVol", musicVol);
        }

        if (PlayerPrefs.HasKey("SfxVol"))
        {
            sfxVol = PlayerPrefs.GetFloat("SfxVol");
            audioMixer.SetFloat("SfxVol", sfxVol);
        }

        if (PlayerPrefs.HasKey("FullScreen"))
        {
            Screen.fullScreen = PlayerPrefs.GetInt("FullScreen") == 1 ? true : false;
        }

        if (PlayerPrefs.HasKey("ScreenWidth") && PlayerPrefs.HasKey("ScreenHeight"))
        {
            screenWidth = PlayerPrefs.GetInt("ScreenWidth");
            screenHeight = PlayerPrefs.GetInt("ScreenHeight");
            Screen.SetResolution(screenWidth, screenHeight, Screen.fullScreen);
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
