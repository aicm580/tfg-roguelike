using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ItemsManager : MonoBehaviour
{
    public static ItemsManager itemsManagerInstance;

    public Transform baseItemPrefab;
    public List<Item> items;
    private Dictionary<string, Item> itemDictionary = new Dictionary<string, Item>();

    private void Awake()
    {
        //Nos aseguramos de que solo haya 1 GameManager
        if (itemsManagerInstance == null)
        {
            itemsManagerInstance = this;
        }
        else if (itemsManagerInstance != this)
        {
            Destroy(gameObject);
        }

        ClearItems();
    }

    public void LoadItems()
    {
        string lvlName = "Level" + GameManager.instance.level;

        Item[] currentLevelItems = Resources.LoadAll<Item>(lvlName + "/Items");
        foreach (Item item in currentLevelItems)
        {
            Item currentItem = Instantiate(item);
            items.Add(currentItem); //añadimos los items del nivel a la lista que contiene los items de los niveles anteriores
            itemDictionary.Add(item.itemName, item);
        }
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
            if (item.probability >= minProbability && item.probability <= maxProbability && item.amount > 0)
            {
                itemsByProbability.Add(item);
            }
        }

        if (itemsByProbability.Count > 0)
        {
            int random = Random.Range(0, itemsByProbability.Count);
            itemsByProbability[random].amount--;
            return itemsByProbability[random];
        }
        else
        {
            return null; 
        }
    }

    public void ClearItems()
    {
        items.Clear();
        itemDictionary.Clear();
    }
}
