using System.Collections;
using UnityEngine;

public class Fuel : MonoBehaviour
{
    private SpriteRenderer spriteRend;

    private void Awake()
    {
        spriteRend = GetComponentInChildren<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            AudioManager.audioManagerInstance.PlaySFX("PickFuel");
            StartCoroutine(StartNextLevel());
            spriteRend.color = new Color(1f, 1f, 1f, 0);
        }    
    }

    private IEnumerator StartNextLevel()
    {
        yield return new WaitForSeconds(0.8f);
        GameManager.instance.NextLevel();
    }
}
