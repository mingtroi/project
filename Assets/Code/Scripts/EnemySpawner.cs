﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemySpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform startPoint1;
    [SerializeField] private Transform startPoint2;
    [SerializeField] private bool useMultiplePaths = false;

    [Header("Wave Configurations")]
    [SerializeField] private List<WaveConfig> waves; // Danh sách các wave
    [SerializeField] private float timeBetweenWaves = 5f;

    [Header("Event")]
    public static UnityEvent onEnemyDestroy = new UnityEvent();

    private int currentWaveIndex = 0;
    private float timeSinceLastSpawn;
    private int enemiesAlive;
    private int enemiesLeftToSpawned;
    private bool isSpawning = false;
    private float eps;

    private void Awake()
    {
        onEnemyDestroy.AddListener(EnemyDestroy);
    }

    private void Start()
    {
        StartCoroutine(StartWave());
    }

    void Update()
    {
        if (!isSpawning) return;

        timeSinceLastSpawn += Time.deltaTime;

        if (timeSinceLastSpawn >= (1f / eps) && enemiesLeftToSpawned > 0)
        {
            SpawnEnemy();
            enemiesLeftToSpawned--;
            enemiesAlive++;
            timeSinceLastSpawn = 0f;
        }

        if (enemiesAlive == 0 && enemiesLeftToSpawned == 0)
        {
            EndWave();
        }
    }

    private void EndWave()
    {
        isSpawning = false;
        timeSinceLastSpawn = 0;

        if (currentWaveIndex < waves.Count - 1) // Chưa hết wave
        {
            currentWaveIndex++;
            StartCoroutine(StartWave());
        }
        else
        {
            if (LevelManager.main.playerHealth > 0)
            {
                LevelManager.main.GameWin();
            }
        }
    }

    private void SpawnEnemy()
    {
        WaveConfig currentWave = waves[currentWaveIndex];
        int index = Random.Range(0, currentWave.enemyPrefabs.Count);
        GameObject prefabToSpawn = currentWave.enemyPrefabs[index];

        Transform spawnPoint = startPoint1;
        Transform[] waypoints = Waypoints.path1;

        if (useMultiplePaths && startPoint2 != null)
        {
            bool spawnAtFirstPath = Random.value > 0.5f;
            spawnPoint = spawnAtFirstPath ? startPoint1 : startPoint2;
            waypoints = spawnAtFirstPath ? Waypoints.path1 : Waypoints.path2;
        }

        GameObject enemyGO = Instantiate(prefabToSpawn, spawnPoint.position, Quaternion.identity);

        EnemyMovement enemyMovement = enemyGO.GetComponent<EnemyMovement>();
        if (enemyMovement != null)
        {
            enemyMovement.SetPath(waypoints);
        }
    }

    private void EnemyDestroy()
    {
        enemiesAlive--;
        Debug.Log("Enemy destroyed! Enemies alive: " + enemiesAlive);
        LevelManager.main.IncreaseKillCount();
    }

    private IEnumerator StartWave()
    {
        yield return new WaitForSeconds(timeBetweenWaves);

        WaveConfig currentWave = waves[currentWaveIndex];
        isSpawning = true;
        enemiesLeftToSpawned = currentWave.enemyCount;
        eps = currentWave.enemiesPerSecond;
    }
}
