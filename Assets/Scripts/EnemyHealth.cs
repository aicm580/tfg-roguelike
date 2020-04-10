using UnityEngine;

public class EnemyHealth : CharacterHealth
{
    [SerializeField]
    protected Transform popupDamageText;

    public override void TakeDamage(int dmgAmount, DamageOrigin dmgOrigin)
    {
        PopupDamage(dmgAmount);
        base.TakeDamage(dmgAmount, dmgOrigin);
    }

    private void PopupDamage(int dmgAmount)
    {
        Vector2 pos = new Vector2(transform.position.x, transform.position.y + 0.4f);
        Transform damagePopupTransform = Instantiate(popupDamageText, pos, Quaternion.identity);
        DamagePopup damagePopup = damagePopupTransform.GetComponent<DamagePopup>();
        damagePopup.Setup(dmgAmount);
    }

    protected override void Die(DamageOrigin dmgOrigin)
    {
        base.Die(dmgOrigin);
        Destroy(gameObject);
    }
}
