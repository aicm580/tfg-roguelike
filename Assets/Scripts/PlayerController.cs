using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float MOVE_SPEED = 4f;
    public float BULLET_SPEED = 5.0f;
    public float SHOOTING_DISTANCE = 2.0f;

    private Rigidbody2D rb;
    private Animator animator;
    public Transform firePoint;
    public GameObject bulletPrefab;
    public GameObject crosshair;

    private Vector2 movement;
    private Vector2 mousePos;
    private Vector2 lookDir;
    


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

        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);

        //Disparo
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    private void FixedUpdate()
    {
        //Movimiento
        rb.MovePosition(rb.position + movement * MOVE_SPEED * Time.fixedDeltaTime);

        //Disparo
        Aim();
    }


    void Aim()
    {
        lookDir = (mousePos - rb.position).normalized;
        crosshair.transform.localPosition = lookDir * SHOOTING_DISTANCE;
    }


    void Shoot()
    {
        Vector2 shootingDirection = lookDir;

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position + (Vector3)(shootingDirection * 0.5f), Quaternion.identity);
        Rigidbody2D bulletRB = bullet.GetComponent<Rigidbody2D>();
        bulletRB.velocity = new Vector2(shootingDirection.x * BULLET_SPEED, shootingDirection.y * BULLET_SPEED);
    }
}
