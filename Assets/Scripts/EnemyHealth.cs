using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField]
    protected int initHealth;
    [SerializeField]
    protected Transform popupDamageText;

    private int currentHealth;
    private Animator animator;

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        currentHealth = initHealth;
        Debug.Log(currentHealth);
    }

    public void TakeDamage(int dmgAmount)
    {
        PopupDamage(dmgAmount);
        currentHealth -= dmgAmount;
        Debug.Log(currentHealth);
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void PopupDamage(int dmgAmount)
    {
        Vector2 pos = new Vector2(transform.position.x, transform.position.y + 0.4f);
        Transform damagePopupTransform = Instantiate(popupDamageText, pos, Quaternion.identity);
        DamagePopup damagePopup = damagePopupTransform.GetComponent<DamagePopup>();
        damagePopup.Setup(dmgAmount);
    }

    private void Die()
    {
        animator.SetBool("dead", true);
        Destroy(gameObject);
    }
}
