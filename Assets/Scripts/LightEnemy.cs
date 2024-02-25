using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightEnemy : BaseEnemy, ISliceable
{
    private float timer;
    // Different Attack?
    private EnemyShooting shooter;
    // Different Range?
    public void OnBeforeSlice()
    {
        TakeDamage(9999);
    }

    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform gunPoint;
    [SerializeField] private float bulletSpeed = 5f;
    [SerializeField] private float fireRate = 1f;
    private float nextFireTime = 0f;


    // public new void Attack()
    // {
    //     if (bulletPrefab != null && gunPoint != null)
    //     {
    //         GameObject bullet = Instantiate(bulletPrefab, gunPoint.position, Quaternion.identity);
    //         Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
    //         if (bulletRb != null)
    //         {
    //             Vector2 shootingDirection = playerAwareness.DirectionToPlayer.normalized;
    //             bulletRb.velocity = shootingDirection * bulletSpeed;
    //         }
    //     }
    // }

}
