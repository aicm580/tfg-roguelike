﻿using System.Collections;
using UnityEngine;

public enum ShootType
{
    NormalMouse,
    BidirectionalMouse,
    Radial,
}

public class CharacterShooting : MonoBehaviour
{
    public float shootDelay;
    public float bulletSize;
    public int bulletsAmount;
    public Bullet bulletPrefab;
    public ShootType shootType; 

    [HideInInspector]
    public bool canShoot = true;


    public void InitializeCharacterShooting(float delay, float size, int amount, BulletType type, ShootType shoot)
    {
        shootDelay = delay;
        bulletSize = size;
        bulletsAmount = amount;
        bulletPrefab = BulletAssets.instance.GetBulletByType(type);
        shootType = shoot;
    }

    public void SetBulletType(BulletType type)
    {
        bulletPrefab = BulletAssets.instance.GetBulletByType(type);
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

    private void InstantiateRadialBullets(int nBullets, Vector2 originPos, DamageOrigin owner)
    {
        float angleStep = 360f / nBullets;
        float angle = 0f;

        for (int i = 0; i < nBullets; i++)
        {
            float bulletPosX = originPos.x + Mathf.Sin((angle * Mathf.PI) / 180);
            float bulletPosY = originPos.y + Mathf.Cos((angle * Mathf.PI) / 180);
            Vector2 bulletPosVector = new Vector2(bulletPosX, bulletPosY);
            Vector2 bulletDirection = (bulletPosVector - originPos).normalized;

            InstantiateBullet(originPos, bulletDirection, Quaternion.identity, owner);

            angle += angleStep;
        }
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

                case ShootType.Radial:
                    InstantiateRadialBullets(bulletsAmount, originPosition, owner);
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