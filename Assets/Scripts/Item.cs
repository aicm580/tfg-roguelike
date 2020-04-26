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
    [Range (0,9)]
    public int probability; //probabilidad, del 0 al 9, de que aparezca el item

    public virtual void UseItem() { }
}
