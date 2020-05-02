using System.Collections;
using UnityEngine;

public enum ShootType
{
    NormalMouse,
    BidirectionalMouse,
}

public class CharacterShooting : MonoBehaviour
{
    public float shootDelay;
    public float bulletSize;
    public Bullet bulletPrefab;
    public ShootType shootType;

    [HideInInspector]
    public bool canShoot = true;

    public void InitializeCharacterShooting(float delay, float size, BulletType bullet, ShootType shoot)
    {
        shootDelay = delay;
        bulletSize = size;
        bulletPrefab = BulletAssets.instance.GetBulletByType(bullet);
        shootType = shoot;
    }

    public void ChangeShootDelay(float shootDelayModifier)
    {
        shootDelay += shootDelayModifier;

        if (shootDelay <= 0.04f)
            shootDelay = 0.05f;
    }

    private void InstantiateBullet(Vector3 originPosition, Vector2 direction, Quaternion rotation, DamageOrigin owner)
    {
        Bullet bullet = Instantiate(bulletPrefab, originPosition + (Vector3)(direction * 0.65f), rotation);
        bullet.direction = direction;
        bullet.bulletOwner = owner;

        bullet.transform.localScale += new Vector3(bulletSize, bulletSize, 0);
    }

    public void Shoot(Vector3 originPosition, Vector2 direction, Quaternion rotation, DamageOrigin owner)
    {
        if (canShoot && !GameManager.instance.isPaused)
        {
            switch (shootType)
            {
                case ShootType.NormalMouse:
                    InstantiateBullet(originPosition, direction, rotation, owner);
                    break;

                case ShootType.BidirectionalMouse:
                    InstantiateBullet(originPosition, direction, rotation, owner);
                    InstantiateBullet(originPosition, -direction, rotation, owner);
                    break;
            }

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