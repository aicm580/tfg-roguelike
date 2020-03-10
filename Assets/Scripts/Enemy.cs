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

    public IntRange friends; //número de enemigos igual que éste que le acompañan

    public int health;

    public Vector2 pos = new Vector2();

    //private Animator anim;


    void Start()
    {
        //anim = GetComponent<Animator>();
    }
}
