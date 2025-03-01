using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class EnemySpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject[] enemyPrefabs;



    [Header("Attribute")]
    [SerializeField] private int baseEnemies = 8;
    [SerializeField] private float enemiesPerSecond = 0.5f;
    [SerializeField] private float timeBetweenWaves = 5f;
    [SerializeField] private float difficultyScalingFactor = 0.75f;

    [Header("Event")]
    public static UnityEvent onEnemyDestroy = new UnityEvent();

    private int currentWave = 1;
    private float timeSinceLastSpawn;
    private int enemiesAlive;
    private int enemiesLeftToSpawned;
    private bool isSpawning = false;

    private void Awake()
    {
        onEnemyDestroy.AddListener(EnemyDestroy);
    }
    // Start is called before the first frame update
    private void Start()
    {
        StartCoroutine(StartWave());
    }
    void Update()
    {
        if (!isSpawning)
        {
            return;
        }
        timeSinceLastSpawn += Time.deltaTime; 

        if(timeSinceLastSpawn >= (1f / enemiesPerSecond)  && enemiesLeftToSpawned > 0)
        {
            SpawnEnemy();
            enemiesLeftToSpawned--;
            enemiesAlive++;
            timeSinceLastSpawn = 0f;
        }
        if(enemiesAlive == 0 && enemiesLeftToSpawned == 0)
        {
            EndWave();
        }
    }
    private void EndWave()
    {
        isSpawning = false;
        timeSinceLastSpawn = 0;
        currentWave++;
        StartCoroutine(StartWave());
    }
    private void SpawnEnemy()
    {
        GameObject prefabToSpawn = enemyPrefabs[0];
        Instantiate(prefabToSpawn, LevelManager.main.startPoint.position, Quaternion.identity);
    }
    private void EnemyDestroy()
    {
        enemiesAlive--;
    }
    private IEnumerator StartWave()
    {
        yield return new WaitForSeconds(timeBetweenWaves);
        isSpawning = true;
        enemiesLeftToSpawned = EnemiesPerWave();
    }
    private int EnemiesPerWave()
    {
        return Mathf.RoundToInt(baseEnemies * Mathf.Pow(currentWave, difficultyScalingFactor));
    }

    // Update is called once per frame
}
