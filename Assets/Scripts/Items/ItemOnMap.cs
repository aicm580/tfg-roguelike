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
            itemTransform.parent = ItemsManager.itemsManagerInstance.itemsHolder.transform;
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
                case "Dilo's Heart":
                    playerHealth.IncreaseCurrentHearts(1, 0); //añade 1 corazón base, vacío 
                    playerShooting.shootDelay += 0.06f;
                    break;

                case "Heart":
                    playerHealth.Heal(1);
                    break;

                case "Meganeura's Wing":
                    playerMovement.moveSpeed += 0.15f;
                    break;

                case "Rotten Mushroom":
                    playerMovement.moveSpeed -= 0.2f;
                    playerShooting.bulletPrefab = BulletAssets.instance.poisonousBullet;
                    break;
                    
                case "Saber Tooth":
                    playerShooting.bulletPrefab = BulletAssets.instance.killerBullet;
                    playerHealth.DecreaseCurrentHearts(1); //pierde un corazón base
                    break;
                    
                case "Wollemia's Root":
                    playerShooting.shootDelay -= 0.06f;
                    break;
            }

            Destroy(gameObject);
            GameManager.instance.UpdateGameStats(); //actualizamos los stats en pantalla
        }
    }
}
