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
    
    

    

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.playerAlive) //si el jugador está vivo
        {
            /*
            //Disparo
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            animator.SetFloat("Horizontal", lookDirection.x);
            animator.SetFloat("Vertical", lookDirection.y);

            if (Input.GetButtonDown("Fire1") && canShoot && !abilityActive && !GameManager.instance.isPaused)
            {
                //Shoot();
            }
            */
            //Habilidad especial
            if (Input.GetKeyDown(KeyCode.Space) && !abilityActive)
            {
                
            }
        }
    }

    private void FixedUpdate()
    {
        if (GameManager.instance.playerAlive) //si el jugador está vivo
        {
            /*
            //Disparo
            firepointPos = new Vector2(firePoint.position.x, firePoint.position.y);
            lookDirection = (mousePos - firepointPos).normalized;
            */
        }
    }

    
}
