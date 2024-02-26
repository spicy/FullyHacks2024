using System.Collections.Generic;
using UnityEngine;

public class BulletPoolManager : MonoBehaviour
{
    public static BulletPoolManager Instance { get; private set; }

    [SerializeField] private GameObject bulletPrefab;
    private List<GameObject> bulletPool = new List<GameObject>();
    private int poolStartSize = 50;
    private int poolMaxSize = 100; // Maximum size of the pool

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeBulletPool();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void InitializeBulletPool()
    {
        for (int i = 0; i < poolStartSize; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab);
            bullet.SetActive(false);
            bulletPool.Add(bullet);
        }
    }

    public GameObject GetBullet()
    {
        foreach (var bullet in bulletPool)
        {
            if (!bullet.activeInHierarchy)
            {
                return bullet;
            }
        }

        if (bulletPool.Count < poolMaxSize)
        {
            GameObject newBullet = Instantiate(bulletPrefab);
            newBullet.SetActive(false);
            bulletPool.Add(newBullet);
            return newBullet;
        }
        else
        {
            // Reuse the oldest bullet in the pool.
            GameObject oldestBullet = null;
            foreach (var bullet in bulletPool)
            {
                if (bullet.activeInHierarchy)
                {
                    oldestBullet = bullet;
                    break;
                }
            }

            if (oldestBullet != null)
            {
                // Deactivate and then reactivate the oldest bullet to reuse it.
                oldestBullet.SetActive(false);
                oldestBullet.SetActive(true);
                return oldestBullet;
            }
            else
            {
                // This case should not happen, but it's here as a fallback.
                return null;
            }
        }
    }
}
