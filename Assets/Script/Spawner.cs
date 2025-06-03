using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Transform[] spawnPoints;

    int level;
    public float timer;

    void Awake()
    {
        spawnPoints = GetComponentsInChildren<Transform>();
    }

    void Update()
    {
        if (!GameManager.instance.isLive) return;

        timer += Time.deltaTime;
        level = Mathf.FloorToInt(GameManager.instance.gameTime / 108f);  //���� ���� ���� �ð�

        if (GameManager.instance.gameTime < 539f)
        {
            if (timer > 0.35f) //���� ���� ���� �ֱ�
            {
                timer = 0;
                Spawn();
            }
        }

        else if (GameManager.instance.gameTime > 540f && GameManager.instance.gameTime < 541f)
        {
            if (!GameObject.FindWithTag("Boss"))
            {
                Spawn();
            }
        }
    }

    void Spawn()
    {
        GameObject enemy = GameManager.instance.pool.Get(level);
        enemy.transform.position = spawnPoints[Random.Range(1, spawnPoints.Length)].position;
    }
}