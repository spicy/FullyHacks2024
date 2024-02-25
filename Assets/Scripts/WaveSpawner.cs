using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private List<Enemy> enemies;
    private List<GameObject> enemiesToSpawn = new List<GameObject>();
    private int waveValue;

    public WaveGenerator(List<Enemy> enemies)
    {
        this.enemies = enemies;
    }

    public void GenerateWave(int currentWave)
    {
        waveValue = currentWave * 10;
        GenerateEnemies();
    }

    private void GenerateEnemies()
    {
        List<GameObject> generatedEnemies = new List<GameObject>();
        while (waveValue > 0 || generatedEnemies.Count < 50)
        {
            int randEnemyId = Random.Range(0, enemies.Count);
            Enemy enemy = enemies[randEnemyId];
            if (waveValue - enemy.cost >= 0)
            {
                generatedEnemies.Add(enemy.enemyPrefab);
                waveValue -= enemy.cost;
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
    public List<GameObject> spawnedEnemies = new List<GameObject>();

    public void SpawnEnemy(GameObject enemyPrefab, Transform spawnLocation)
    {
        GameObject enemy = GameObject.Instantiate(enemyPrefab, spawnLocation.position, Quaternion.identity);
        spawnedEnemies.Add(enemy);
    }
}


public class WaveSpawner : MonoBehaviour
{
    public List<Enemy> enemies = new List<Enemy>();
    public Transform[] spawnLocations;
    public int currentWave;
    private IWaveGenerator waveGenerator;
    private IEnemySpawner enemySpawner;
    public float spawnInterval;
    public float waveTimer;
    public float spawnTimer;

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

        if (waveGenerator.GetEnemiesToSpawn().Count == 0 && ((EnemySpawner)enemySpawner).spawnedEnemies.Count == 0)
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
    public GameObject enemyPrefab;
    public int cost;
}