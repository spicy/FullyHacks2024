using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class LightEnemy : BaseEnemy, ISliceable
{
    [SerializeField] private Transform gunPoint;
    [SerializeField] private float bulletSpeed = 5f;
    [SerializeField] private float fireRate = 1f;
    private float nextFireTime = 1f;

    private new void Start()
    {
        base.Start();
        StartCoroutine(AttackRoutine());
    }

    IEnumerator AttackRoutine()
    {
        while (true)
        {
            yield return new WaitUntil(() => Time.time >= nextFireTime);
            Attack();
            // Calculate the next fire time by adding a base delay (1f / fireRate) and a random delay
            float randomDelay = Random.Range(0f, 0.25f);
            nextFireTime = Time.time + (1f / fireRate) + randomDelay;
        }
    }

    private new void Attack()
    {
        if (gunPoint != null && playerAwareness.IsAwareOfPlayer)
        {
            GameObject bullet = BulletPoolManager.Instance.GetBullet();
            bullet.transform.position = gunPoint.position;
            bullet.SetActive(true);

            Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
            if (bulletRb != null)
            {
                Vector2 shootingDirection = playerAwareness.DirectionToPlayer;

                // Calculate the angle from the shooting direction
                float angle = Mathf.Atan2(shootingDirection.y, shootingDirection.x) * Mathf.Rad2Deg;
                bullet.transform.rotation = Quaternion.Euler(0, 0, angle);

                // Set the bullet's velocity to move it towards the player
                bulletRb.velocity = shootingDirection * bulletSpeed;
            }
        }
    }


    public void OnBeforeSlice()
    {
        TakeDamage(9999);
    }
}
