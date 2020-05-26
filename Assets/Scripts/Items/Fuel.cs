using UnityEngine;

public class Fuel : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
            AudioManager.audioManagerInstance.PlaySFX("PickFuel");
            GameManager.instance.NextLevel();
    }
}
