using System.Collections;
using UnityEngine;


public class CharacterShooting : MonoBehaviour
{
    public float defaultShootDelay = 0.2f;
    [HideInInspector]
    public float shootDelay;

    public Bullet defaultBullet;
    [HideInInspector]
    public Bullet bulletPrefab;

    [HideInInspector]
    public bool canShoot = true;

    public void InitializeCharacterShooting()
    {
        bulletPrefab = defaultBullet;
        shootDelay = defaultShootDelay;
    }

    public void Shoot(Vector3 originPosition, Vector2 direction, Quaternion rotation, DamageOrigin owner)
    {
        if (canShoot && !GameManager.instance.isPaused)
        {
            Bullet bullet = (Bullet)Instantiate(bulletPrefab, originPosition, rotation);
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