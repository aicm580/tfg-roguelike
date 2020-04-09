using UnityEngine;

[RequireComponent(typeof(CharacterMovement))]
public class PlayerInputController : MonoBehaviour
{
    public Transform firePoint;

    private CharacterMovement characterMovement;
    private CharacterShooting characteShooting;

    private Vector2 movement;
    private Vector2 mousePos;
    private Vector2 firepointPos;
    private Vector2 lookDirection;
    private Vector3 bulletOrigin = new Vector3();
    private Animator animator;

    void Awake()
    {
        characterMovement = GetComponent<CharacterMovement>();
        characteShooting = GetComponent<CharacterShooting>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        //KEYBOARD INPUT MANAGEMENT
        movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        animator.SetFloat("Speed", movement.sqrMagnitude);

        //MOUSE INPUT MANAGEMENT
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        animator.SetFloat("Horizontal", lookDirection.x);
        animator.SetFloat("Vertical", lookDirection.y);

        if (Input.GetButtonDown("Fire1"))
        {
            characteShooting.Shoot(bulletOrigin, lookDirection, Quaternion.identity);
        }
    }

    private void FixedUpdate()
    {
        characterMovement.Move(movement);

        firepointPos = new Vector2(firePoint.position.x, firePoint.position.y);
        lookDirection = (mousePos - firepointPos).normalized;
        bulletOrigin = firePoint.position + (Vector3)(lookDirection * 0.5f);
    }
}
