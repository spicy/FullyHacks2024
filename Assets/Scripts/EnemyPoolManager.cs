using System.Collections.Generic;
using UnityEngine;

public class EnemyPoolManager : MonoBehaviour
{
    public static EnemyPoolManager Instance;
    private Dictionary<GameObject, Queue<GameObject>> poolDictionary = new Dictionary<GameObject, Queue<GameObject>>();
    private const int prepopulateCount = 100; // Num of enemies to prepopulate on demand

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public GameObject SpawnFromPool(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(prefab))
        {
            poolDictionary.Add(prefab, new Queue<GameObject>());
            AddEnemiesToPool(prefab, prepopulateCount); // Prepopulate on demand
        }

        if (poolDictionary[prefab].Count == 0)
        {
            AddEnemiesToPool(prefab, prepopulateCount);
        }

        GameObject enemyToSpawn = poolDictionary[prefab].Dequeue();
        enemyToSpawn.transform.position = position;
        enemyToSpawn.transform.rotation = rotation;
        enemyToSpawn.SetActive(true);

        return enemyToSpawn;
    }

    public void ReturnToPool(GameObject prefab, GameObject instance)
    {
        instance.SetActive(false);
        if (!poolDictionary.ContainsKey(prefab))
        {
            Debug.LogWarning("Pool for prefab " + prefab.name + " doesn't exist.");
            return;
        }

        poolDictionary[prefab].Enqueue(instance);
    }

    private void AddEnemiesToPool(GameObject prefab, int count)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject newEnemy = Instantiate(prefab);
            newEnemy.SetActive(false);
            poolDictionary[prefab].Enqueue(newEnemy);
        }
    }
}
