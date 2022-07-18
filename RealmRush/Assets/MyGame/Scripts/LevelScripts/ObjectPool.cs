using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] private GameObject EnemyObject;
    [SerializeField] private int MaxEnemiesAllowed = 15;
    [SerializeField] private float DelayBetweenEnemies = 2f;
    [SerializeField] private float MaxDelayBetweenEnemies = 5f;
    [SerializeField] private float MinDelayBetweenEnemies = 1f;

    [SerializeField] private GameObject[] PooledEnemies;
    [SerializeField] private bool CurrentlySpawning = false;


    private void Awake()
    {
        PopulateEnemyPool();
    }

    // Update is called once per frame
    void Update()
    {
        if (!CurrentlySpawning && CheckIfPoolHasValidSpawn())
        {
            CurrentlySpawning = true;
            StartCoroutine(SpawnEnemy(DelayBetweenEnemies));
            RandomizeDelay();
        }
    }

    private void RandomizeDelay()
    {
        float randomDelay = Random.Range(MinDelayBetweenEnemies, MaxDelayBetweenEnemies);
        DelayBetweenEnemies = randomDelay;
    }

    private bool CheckIfPoolHasValidSpawn()
    {
        bool nonSpawnedFound = false;
        foreach (var enemy in PooledEnemies)
        {
            if (!enemy.activeInHierarchy)
            {
                nonSpawnedFound = true;
                break;
            }
        }

        return nonSpawnedFound;
    }

    private IEnumerator SpawnEnemy(float delayBetweenSpawns)
    {
        yield return new WaitForSeconds(delayBetweenSpawns);

        foreach (var enemy in PooledEnemies)
        {
            if (!enemy.activeInHierarchy)
            {
                enemy.SetActive(true);
                break;
            }
        }

        CurrentlySpawning = false;
    }

    private void PopulateEnemyPool()
    {
        PooledEnemies = new GameObject[MaxEnemiesAllowed];
        for (int i = 0; i < PooledEnemies.Length; i++)
        {
            PooledEnemies[i] = Instantiate(EnemyObject);
            PooledEnemies[i].SetActive(false);
        }
    }
}