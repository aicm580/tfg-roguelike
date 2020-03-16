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
    public Camera cam;
    public Transform firePoint;
    public GameObject bulletPrefab;
    public GameObject crosshair;

    private Vector2 movement;
    private Vector2 mousePos;
    


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);

        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        

        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * MOVE_SPEED * Time.fixedDeltaTime);

        
        Aim();
    }


    void Aim()
    {
        Vector2 lookDir = (mousePos - rb.position).normalized;
        
        crosshair.transform.localPosition = lookDir * SHOOTING_DISTANCE;
    }


    void Shoot()
    {
        Vector2 shootingDirection = crosshair.transform.localPosition;

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        bullet.GetComponent<Rigidbody2D>().velocity = shootingDirection * BULLET_SPEED;
        //bullet.transform.Rotate(0, 0, Mathf.Atan2(shootingDirection.y, shootingDirection.x) * Mathf.Rad2Deg);
    }



}
