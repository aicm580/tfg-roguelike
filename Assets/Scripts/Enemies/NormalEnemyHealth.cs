using UnityEngine;

public class NormalEnemyHealth : CharacterHealth
{
    [SerializeField]
    protected Transform popupDamageText;
    
    protected override void Start()
    {
        base.Start();
        charName = GetComponent<Enemy>().enemyName;
    }

    public override void TakeDamage(int dmgAmount)
    {
        PopupDamage(dmgAmount);
        base.TakeDamage(dmgAmount);
    }
    
    private void PopupDamage(int dmgAmount)
    {
        Vector2 pos = new Vector2(transform.position.x, transform.position.y + 0.4f);
        Transform damagePopupTransform = Instantiate(popupDamageText, pos, Quaternion.identity);
        DamagePopup damagePopup = damagePopupTransform.GetComponent<DamagePopup>();
        damagePopup.Setup(dmgAmount);
    }

    protected override void Die()
    {
        base.Die();
        GameManager.instance.normalEnemiesKilled++;
        SpawnItems(1f - probability, minRarity, maxRarity); 
    }
}
