using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemOnMap : MonoBehaviour
{
    public static ItemOnMap SpawnItemOnMap(Vector3 pos, Item item)
    {
        Transform itemTransform = Instantiate(GameManager.instance.baseItemPrefab, pos, Quaternion.identity);
        ItemOnMap itemOnMap = itemTransform.GetComponent<ItemOnMap>();
        itemOnMap.SetItem(item);

        return itemOnMap;
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
}
