using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Player Character", menuName = "Player Character")]
public class PlayerCharacter : ScriptableObject
{
    public string playerName;

    public int initHealth;
    public int initHearts;
    public int maxHearts;

    public float shootDelay;
    public float bulletSize;
    public BulletType bulletType;
    public ShootType shootType;

    public float moveSpeed;

    public int abilityDuration;
}
