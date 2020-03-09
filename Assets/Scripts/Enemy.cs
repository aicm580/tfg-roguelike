using UnityEngine;

public enum EnemyType
{
    Dilophosaurus, Meganeura,
} 

public class Enemy : MonoBehaviour
{
    public EnemyType enemyType;

    public IntRange friends; //número de enemigos igual que éste que le acompañan

    public int health;

    public Vector2 pos = new Vector2();

    //private Animator anim;


    void Start()
    {
        //anim = GetComponent<Animator>();
    }
}
