using UnityEngine;

[RequireComponent(typeof(CharacterMovement))]
public class PlayerInputController : MonoBehaviour
{
    private CharacterMovement characterMovement;
    private Vector2 movement;
    private Animator animator;

    void Awake()
    {
        characterMovement = GetComponent<CharacterMovement>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        animator.SetFloat("Speed", movement.sqrMagnitude);
    }

    private void FixedUpdate()
    {
        characterMovement.Move(movement);
    }
}
