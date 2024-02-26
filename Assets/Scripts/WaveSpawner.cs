using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public interface IWaveGenerator
{
    void GenerateWave(int currentWave);
    List<GameObject> GetEnemiesToSpawn();
    void SetMonoBehaviour(MonoBehaviour monoBehaviour);
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
    private float minSpawnDelay = 0.5f;
    private float maxSpawnDelay = 2f;
    private MonoBehaviour monoBehaviourRef;

    public WaveGenerator(List<Enemy> enemies)
    {
        this.enemies = enemies;
    }

    public void GenerateWave(int currentWave)
    {
        waveValue = currentWave * WaveMultiplier;
        if (monoBehaviourRef != null)
        {
            monoBehaviourRef.StartCoroutine(GenerateEnemiesOverTime());
        }
    }

    private IEnumerator GenerateEnemiesOverTime()
    {
        while (waveValue > 0 && enemiesToSpawn.Count < MaxEnemiesPerWave)
        {
            int randEnemyId = Random.Range(0, enemies.Count);
            Enemy enemy = enemies[randEnemyId];
            if (waveValue - enemy.Cost >= 0)
            {
                enemiesToSpawn.Add(enemy.EnemyPrefab);
                waveValue -= enemy.Cost;

                float waitTime = Random.Range(minSpawnDelay, maxSpawnDelay);
                yield return new WaitForSeconds(waitTime);
            }
            else
            {
                break;
            }
        }
        enemiesToSpawn.Clear();
    }

    public List<GameObject> GetEnemiesToSpawn()
    {
        return enemiesToSpawn;
    }

    public void SetMonoBehaviour(MonoBehaviour monoBehaviour)
    {
        monoBehaviourRef = monoBehaviour;
    }
}

public class EnemySpawner : IEnemySpawner
{
    public List<GameObject> SpawnedEnemies { get; private set; } = new List<GameObject>();

    public void SpawnEnemy(GameObject enemyPrefab, Transform spawnLocation)
    {
        GameObject enemy = GameObject.Instantiate(enemyPrefab, spawnLocation.position, Quaternion.identity);
        SpawnedEnemies.Add(enemy);
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
        waveGenerator.SetMonoBehaviour(this); // Pass this MonoBehaviour to the waveGenerator
        enemySpawner = new EnemySpawner();
        NextWave();
    }

    void Update()
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
            spawnTimer -= Time.deltaTime;
        }

        if (waveGenerator.GetEnemiesToSpawn().Count == 0 && ((EnemySpawner)enemySpawner).SpawnedEnemies.Where(enemy => enemy != null).Count() == 0)
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
