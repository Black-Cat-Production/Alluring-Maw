using System;
using System.Collections.Generic;
using Lukas.Scripts.Core;
using UnityEngine;
using Random = UnityEngine.Random;

public class RoomSpawner : MonoBehaviour
{
    [SerializeField] Enemy enemyPrefab;
    [SerializeField] int spawnAmount;
    [SerializeField] GameObject door;

    List<Enemy> spawnedEnemies;

    void Awake()
    {
        spawnedEnemies = new List<Enemy>();
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
    }

    Enemy SpawnEnemy()
    {
        var randomPos = Random.insideUnitSphere * 5;
        var instantiatePos = new Vector3(randomPos.x, 0, randomPos.z);
        instantiatePos += new Vector3(transform.position.x, 1, transform.position.z);
        var spawnedEnemy = Instantiate(enemyPrefab, instantiatePos, Quaternion.identity);
        spawnedEnemy.SetSpawner(this);
        return spawnedEnemy;
    }

    public void EnemyDied(Enemy _enemy)
    {
        spawnedEnemies.Remove(_enemy);
    }
}