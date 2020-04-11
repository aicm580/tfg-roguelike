using System.Collections;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public float moveSpeed;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Move(Vector2 movement)
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    public void MovementPoison(float poisonEffect, float poisonDuration)
    {
        moveSpeed -= poisonEffect;
        StartCoroutine(PoisonDisappears(poisonEffect, poisonDuration));
        Debug.Log(moveSpeed);
    }

    IEnumerator PoisonDisappears(float poisonEffect, float poisonDuration)
    {
        yield return new WaitForSeconds(poisonDuration);
        moveSpeed += poisonEffect;
    }
}