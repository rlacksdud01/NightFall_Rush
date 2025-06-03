using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSwarm : MonoBehaviour
{
    public Transform[] spawnPoints;

    public float timer;

    void Awake()
    {
        spawnPoints = GetComponentsInChildren<Transform>();
    }

    void Update()
    {
        if (!GameManager.instance.isLive) return;

        timer += Time.deltaTime;

        if (GameManager.instance.gameTime < 539f)
        {
            if (timer > 40) //현재 몬스터 스폰 주기
            {
                timer = 0;
                Spawn();
            }
        }
    }

    void Spawn()
    {
        if (GameManager.instance.gameTime < 216)
        {
            GameObject enemy = GameManager.instance.pool.Get(6);
            enemy.transform.position = spawnPoints[Random.Range(1, spawnPoints.Length)].position;
        }

        else if (GameManager.instance.gameTime < 324)
        {
            GameObject enemy = GameManager.instance.pool.Get(7);
            enemy.transform.position = spawnPoints[Random.Range(1, spawnPoints.Length)].position;
        }

        else if (GameManager.instance.gameTime < 432)
        {
            GameObject enemy = GameManager.instance.pool.Get(8);
            enemy.transform.position = spawnPoints[Random.Range(1, spawnPoints.Length)].position;
        }

        else if (GameManager.instance.gameTime < 539)
        {
            GameObject enemy = GameManager.instance.pool.Get(9);
            enemy.transform.position = spawnPoints[Random.Range(1, spawnPoints.Length)].position;
        }
    }
}
