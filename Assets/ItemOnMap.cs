using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemOnMap : MonoBehaviour
{
    public static void SpawnItemOnMap(Vector3 pos, Item item)
    {
        if (item != null)
        {
            Transform itemTransform = Instantiate(ItemsManager.itemsManagerInstance.baseItemPrefab, pos, Quaternion.identity);
            ItemOnMap itemOnMap = itemTransform.GetComponent<ItemOnMap>();
            itemOnMap.SetItem(item);
        }
    }

    private Item item;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetItem(Item item)
    {
        this.item = item;
        spriteRenderer.sprite = item.itemSprite;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            CharacterShooting playerShooting = collision.gameObject.GetComponent<CharacterShooting>();
            CharacterMovement playerMovement = collision.gameObject.GetComponent<CharacterMovement>();
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            switch (item.itemName)
            {
                case "Rotten Mushroom":
                    playerMovement.moveSpeed -= 0.35f;
                    playerShooting.bulletPrefab = BulletAssets.instance.poisonousBullet;
                    break;

                case "item2":
                    playerShooting.shootDelay -= 0.05f;
                    break;

                case "item3":
                    playerMovement.moveSpeed += 0.15f;
                    break;

                case "Saber Tooth":
                    playerShooting.bulletPrefab = BulletAssets.instance.killerBullet;
                    playerHealth.IncreaseCurrentHearts(2, 1); //pierde un corazón base
                    break;

                case "item5":
                    playerHealth.IncreaseCurrentHearts(1, 0); //añade 1 corazón base, vacío 
                    break;
            }

            Destroy(gameObject);
            GameManager.instance.UpdateGameStats(); //actualizamos los stats en pantalla
        }
    }
}
