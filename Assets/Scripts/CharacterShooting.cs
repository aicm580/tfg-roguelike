using UnityEngine;

public class CharacterShooting : MonoBehaviour
{
    [SerializeField]
    protected float shootDelay = 0.2f;
    [SerializeField]
    protected GameObject bulletPrefab;

    public void Shoot(Vector3 originPosition, Quaternion rotation)
    {
        Instantiate(bulletPrefab, originPosition, rotation);
    }
}
