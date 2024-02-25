using System.Collections.Generic;
using UnityEngine;

// Interfaces are unchanged as they represent clear contracts.
public interface IWaveGenerator
{
    void GenerateWave(int currentWave);
    List<GameObject> GetEnemiesToSpawn();
}

public interface IEnemySpawner
{
    void SpawnEnemy(GameObject enemyPrefab, Transform spawnLocation);
}

public class WaveGenerator : IWaveGenerator
{
    private const int WaveMultiplier = 10;
    private const int MaxEnemiesPerWave = 5;
    private List<Enemy> enemies;
    private List<GameObject> enemiesToSpawn = new List<GameObject>();
    private int waveValue;

    public WaveGenerator(List<Enemy> enemies)
    {
        this.enemies = enemies;
    }

    public void GenerateWave(int currentWave)
    {
        waveValue = currentWave * WaveMultiplier;
        GenerateEnemies();
    }

    private void GenerateEnemies()
    {
        List<GameObject> generatedEnemies = new List<GameObject>();
        while (waveValue > 0 && generatedEnemies.Count < MaxEnemiesPerWave)
        {
            int randEnemyId = Random.Range(0, enemies.Count);
            Enemy enemy = enemies[randEnemyId];
            if (waveValue - enemy.Cost >= 0)
            {
                generatedEnemies.Add(enemy.EnemyPrefab);
                waveValue -= enemy.Cost;
            }
            else
            {
                break;
            }
        }
        enemiesToSpawn = generatedEnemies;
    }

    public List<GameObject> GetEnemiesToSpawn()
    {
        return enemiesToSpawn;
    }
}

public class EnemySpawner : IEnemySpawner
{
    public List<GameObject> SpawnedEnemies { get; private set; } = new List<GameObject>();

    public void SpawnEnemy(GameObject enemyPrefab, Transform spawnLocation)
    {
        GameObject enemy = EnemyPoolManager.Instance.SpawnFromPool(enemyPrefab, spawnLocation.position, Quaternion.identity);
        SpawnedEnemies.Add(enemy);
    }

    public void ReturnEnemyToPool(GameObject enemyPrefab, GameObject enemy)
    {
        SpawnedEnemies.Remove(enemy);
        EnemyPoolManager.Instance.ReturnToPool(enemyPrefab, enemy);
    }
}

public class WaveSpawner : MonoBehaviour
{
    [SerializeField] private List<Enemy> enemies = new List<Enemy>();
    [SerializeField] private Transform[] spawnLocations;
    private int currentWave;
    private IWaveGenerator waveGenerator;
    private IEnemySpawner enemySpawner;
    [SerializeField] private float waveTimer;
    private float spawnInterval;
    private float spawnTimer;

    void Start()
    {
        waveGenerator = new WaveGenerator(enemies);
        enemySpawner = new EnemySpawner();
        NextWave();
    }

    void FixedUpdate()
    {
        if (spawnTimer <= 0 && waveGenerator.GetEnemiesToSpawn().Count > 0)
        {
            Transform spawnLocation = spawnLocations[Random.Range(0, spawnLocations.Length)];
            GameObject enemyToSpawn = waveGenerator.GetEnemiesToSpawn()[0];
            waveGenerator.GetEnemiesToSpawn().RemoveAt(0);
            enemySpawner.SpawnEnemy(enemyToSpawn, spawnLocation);
            spawnTimer = spawnInterval;
        }
        else
        {
            spawnTimer -= Time.fixedDeltaTime;
        }

        if (waveGenerator.GetEnemiesToSpawn().Count == 0 && ((EnemySpawner)enemySpawner).SpawnedEnemies.Count == 0)
        {
            NextWave();
        }
    }

    private void NextWave()
    {
        currentWave++;
        waveGenerator.GenerateWave(currentWave);
        CalculateSpawnInterval();
    }

    private void CalculateSpawnInterval()
    {
        if (waveGenerator.GetEnemiesToSpawn().Count > 0)
        {
            spawnInterval = waveTimer / waveGenerator.GetEnemiesToSpawn().Count;
        }
    }
}

[System.Serializable]
public class Enemy
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private int cost;

    public GameObject EnemyPrefab => enemyPrefab;
    public int Cost => cost;
}
