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

    public GameObject player;
    public Transform popupDamageText;
    public GameObject gameOverPanel;
    public GameObject pausePanel;

    public Texture2D cursorTexture;

    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;

    public int currentHealth;
    public int numOfHearts;
    public bool playerAlive;
    public bool isPaused;

    private int level;

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
            else //si no está pausado, pausamos
            {
                Pause();
            }
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
        Time.timeScale = 1f;
        isPaused = false;
        pausePanel.SetActive(false);
        level = 1; 
        InitLevel();

        playerAlive = true;
        numOfHearts = playerController.initHearts;
        currentHealth = playerController.initHealth;
        SetUIHearts();
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
        playerAlive = false;
        gameOverPanel.SetActive(true);
    }

    public void Resume()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    private void Pause()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        isPaused = false;
        SceneManager.LoadScene("MenuScene");
    }
}
