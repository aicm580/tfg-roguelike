using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ItemsManager : MonoBehaviour
{
    public static ItemsManager itemsManagerInstance;

    [HideInInspector]
    public GameObject itemsHolder;

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

    public void SetupItems()
    {
        itemsHolder = GameObject.Find("ItemsHolder");
        if (itemsHolder != null)
        {
            foreach (Transform child in itemsHolder.transform)
            {
                Destroy(child.gameObject);
            }
        }
        else
        {
            itemsHolder = new GameObject("ItemsHolder");
        }

        LoadItems();
    }

    private void LoadItems()
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

    public Item GetItemByRarity(int minRarity, int maxRarity)
    {
        int rarity = GetRarity(minRarity, maxRarity);

        List<Item> availableItems = new List<Item>();
        foreach (Item item in items)
        {
            if (item.rarity == rarity && item.amount > 0)
            {
                availableItems.Add(item);
            }
        }

        if (availableItems.Count != 0)
        {
            int r = Random.Range(0, availableItems.Count);
            availableItems[r].amount--;
            return availableItems[r];
        }
        else 
        {
            return null;
        }
    }

    private int GetRarity(int minRarity, int maxRarity)
    {
        float minRandom = 0;
        float maxRandom = 1f;

        switch (minRarity)
        {
            case 1:
                maxRandom = 1f;
                break;
            case 2:
                maxRandom = 0.549f;
                break;
            case 3:
                maxRandom = 0.249f;
                break;
            case 4:
                maxRandom = 0.099f;
                break;
            case 5:
                maxRandom = 0.029f;
                break;
        }

        switch (maxRarity)
        {
            case 1:
                minRandom = 0.55f;
                break;
            case 2:
                minRandom = 0.25f;
                break;
            case 3:
                minRandom = 0.1f;
                break;
            case 4:
                minRandom = 0.03f;
                break;
            case 5:
                minRandom = 0;
                break;
        }
        
        int rarity = 1;

        if (minRarity != maxRarity) //si son diferentes, calculamos la rareza resultante
        {
            float random = Random.Range(minRandom, maxRandom);
            
            if (random < 0.03f) //0.03 - 3%
                rarity = 5;
            else if (random < 0.1f) //0.07 - 7%
                rarity = 4;
            else if (random < 0.25f) //0.15 - 15%
                rarity = 3;
            else if (random < 0.55f) //0.3 - 30%
                rarity = 2;
            else if (random >= 0.55f) //0.45 - 45%
                rarity = 1;

            Debug.Log("Rarity: " + rarity);
        }
        else
        {
            rarity = minRarity;
        }

        return rarity;
    }

    public void ClearItems()
    {
        items.Clear();
        itemDictionary.Clear();
    }
}
