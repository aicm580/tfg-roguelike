using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "new Item", menuName = "Items")]
public class Item : ScriptableObject
{
    public string itemName;
    public string itemDescription;
    public int amount; //veces que un item puede ser recogido
    public Sprite itemSprite;
    [Range (1,5)]
    public int rarity; //rareza del item, del 1 al 5, siendo el 1 el item más común
}
