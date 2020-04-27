using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletAssets : MonoBehaviour
{
    public static BulletAssets instance { get; private set; }

    private void Awake()
    {
        instance = this;
    }

    public Bullet standardBullet;
    public Bullet poisonousBullet;
    public Bullet killerBullet;

}
