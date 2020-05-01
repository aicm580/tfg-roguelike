﻿using System.Collections;
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
    private int collisions = 0;
    private BreakableGenerator breakableGenerator;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        breakableGenerator = FindObjectOfType<BreakableGenerator>();
    }

    public void SetItem(Item item)
    {
        this.item = item;
        spriteRenderer.sprite = item.itemSprite;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collisions != 0)
            Debug.Log("YA HA COLISIONADO ");
        if (collision.gameObject.tag == "Player" && collisions == 0)
        {
            CharacterShooting playerShooting = collision.gameObject.GetComponent<CharacterShooting>();
            CharacterMovement playerMovement = collision.gameObject.GetComponent<CharacterMovement>();
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            switch (item.itemName)
            {
                case "Crystal Clear Drop":
                    

                case "Rufo's Broken Mirror":
                    playerShooting.shootType = ShootType.OppositeMouse;
                    playerShooting.ChangeShootDelay(-0.075f);
                    break;

                case "Dilo's Heart":
                    playerHealth.IncreaseCurrentHearts(1, 0); //añade 1 corazón base, vacío 
                    playerShooting.ChangeShootDelay(0.04f);
                    break;

                case "Four Leaf Clover":
                    breakableGenerator.breakableMinRandom -= 0.1f; //aumenta en un 10% la probabilidad de que aparezcan objetos rompibles en próximos niveles
                    break;

                case "Heart":
                    playerHealth.Heal(1);
                    break;

                case "Meganeura's Wing":
                    playerMovement.ChangeMoveSpeed(0.15f);
                    break;

                case "Rotten Mushroom":
                    playerMovement.ChangeMoveSpeed(-0.2f);
                    playerShooting.bulletPrefab = BulletAssets.instance.poisonousBullet;
                    break;
                    
                case "Saber Tooth":
                    playerShooting.bulletPrefab = BulletAssets.instance.killerBullet;
                    playerHealth.DecreaseCurrentHearts(1); //pierde un corazón base
                    break;
                    
                case "Wollemia's Root":
                    playerShooting.ChangeShootDelay(-0.06f);
                    break;
            }
            Debug.Log("Item: " + item.itemName);
            Destroy(gameObject);
            GameManager.instance.UpdateGameStats(); //actualizamos los stats en pantalla
            collisions++;
        }
    }
}
