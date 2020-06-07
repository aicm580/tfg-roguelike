using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class StartupManager : MonoBehaviour
{
    public AudioMixer audioMixer;
    public GameObject logoPanel;

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
        while (!LocalizationManager.localizationInstance.GetIsReady())
        {
            yield return null;
        }
        logoPanel.SetActive(true);
        yield return new WaitForSeconds(0.85f); //para que de tiempo a ver el logo

        SceneManager.LoadScene("MenuScene");
    }
}
