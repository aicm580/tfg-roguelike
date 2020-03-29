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
    public PlayerController playerController;

    public Transform popupDamageText;
    public GameObject player;
    public GameObject gameOverPanel;
    public GameObject pausePanel;
    public Text timerText;

    public Texture2D cursorTexture;

    public Image[] hearts;

    public int currentHealth;
    public int numOfHearts;
   
    public bool playerAlive;
    public bool isPaused;

    private int level;

    private float timer;
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
    public int timePlayed;
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
        SetCursor(cursorTexture);

        mapGenerator = GetComponent<MapGenerator>();
        enemiesGenerator = GetComponent<EnemiesGenerator>();
        playerController = player.GetComponent<PlayerController>();

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
            timer += Time.deltaTime;
            int intTime = (int)timer;
            int minutes = intTime / 60;
            int seconds = intTime % 60;
            float ms = timer * 1000;
            ms = ms % 1000;

            string timeTxt = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, ms/10);
            timerText.text = timeTxt;
        }
    }

    void SetCursor(Texture2D tex)
    {
        CursorMode mode = CursorMode.ForceSoftware;
        float xspot = tex.width / 2;
        float yspot = tex.height / 2;
        Vector2 hotSpot = new Vector2(xspot, yspot);
        Cursor.SetCursor(tex, hotSpot, mode);
    }

    void InitLevel()
    { 
        //Generamos el mapa del nivel
        mapGenerator.SetupMap();
        //Indicamos la posición inicial del jugador
        player.transform.position = InitPlayerPosition();
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
        timePlayed = 0;
        maxLevelReached = level;

        Time.timeScale = 1f;
        isPaused = false;
        pausePanel.SetActive(false);
        gameOverPanel.SetActive(false);
        level = 1; 
        InitLevel();

        playerAlive = true;
        numOfHearts = playerController.initHearts;
        currentHealth = playerController.initHealth;
        SetUIHearts();
        timerActive = true;
    }

    Vector3 InitPlayerPosition()
    {
        int randomIndex = Random.Range(0, (int)System.Math.Ceiling(mapGenerator.rooms[0].emptyPositions.Count / 2.5f));
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

    public void SetUIHearts()
    {
        if (currentHealth > numOfHearts)
        {
            currentHealth = numOfHearts;
        }

        for (int i = 0; i < hearts.Length; i++)
        {
            if (i >= currentHealth)
            {
                hearts[i].GetComponent<Animator>().SetBool("empty", true);
            }
            else
            {
                hearts[i].GetComponent<Animator>().SetBool("empty", false);
            }

            if (i < numOfHearts)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }
        }
    }

    public void LoseLife()
    {
        hearts[currentHealth - 1].GetComponent<Animator>().SetTrigger("loseHeart");
        hearts[currentHealth - 1].GetComponent<Animator>().SetBool("empty", true);
        currentHealth -= 1;
        Debug.Log("YOU LOST A LIFE");
        
        CheckIfAlive();
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
    }

    public void Resume()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        timerActive = true;
    }

    public void Restart()
    {
        SaveStats();
        InitRun();
    }

    private void Pause()
    {
        timerActive = false;
        pausePanel.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
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
            //timePlayed;

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
