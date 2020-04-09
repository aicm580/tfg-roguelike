using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController playerInstance;

    public string playerName = "Rufo";
    public int initHealth = 3; //nº de vidas con el que empieza el jugador
    public int initHearts = 3; //nº de vidas del personaje
    public int maxHearts = 10; //nº de corazones máximo que puede recolectar el personaje
    public int bulletDamage = 10;
    
    public float bulletDelay = 0.2f; //tiempo a esperar entre que se lanza una bala y la siguiente
    public float abilityDuration = 3f; //tiempo que permanece activa la habilidad especial

    private Animator animator;
    public Transform firePoint;
    public GameObject bulletPrefab;

    private Vector2 mousePos;
    private Vector2 firepointPos;
    private Vector2 lookDirection;

    private bool canShoot = true;
    private bool abilityActive = false;

    private void Awake()
    {
        //Nos aseguramos de que solo haya 1 GameManager
        if (playerInstance == null)
        {
            playerInstance = this;
        }
        else if (playerInstance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.playerAlive) //si el jugador está vivo
        {
            //Disparo
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            animator.SetFloat("Horizontal", lookDirection.x);
            animator.SetFloat("Vertical", lookDirection.y);

            if (Input.GetButtonDown("Fire1") && canShoot && !abilityActive && !GameManager.instance.isPaused)
            {
                //Shoot();
            }

            //Habilidad especial
            if (Input.GetKeyDown(KeyCode.Space) && !abilityActive)
            {
                abilityActive = true;
                StartCoroutine(DisableAbility());
            }
        }
    }

    private void FixedUpdate()
    {
        if (GameManager.instance.playerAlive) //si el jugador está vivo
        {
            
            //Disparo
            firepointPos = new Vector2(firePoint.position.x, firePoint.position.y);
            lookDirection = (mousePos - firepointPos).normalized;
        }
    }

    IEnumerator DisableAbility()
    {
        yield return new WaitForSeconds(abilityDuration);
        abilityActive = false;
        Debug.Log("HABILIDAD ESPECIAL DESACTIVADA");
    }
}
