using System.Collections;
using UnityEngine;

public class NormalEnemyHealth : CharacterHealth
{
    [SerializeField]
    protected Transform popupDamageText;

    private SpriteRenderer spriteRend;
    public Color dmgColor = new Color(255f/255f, 177f/255f, 177f/255f);

    protected override void Start()
    {
        base.Start();
        spriteRend = GetComponentInChildren<SpriteRenderer>();
    }

    public override void TakeDamage(int dmgAmount)
    {
        spriteRend.color = dmgColor;
        StartCoroutine(ReturnToOriginalColor());

        PopupDamage(dmgAmount);
        base.TakeDamage(dmgAmount);
    }

    private IEnumerator ReturnToOriginalColor()
    {
        yield return new WaitForSeconds(0.2f);
        spriteRend.color = Color.white;
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
