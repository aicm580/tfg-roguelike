using System.Collections;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public float defaultMoveSpeed;
    [HideInInspector]
    public float moveSpeed;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void InitializeCharacterMovement()
    {
        moveSpeed = defaultMoveSpeed;
    }

    public void Move(Vector2 movement, float multiplier)
    {
        rb.MovePosition(rb.position + movement * moveSpeed * multiplier * Time.fixedDeltaTime);
    }

    public void MovementPoison(float poisonEffect, float poisonDuration)
    {
        float pEffect = poisonEffect;
        if (moveSpeed - poisonEffect <= 0.35f)
        {
            pEffect -= Mathf.Abs(moveSpeed - poisonEffect) + 0.35f;
        }
        moveSpeed -= pEffect;
        GameManager.instance.UpdateGameStats(); //modificamos los valores de los stats en pantalla, ya que la velocidad ha cambiado
        StartCoroutine(PoisonDisappears(pEffect, poisonDuration));
    }

    IEnumerator PoisonDisappears(float poisonEffect, float poisonDuration)
    {
        yield return new WaitForSeconds(poisonDuration);
        moveSpeed += poisonEffect;
        GameManager.instance.UpdateGameStats(); //modificamos los valores de los stats en pantalla, ya que la velocidad ha cambiado
    }
}