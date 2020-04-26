using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ItemsManager : MonoBehaviour
{
    public List<Item> items;
    private Dictionary<string, Item> itemDictionary = new Dictionary<string, Item>();

    private void Awake()
    {
        ClearItems();
    }

    public void LoadItems()
    {
        string lvlName = "Level" + GameManager.instance.level;

        Item[] currentLevelItems = Resources.LoadAll<Item>(lvlName + "/Items");
        foreach (Item item in currentLevelItems)
        {
            items.Add(item); //añadimos los items del nivel a la lista que contiene los items de los niveles anteriores
            itemDictionary.Add(item.itemName, item);
        }

        Item currentItem = GetItemByProbability(6, 9);
        if (currentItem != null)
            ItemOnMap.SpawnItemOnMap(Vector3.zero, currentItem);
    }

    public Item GetItemByName(string itemName)
    {
        return itemDictionary[itemName];
    }

    public Item GetItemByProbability(int minProbability, int maxProbability)
    {
        List<Item> itemsByProbability = new List<Item>();
        foreach (Item item in items)
        {
            if (item.probability >= minProbability && item.probability <= maxProbability)
            {
                itemsByProbability.Add(item);
            }
        }

        if (itemsByProbability.Count > 0)
        {
            int random = Random.Range(0, itemsByProbability.Count);
            return itemsByProbability[random];
        }
        else
        {
            return null; 
        }
    }

    public void ItemFound(Item item)
    {

    }

    public void ClearItems()
    {
        items.Clear();
        itemDictionary.Clear();
    }
}
