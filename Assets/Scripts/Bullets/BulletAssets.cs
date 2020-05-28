using System.Collections.Generic;
using UnityEngine;

public enum BulletType
{
    Normal, 
    Poisonous, 
    ImprovedPoisonous,
    Killer,
}

public class BulletAssets : MonoBehaviour
{
    public static BulletAssets instance { get; private set; }

    public Bullet[] bullets;
    public GameObject explosion;

    private Dictionary<BulletType, Bullet> bulletDictionary = new Dictionary<BulletType, Bullet>();

    private void Awake()
    {
        instance = this;

        foreach (Bullet bullet in bullets)
        {
            bulletDictionary.Add(bullet.bulletType, bullet);
        }
    }

    public Bullet GetBulletByType(BulletType bulletType)
    {
        return bulletDictionary[bulletType];
    }
}
