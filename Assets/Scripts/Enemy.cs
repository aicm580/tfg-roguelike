using UnityEngine;

public enum EnemyType
{
    Dilophosaurus, Meganeura,
} 

public enum Biome
{
    Land, Water, 
}

public class Enemy : MonoBehaviour
{
    public EnemyType enemyType;
    public Biome biome; 

    public IntRange friends; //rango de número de enemigos igual que éste que le acompañan
    public int calculatedFriends;

    public int health;

    //private Animator anim;


    void Start()
    {
        //anim = GetComponent<Animator>();
    }
}
