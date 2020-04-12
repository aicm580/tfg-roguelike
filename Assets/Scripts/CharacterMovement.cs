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
        float pEffect = poisonEffect;
        if (moveSpeed - poisonEffect <= 0.35f)
        {
            pEffect -= Mathf.Abs(moveSpeed - poisonEffect) + 0.35f;
            Debug.Log("Modified poison effect: " + pEffect);
        }
        moveSpeed -= pEffect;
        StartCoroutine(PoisonDisappears(pEffect, poisonDuration));
        Debug.Log("Move speed: " + moveSpeed);
    }

    IEnumerator PoisonDisappears(float poisonEffect, float poisonDuration)
    {
        yield return new WaitForSeconds(poisonDuration);
        moveSpeed += poisonEffect;
    }
}