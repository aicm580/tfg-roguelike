using System.Collections;
using UnityEngine;

public enum ShootType
{
    NormalMouse,
    OppositeMouse,
    BidirectionalMouse,
}

public class CharacterShooting : MonoBehaviour
{
    public float defaultShootDelay = 0.2f;
    [HideInInspector]
    public float shootDelay;

    public Bullet defaultBullet;
    [HideInInspector]
    public Bullet bulletPrefab;

    public ShootType shootType;

    [HideInInspector]
    public bool canShoot = true;

    public void InitializeCharacterShooting()
    {
        bulletPrefab = defaultBullet;
        shootDelay = defaultShootDelay;
    }

    public void ChangeShootDelay(float shootDelayModifier)
    {
        shootDelay += shootDelayModifier;

        if (shootDelay <= 0.04f)
            shootDelay = 0.05f;
    }

    public void Shoot(Vector3 originPosition, Vector2 direction, Quaternion rotation, DamageOrigin owner)
    {
        if (shootType == ShootType.OppositeMouse)
            direction *= -1;
        if (canShoot && !GameManager.instance.isPaused)
        {
            Bullet bullet = Instantiate(bulletPrefab, originPosition + (Vector3)(direction * 0.65f), rotation);
            bullet.direction = direction;
            bullet.bulletOwner = owner;

            canShoot = false;
            StartCoroutine(ShootDelay());
        }
    }

    IEnumerator ShootDelay()
    {
        yield return new WaitForSeconds(shootDelay);
        canShoot = true;
    }
}