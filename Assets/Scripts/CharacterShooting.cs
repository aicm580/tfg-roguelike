using System.Collections;
using UnityEngine;

public class CharacterShooting : MonoBehaviour
{
    [SerializeField]
    protected float shootDelay = 0.2f;
    [SerializeField]
    protected Bullet bulletPrefab;

    private bool canShoot = true;

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