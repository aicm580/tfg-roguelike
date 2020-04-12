using UnityEngine;

public class Poison : MonoBehaviour
{
    [SerializeField]
    private float poisonEffect;
    [SerializeField]
    private float poisonDuration;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<CharacterMovement>() != null)
        {
            collision.gameObject.GetComponent<CharacterMovement>().MovementPoison(poisonEffect, poisonDuration);
            Debug.Log("ENVENENADO");
        }
    }
}
