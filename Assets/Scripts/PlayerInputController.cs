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
    //private float lookAtBulletTime = 0;
   // private float lookAtBulletEndTime = 0.25f;
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
            
            /*
            if (lookAtBulletTime < lookAtBulletEndTime && lookAtBullet)
            {
                lookAtBulletTime += Time.deltaTime;
                Debug.Log(lookAtBulletTime);
            }
            else
            {
                lookAtBullet = false;
               
            }*/

            animator.SetFloat("Horizontal", lookDirection.x);
            animator.SetFloat("Vertical", lookDirection.y);

            if (Input.GetButtonDown("Fire1"))
            {
                /*
                if (characterShooting.shootType == ShootType.OppositeMouse && characterShooting.canShoot)
                {
                    animator.SetFloat("Horizontal", -lookDirection.x);
                    animator.SetFloat("Vertical", -lookDirection.y);
                    lookAtBullet = true;
                    lookAtBulletTime = 0;
                }*/

                characterShooting.Shoot(bulletOrigin, lookDirection, Quaternion.identity, DamageOrigin.Player);
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
            /*
            else if (characterShooting.shootType == ShootType.Radial)
            {
                bulletsOrigins = characterShooting.GetBulletsOrigins(firePoint.position);
                bulletsDirections = characterShooting.GetRadialDirections(bulletsOrigins);
            }*/

            characterMovement.Move(movement, 1);
            /*
            if (movement.sqrMagnitude > 0)
                AudioManager.audioManagerInstance.PlaySFX("Footstep");
            */

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
