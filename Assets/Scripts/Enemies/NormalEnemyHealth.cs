using UnityEngine;

public class NormalEnemyHealth : CharacterHealth
{
    [SerializeField]
    protected Transform popupDamageText;

    public int minRarity;
    public int maxRarity;

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
       
        SpawnItems(0.95f, minRarity, maxRarity); //Hay un 5% de probabilidades de que, al morir, el enemigo deje un item
    }
}
