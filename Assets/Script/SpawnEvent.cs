using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEvent : MonoBehaviour
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

        level = Mathf.FloorToInt(GameManager.instance.gameTime / 108f);  //다음 몬스터 스폰 시간

        if (GameManager.instance.gameTime < 539f)
        {
            if (timer > 120) //현재 몬스터 스폰 주기
            {
                timer = 0;
                Spawn();
            }
        }
    }

    void Spawn()
    {
        GameObject enemy = GameManager.instance.pool.Get(level);
        enemy.transform.position = spawnPoints[1].position;
    }
}
