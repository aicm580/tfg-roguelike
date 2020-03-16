using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float MOVE_SPEED = 4f;
    public int BULLET_DAMAGE = 5;
    public float BULLET_SPEED = 5.0f;
    public float BULLET_LIFETIME = 0.9f;
    public float BULLET_DELAY = 0.25f;

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
        rb.MovePosition(rb.position + movement * MOVE_SPEED * Time.fixedDeltaTime);

        //Disparo
        lookDirection = (mousePos - rb.position).normalized;
    }


    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position + (Vector3)(lookDirection * 0.5f), Quaternion.identity);
        Rigidbody2D bulletRB = bullet.GetComponent<Rigidbody2D>();
        bulletRB.velocity = new Vector2(lookDirection.x * BULLET_SPEED, lookDirection.y * BULLET_SPEED);
        Destroy(bullet, BULLET_LIFETIME);

        canShoot = false;
        StartCoroutine(ShootDelay());
    }

    IEnumerator ShootDelay()
    {
        yield return new WaitForSeconds(BULLET_DELAY);
        canShoot = true;
    }
}
