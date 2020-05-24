using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CharacterMovement))]
[RequireComponent(typeof(CharacterShooting))]
public class PlayerInputController : MonoBehaviour
{
    public Transform firePoint;
    public float abilityDuration = 3f; //tiempo que permanece activa la habilidad especial

    private CharacterMovement characterMovement;
    private CharacterShooting characterShooting;

    private Vector2 movement;
    private Vector2 mousePos;
    private Vector2 firepointPos;
    private Vector2 lookDirection;
    private Vector3 bulletOrigin = new Vector3();
    private Vector3[] bulletsOrigins;
    private Vector3[] bulletsDirections;
    private bool abilityActive;
    private Animator animator;

    void Awake()
    {
        characterMovement = GetComponent<CharacterMovement>();
        characterShooting = GetComponent<CharacterShooting>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (GameManager.instance.playerAlive)
        {
            //MOUSE INPUT MANAGEMENT
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            animator.SetFloat("Horizontal", lookDirection.x);
            animator.SetFloat("Vertical", lookDirection.y);

            if (Input.GetButtonDown("Fire1"))
            {
                characterShooting.Shoot(bulletOrigin, lookDirection, 0.875f, Quaternion.identity, DamageOrigin.Player, GameManager.instance.playerName);
            }

            //KEYBOARD INPUT MANAGEMENT (MOVEMENT)
            movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            animator.SetFloat("Speed", movement.sqrMagnitude);

            //KEYBOARD INPUT MANAGEMETN (SPECIAL ABILITY)
            if (Input.GetKeyDown(KeyCode.Space) && !abilityActive)
            {
                abilityActive = true;
                StartCoroutine(DisableAbility());
            }
        }
    }

    private void FixedUpdate()
    {
        if (GameManager.instance.playerAlive)
        {
            bulletOrigin = firePoint.position;

            characterMovement.Move(movement, 1);

            firepointPos = new Vector2(firePoint.position.x, firePoint.position.y);
            lookDirection = (mousePos - firepointPos).normalized;
        }   
    }

    IEnumerator DisableAbility()
    {
        yield return new WaitForSeconds(abilityDuration);
        abilityActive = false;
    }
}
