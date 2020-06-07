using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CharacterMovement))]
[RequireComponent(typeof(CharacterShooting))]
public class PlayerInputController : MonoBehaviour
{
    public Transform firePoint;

    [HideInInspector]
    public float abilityDuration; //tiempo que permanece activa la habilidad especial
    [HideInInspector]
    public bool abilityActive;
    public GameObject abilityPanel;

    private float abilityTimer;
    private bool abilityAvailable = true;

    private CharacterMovement characterMovement;
    private CharacterShooting characterShooting;

    private Vector2 movement;
    private Vector2 mousePos;
    private Vector2 firepointPos;
    private Vector2 lookDirection;
    private Vector3 bulletOrigin = new Vector3();

    private Animator animator;

    void Awake()
    {
        characterMovement = GetComponent<CharacterMovement>();
        characterShooting = GetComponent<CharacterShooting>();
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (GameManager.instance.playerAlive)
        {
            //MOUSE INPUT MANAGEMENT
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            animator.SetFloat("Horizontal", lookDirection.x);
            animator.SetFloat("Vertical", lookDirection.y);

            if (Input.GetButtonDown("Fire1") && !abilityActive)
            {
                characterShooting.Shoot(bulletOrigin, lookDirection, 0.875f, Quaternion.identity, DamageOrigin.Player, GameManager.instance.playerName);
            }

            //KEYBOARD INPUT MANAGEMENT (MOVEMENT)
            movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            animator.SetFloat("Speed", movement.sqrMagnitude);

            //KEYBOARD INPUT MANAGEMETN (SPECIAL ABILITY)
            if (Input.GetKeyDown(KeyCode.Space) && !abilityActive && abilityAvailable)
            {
                abilityTimer = 0;
                abilityActive = true;
                abilityAvailable = false;
                abilityPanel.SetActive(true);
            }

            if (abilityActive && abilityTimer >= abilityDuration)
            {
                abilityActive = false;
                abilityAvailable = false;
                abilityPanel.SetActive(false);
                abilityTimer = 0;
            }
            else if(abilityActive)
            {
                abilityTimer += Time.deltaTime;
            }
        }
        else
        {
            animator.SetFloat("Speed", 0);
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

    public void InitializeCharacterAbility()
    {
        abilityTimer = 0;
        abilityActive = false;
        abilityAvailable = true;
        abilityPanel.SetActive(false);
    }
}
