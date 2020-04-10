using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private MapGenerator mapGenerator;
    private EnemiesGenerator enemiesGenerator;

    public GameObject gameOverPanel;
    public GameObject pausePanel;
    public GameObject loadPanel;
    public Toggle timerToogle;
    public Text timerText;
   
    public bool playerAlive;
    public bool isPaused;
    private bool runIsReady = false;

    private int level;

    public float timePlayed;
    private bool timerActive = false;

    public int damageDone;
    public int totalDeaths;
    public int deathsByBoss;
    public int deathsByNormalEnemies;
    public int totalKills;
    public int normalEnemiesKilled;
    public int bossesKilled;
    public int wins;
    public int travels;
    public int maxLevelReached;

    private StatsData stats;


    private void Awake()
    {
        //Nos aseguramos de que solo haya 1 GameManager
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        mapGenerator = GetComponent<MapGenerator>();
        enemiesGenerator = GetComponent<EnemiesGenerator>();

        bool showTimer = PlayerPrefs.GetInt("TimerActive") == 1 ? true : false;
        timerText.gameObject.SetActive(showTimer);
        
        loadPanel.SetActive(true);
        StartCoroutine(DisableLoadPanel());
        InitRun();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(isPaused) //si ya está pausado, reanudamos
            {
                Resume();
            }
            else if (playerAlive) //si no está pausado y el jugador sigue vivo, pausamos
            {
                Pause();
            }
        }

        if (timerActive)
        {
            timePlayed += Time.deltaTime;
            int intTime = (int)timePlayed;
            int minutes = intTime / 60;
            int seconds = intTime % 60;
            float ms = timePlayed * 1000;
            ms = ms % 1000;

            string timeTxt = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, ms/10);
            timerText.text = timeTxt;
        }
    }

    void InitLevel()
    { 
        //Generamos el mapa del nivel
        mapGenerator.SetupMap();
        //Indicamos la posición inicial del jugador
        PlayerController.playerInstance.transform.position = InitPlayerPosition();
        //Generamos los enemigos del nivel
        enemiesGenerator.GenerateEnemies(level);
    }

    public void InitRun()
    {
        stats = SaveManager.LoadStats();
        damageDone = 0;
        totalDeaths = 0;
        deathsByBoss = 0;
        deathsByNormalEnemies = 0;
        totalKills = 0;
        normalEnemiesKilled = 0;
        bossesKilled = 0;
        wins = 0;
        travels = 0;
        maxLevelReached = level;

        Time.timeScale = 1f;
        isPaused = false;
        pausePanel.SetActive(false);
        gameOverPanel.SetActive(false);
        level = 1; 
        InitLevel();

        playerAlive = true;

        timePlayed = 0;
        timerActive = true;

        runIsReady = true;
    }

    IEnumerator DisableLoadPanel()
    {
        while (!runIsReady)
        {
            yield return null;
        }
        yield return new WaitForSeconds(0.85f); //para que de tiempo a leer la frase de carga
        loadPanel.SetActive(false);
        CursorManager.cursorInstance.SetCursor(CursorManager.cursorInstance.gameCursor);
    }

    Vector3 InitPlayerPosition()
    {
        int randomIndex = Random.Range(0, (int)System.Math.Ceiling(mapGenerator.rooms[0].emptyPositions.Count / 3.5f));
        Vector3 randomPosition = mapGenerator.rooms[0].emptyPositions[randomIndex];

        mapGenerator.rooms[0].emptyPositions.RemoveAt(randomIndex);

        return randomPosition;
    }
    
    //Esta función permite comprobar si una posición concreta de una sala concreta está disponible
    public bool CheckPosition(Vector3 positionToCheck, int room)
    {
        if (mapGenerator.rooms[room].emptyPositions.Contains(positionToCheck))
        {
            return true;
        }

        return false;
    }
    

    private void CheckIfAlive()
    {
        if (currentHealth <= 0)
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        timerActive = false;
        playerAlive = false;
        gameOverPanel.SetActive(true);
        CursorManager.cursorInstance.SetCursor(CursorManager.cursorInstance.basicCursor);
    }

    public void Resume()
    {
        pausePanel.SetActive(false);
        CursorManager.cursorInstance.SetCursor(CursorManager.cursorInstance.gameCursor);
        Time.timeScale = 1f;
        isPaused = false;
        timerActive = true;
    }

    public void Restart()
    {
        loadPanel.SetActive(true);
        runIsReady = false;
        SaveStats();
        StartCoroutine(DisableLoadPanel());
        InitRun();
    }

    private void Pause()
    {
        timerActive = false;
        CursorManager.cursorInstance.SetCursor(CursorManager.cursorInstance.basicCursor);
        pausePanel.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void EnableOrDisableTimer()
    {
        timerText.gameObject.SetActive(timerToogle.isOn);
    }

    public void LoadMenu()
    {
        SaveStats();
        Time.timeScale = 1f;
        isPaused = false;
        SceneManager.LoadScene("MenuScene");
    }

    public void SaveAndQuit()
    {
        SaveStats();
        Debug.Log("Quitting game...");
        Application.Quit();
    }

    private void SaveStats()
    {
        if (stats != null)
        {
            damageDone += stats.damageDone;
            totalDeaths += stats.totalDeaths;
            deathsByBoss += stats.deathsByBoss;
            deathsByNormalEnemies += stats.deathsByNormalEnemies;
            totalKills += stats.totalKills;
            normalEnemiesKilled += stats.normalEnemiesKilled;
            bossesKilled += stats.bossesKilled;
            wins += stats.wins;
            travels = stats.travels + 1;
            timePlayed += stats.timePlayed;

            if (stats.maxLevelReached <= level)
            {
                maxLevelReached = level;
            }
            else
            {
                maxLevelReached = stats.maxLevelReached;
            }  
        }

        SaveManager.SaveStats(this);
    }
}
