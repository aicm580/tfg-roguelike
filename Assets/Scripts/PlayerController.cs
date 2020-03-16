using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public string playerName = "Rufo";
    public int currentHealth = 3; 
    public int hearts = 3; //nº de vidas del personaje
    public float move_speed = 4f; //velocidad del personaje
    public int bullet_damage = 5; //daño que inflingen las balas del personaje
    public float bullet_speed = 5.0f; //velocidad de las balas del personaje
    public float bullet_lifetime = 0.9f; //tiempo de vida de las balas del personaje
    public float bullet_delay = 0.2f; //tiempo a esperar entre que se lanza una bala y la siguiente

    private Rigidbody2D rb;
    private Animator animator;
    public Transform firePoint;
    public GameObject bulletPrefab;
    public GameObject crosshair;

    private Vector2 movement;
    private Vector2 mousePos;
    private Vector2 lookDirection;

    private bool canShoot = true;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //Movimiento
        movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        animator.SetFloat("Speed", movement.sqrMagnitude);

        //Disparo
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        animator.SetFloat("Horizontal", lookDirection.x);
        animator.SetFloat("Vertical", lookDirection.y);

        if (Input.GetButtonDown("Fire1") && canShoot)
        {
            Shoot();
        }
    }

    private void FixedUpdate()
    {
        //Movimiento
        rb.MovePosition(rb.position + movement * move_speed * Time.fixedDeltaTime);

        //Disparo
        lookDirection = (mousePos - rb.position).normalized;
    }


    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position + (Vector3)(lookDirection * 0.5f), Quaternion.identity);
        Rigidbody2D bulletRB = bullet.GetComponent<Rigidbody2D>();
        bulletRB.velocity = new Vector2(lookDirection.x * bullet_speed, lookDirection.y * bullet_speed);
        Destroy(bullet, bullet_lifetime);

        canShoot = false;
        StartCoroutine(ShootDelay());
    }

    IEnumerator ShootDelay()
    {
        yield return new WaitForSeconds(bullet_delay);
        canShoot = true;
    }
}
