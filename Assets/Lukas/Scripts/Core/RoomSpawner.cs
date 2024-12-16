using System;
using System.Collections.Generic;
using Lukas.Scripts.Core.KI;
using UnityEngine;
using Random = UnityEngine.Random;

public class RoomSpawner : MonoBehaviour
{
    [SerializeField] EnemyAIModule enemyAIModulePrefab;
    [SerializeField] int spawnAmount;
    [SerializeField] GameObject door;

    List<EnemyAIModule> spawnedEnemies;

    void Awake()
    {
        spawnedEnemies = new List<EnemyAIModule>();
        for (int i = 0; i < spawnAmount; i++)
        {
            spawnedEnemies.Add(SpawnEnemy());
        }
    }

    void FixedUpdate()
    {
        if (spawnedEnemies.Count != 0) return;
        Destroy(door.gameObject);
        Debug.Log("Room Cleared");
        //I dont know if this is good, will see, depending on SpawnerFunctions
        Destroy(gameObject);
    }

    EnemyAIModule SpawnEnemy()
    {
        var randomPos = Random.insideUnitSphere * 5;
        var instantiatePos = new Vector3(randomPos.x, 0, randomPos.z);
        instantiatePos += new Vector3(transform.position.x, 1, transform.position.z);
        var spawnedEnemy = Instantiate(enemyAIModulePrefab, instantiatePos, Quaternion.identity);
        spawnedEnemy.SetSpawner(this);
        return spawnedEnemy;
    }

    public void EnemyDied(EnemyAIModule _enemyAIModule)
    {
        spawnedEnemies.Remove(_enemyAIModule);
    }
}