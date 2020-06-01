using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //public Item currentItem;
    
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
    public GameObject winPanel;
    public GameObject pausePanel;
    public GameObject loadPanel;
    public GameObject optionsPanel;

    public Toggle timerToogle;
    public Toggle statsToogle;
    public Text timerText;
    public GameObject statsPanel;
    public Text moveSpeedValue;
    public Text shootDelayValue;
    public Text bulletDamageValue;
    public Text bulletLifetimeValue;
    public Text bulletSpeedValue;
    public Text playerText;
    public Text killerText;

    public GameObject itemPanel;
    public Text itemNameText;
    public Text itemEffectText;

    public HealthBar bossHealthBar;

    [HideInInspector]
    public bool playerAlive, enemiesActive;
    [HideInInspector]
    public bool isPaused;
    private bool runIsReady = false;

    [HideInInspector]
    public int level;
    private int lastLevel = 2;

    [HideInInspector]
    public int totalDeaths, deathsByBoss, deathsByNormalEnemies, deathsByItems, deathsByObstacles,
               damageDone, totalKills, normalEnemiesKilled, bossesKilled, wins, travels, maxLevelReached;
    [HideInInspector]
    public float timePlayed, recordTime;
    private bool timerActive = false;

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
                Resume();
            else if (playerAlive) //si no está pausado y el jugador sigue vivo, pausamos
                Pause();
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

        //ItemOnMap.SpawnItemOnMap(playerTransform.position + new Vector3 (2,2,0), currentItem);
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
        deathsByObstacles = 0;
        deathsByItems = 0;
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
        itemPanel.SetActive(false);

        level = 1;
        InitLevel();

        playerAlive = true;
        InitializePlayer();

        UpdateGameStats();

        timePlayed = 0;
        timerActive = true;

        AudioManager.audioManagerInstance.PlayMusic("GameBackground");

        runIsReady = true;
    }

    private void InitializePlayer()
    {
        playerChar = playerCharSO;
        playerName = playerChar.playerName;
        playerTransform.GetComponent<PlayerHealth>().SetPlayerHealth(playerChar.initHealth, playerChar.initHearts, playerChar.maxHearts);
        playerTransform.GetComponent<PlayerInputController>().abilityDuration = playerChar.abilityDuration;
        playerShooting.InitializeCharacterShooting(playerChar.shootDelay, playerChar.bulletSize, playerChar.bulletsAmount, playerChar.bulletType, playerChar.shootType);
        playerMovement.InitializeCharacterMovement(playerChar.moveSpeed);
    }

    private IEnumerator DisableLoadPanel()
    {
        while (!runIsReady)
        {
            yield return null;
        }
        yield return new WaitForSeconds(0.85f); //para que de tiempo a leer la frase de carga
        loadPanel.SetActive(false);
        CursorManager.cursorInstance.SetCursor(CursorManager.cursorInstance.gameCursor);
        yield return new WaitForSeconds(1.1f); //esperamos 1.1f antes de que los enemigos puedan empezar a perseguir
        enemiesActive = true;
    }

    private Vector3 InitPlayerPosition()
    {
        int randomIndex = Random.Range(0, (int)System.Math.Ceiling(MapGenerator.rooms[0].emptyPositions.Count / 5f));
        Vector3 randomPosition = MapGenerator.rooms[0].emptyPositions[randomIndex];

        MapGenerator.rooms[0].emptyPositions.RemoveAt(randomIndex);

        return randomPosition;
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
        enemiesActive = true;
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

    public void GameOver(string dmgOriginName)
    {
        bossHealthBar.DisableHealthBar();
        timerActive = false;
        playerAlive = false;
        enemiesActive = false;
        playerText.text = playerName.ToUpper();
        killerText.text = LocalizationManager.localizationInstance.GetLocalizedValue(dmgOriginName);
        gameOverPanel.SetActive(true);
        CursorManager.cursorInstance.SetCursor(CursorManager.cursorInstance.basicCursor);
        AudioManager.audioManagerInstance.musicSource.Stop();
        AudioManager.audioManagerInstance.PlaySFX("GameOver");
    }

    private void Win()
    {
        Debug.Log("GAME COMPLETED");
        wins++;
        timerActive = false;
        playerAlive = false;
        enemiesActive = false;
        winPanel.SetActive(true);
        CursorManager.cursorInstance.SetCursor(CursorManager.cursorInstance.basicCursor);
    }

    public void NextLevel()
    {
        if (level < lastLevel)
        {
            loadPanel.SetActive(true);
            enemiesActive = false;
            runIsReady = false;
            StartCoroutine(DisableLoadPanel());
            level++;
            InitLevel();
            runIsReady = true;
        }
        else
        {
            Debug.Log(level);
            Win();
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
        AudioManager.audioManagerInstance.musicSource.Stop();
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
            totalDeaths = deathsByBoss + deathsByNormalEnemies + deathsByItems + deathsByObstacles + stats.totalDeaths;
            deathsByBoss += stats.deathsByBoss;
            deathsByNormalEnemies += stats.deathsByNormalEnemies;
            deathsByItems += stats.deathsByItems;
            deathsByObstacles += stats.deathsByObstacles;
            totalKills = normalEnemiesKilled + bossesKilled + stats.totalKills;
            normalEnemiesKilled += stats.normalEnemiesKilled;
            bossesKilled += stats.bossesKilled;
            wins += stats.wins;
            travels = stats.travels + 1;
            
            if (stats.maxLevelReached <= level)
                maxLevelReached = level;
            else
                maxLevelReached = stats.maxLevelReached;

            //Si ha ganado esta partida, miramos si ha cumplido unn tiempo récord
            if (wins > stats.wins && stats.recordTime > timePlayed)
                recordTime = timePlayed;
            else
                recordTime = stats.recordTime;

            timePlayed += stats.timePlayed;
        }

        SaveManager.SaveStats(this);
    }

    public void ShowItemPanel(string itemName)
    {
        itemPanel.gameObject.SetActive(true);
        string itemDescription = itemName + " Effect";
        itemNameText.text = LocalizationManager.localizationInstance.GetLocalizedValue(itemName);
        itemEffectText.text = LocalizationManager.localizationInstance.GetLocalizedValue(itemDescription); ;
        StartCoroutine(DisableItemPanel());
    }

    private IEnumerator DisableItemPanel()
    {
        yield return new WaitForSeconds(3.5f);
        itemPanel.SetActive(false);
    }

    public void UpdateGameStats()
    {
        moveSpeedValue.text = playerMovement.moveSpeed.ToString();
        shootDelayValue.text = playerShooting.shootDelay.ToString();
        bulletDamageValue.text = playerShooting.bulletPrefab.bulletDamage.ToString();
        bulletSpeedValue.text = playerShooting.bulletPrefab.bulletSpeed.ToString();
        bulletLifetimeValue.text = playerShooting.bulletPrefab.bulletLifetime.ToString();
    }
}
