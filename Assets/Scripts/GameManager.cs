using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Item currentItem;
    
    public static GameManager instance;

    private MapGenerator mapGenerator;
    private EnemiesGenerator enemiesGenerator;
    private BreakableGenerator breakableGenerator;

    public Transform playerTransform;
    [HideInInspector]
    public PlayerCharacter playerChar;
    public PlayerCharacter playerCharSO;
    [HideInInspector]
    public string playerName;
    private CharacterMovement playerMovement;
    private CharacterShooting playerShooting;

    public GameObject gameOverPanel;
    public GameObject pausePanel;
    public GameObject loadPanel;
    public GameObject optionsPanel;

    public Toggle timerToogle;
    public Toggle statsToogle;
    public Text timerText;
    public GameObject statsPanel;
    public Text moveSpeedText;
    public Text shootDelayText;
    public Text bulletDamageText;
    public Text bulletLifetimeText;
    public Text bulletSpeedText;

    [HideInInspector]
    public bool playerAlive, enemiesActive;
    [HideInInspector]
    public bool isPaused;
    private bool runIsReady = false;

    [HideInInspector]
    public int level;
    private int lastLevel = 2;

    [HideInInspector]
    public float timePlayed;
    private bool timerActive = false;

    [HideInInspector]
    public int damageDone, totalDeaths, deathsByBoss, deathsByNormalEnemies, totalKills, normalEnemiesKilled, bossesKilled, wins, travels, maxLevelReached;

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
        breakableGenerator = GetComponent<BreakableGenerator>();
        playerMovement = playerTransform.GetComponent<CharacterMovement>();
        playerShooting = playerTransform.GetComponent<CharacterShooting>();

        bool showTimer = PlayerPrefs.GetInt("TimerActive") == 1 ? true : false;
        timerText.gameObject.SetActive(showTimer);
        bool showStats = PlayerPrefs.GetInt("GameStatsActive") == 1 ? true : false;
        statsPanel.gameObject.SetActive(showStats);

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
        //Generamos los objetos rompibles del nivel
        breakableGenerator.GenerateBreakables(level);
        //Indicamos la posición inicial del jugador
        playerTransform.position = InitPlayerPosition();
        //Generamos los enemigos del nivel
        enemiesGenerator.GenerateEnemies(level);
        //Cargamos los items del nivel actual
        ItemsManager.itemsManagerInstance.SetupItems();

        ItemOnMap.SpawnItemOnMap(playerTransform.position + new Vector3 (2,2,0), currentItem);
    }

    public void InitRun()
    {
        enemiesActive = false;

        breakableGenerator.InitRandom();

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
        playerChar = playerCharSO;
        playerName = playerChar.playerName;
        playerTransform.GetComponent<PlayerHealth>().SetPlayerHealth(playerChar.initHealth, playerChar.initHearts, playerChar.maxHearts);
        playerShooting.InitializeCharacterShooting(playerChar.shootDelay, playerChar.bulletSize, playerChar.bulletsAmount, playerChar.bulletType, playerChar.shootType);
        playerMovement.InitializeCharacterMovement(playerChar.moveSpeed);
        UpdateGameStats();

        timePlayed = 0;
        timerActive = true;

        AudioManager.audioManagerInstance.PlayMusic("GameBackground");

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
        yield return new WaitForSeconds(1.85f); //esperamos 1.85f antes de que los enemigos puedan empezar a perseguir
        enemiesActive = true;
    }

    Vector3 InitPlayerPosition()
    {
        int randomIndex = Random.Range(0, (int)System.Math.Ceiling(mapGenerator.rooms[0].emptyPositions.Count / 5f));
        Vector3 randomPosition = mapGenerator.rooms[0].emptyPositions[randomIndex];

        mapGenerator.rooms[0].emptyPositions.RemoveAt(randomIndex);

        return randomPosition;
    }
   
    public void GameOver()
    {
        timerActive = false;
        playerAlive = false;
        gameOverPanel.SetActive(true);
        CursorManager.cursorInstance.SetCursor(CursorManager.cursorInstance.basicCursor);
        AudioManager.audioManagerInstance.PlaySFX("GameOver");
        AudioManager.audioManagerInstance.musicSource.Stop();
    }

    public void Resume()
    {
        optionsPanel.SetActive(false);
        pausePanel.SetActive(false);
        AudioManager.audioManagerInstance.musicSource.volume += 0.06f;
        CursorManager.cursorInstance.SetCursor(CursorManager.cursorInstance.gameCursor);
        Time.timeScale = 1f;
        isPaused = false;
        timerActive = true;
    }

    public void Restart()
    {
        loadPanel.SetActive(true);
        runIsReady = false;
        enemiesActive = false;
        SaveStats();
        ItemsManager.itemsManagerInstance.ClearItems();
        StartCoroutine(DisableLoadPanel());
        InitRun();
    }

    public void NextLevel()
    {
        if (level < lastLevel)
        {
            loadPanel.SetActive(true);
            runIsReady = false;
            StartCoroutine(DisableLoadPanel());
            level++;
            InitLevel();
            runIsReady = true;
        }
        else
        {
            Debug.Log("GAME COMPLETED");
        }
    }

    private void Pause()
    {
        timerActive = false;
        AudioManager.audioManagerInstance.musicSource.volume -= 0.06f;
        CursorManager.cursorInstance.SetCursor(CursorManager.cursorInstance.basicCursor);
        pausePanel.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void EnableOrDisableTimer()
    {
        timerText.gameObject.SetActive(timerToogle.isOn);
    }
    
    public void EnableOrDisableGameStats()
    {
        statsPanel.gameObject.SetActive(statsToogle.isOn);
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

    public void UpdateGameStats()
    {
        moveSpeedText.text = "Move Speed: " + playerMovement.moveSpeed;
        shootDelayText.text = "Shoot Delay: " + playerShooting.shootDelay;
        bulletDamageText.text = "Bullet Damage: " + playerShooting.bulletPrefab.bulletDamage;
        bulletSpeedText.text = "Bullet Speed: " + playerShooting.bulletPrefab.bulletSpeed;
        bulletLifetimeText.text = "Bullet Lifetime: " + playerShooting.bulletPrefab.bulletLifetime;
    }
}
