using System.Collections;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public float moveSpeed; //las modificaciones de velocidad ingame se aplican sobre este parámetro

    private Rigidbody2D rb;
    private Animator animator;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
    }

    public void InitializeCharacterMovement(float speed)
    {
        moveSpeed = speed;
    }

    public void Move(Vector2 movement, float multiplier)
    {
        rb.MovePosition(rb.position + movement * moveSpeed * multiplier * Time.fixedDeltaTime);
    }

    public void ChangeMoveSpeed(float moveSpeedModifier)
    {
        moveSpeed += moveSpeedModifier;

        if (moveSpeed > 9)
            moveSpeed = 9;
        else if (moveSpeed < 1)
            moveSpeed = 1;
    }

    public void MovementPoison(float poisonEffect, float poisonDuration)
    {
        float pEffect = poisonEffect;
        if (moveSpeed - poisonEffect <= 0.35f)
        {
            pEffect -= Mathf.Abs(moveSpeed - poisonEffect) + 0.35f;
        }
        moveSpeed -= pEffect;
        animator.SetBool("poisoned", true);
        GameManager.instance.UpdateGameStats(); //modificamos los valores de los stats en pantalla, ya que la velocidad ha cambiado
        StartCoroutine(PoisonDisappears(pEffect, poisonDuration));
    }

    IEnumerator PoisonDisappears(float poisonEffect, float poisonDuration)
    {
        yield return new WaitForSeconds(poisonDuration);
        animator.SetBool("poisoned", false);
        if (GameManager.instance.enemiesActive)
        {
            moveSpeed += poisonEffect;
            GameManager.instance.UpdateGameStats(); //modificamos los valores de los stats en pantalla, ya que la velocidad ha cambiado
        }
    }
}