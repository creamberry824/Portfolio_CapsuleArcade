using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Spawner : MonoBehaviour
{
    [Header("Option")]
    public GameObject[] BossPrefabs;
    public Transform BossSpawnPoint;
    public Transform[] EnemySpawnPoint;

    private void Awake()
    {
        EnemySpawnPoint = GetComponentsInChildren<Transform>();
    }
    public void EnemySpawn(int iEnemyCountMax, int iEnemyMaxHp, int iMobKind)
    {
        if (GameManager.Instance.poolManager_Enemy.EnemyCount < iEnemyCountMax)
        {
            Enemy.iMaxHp = iEnemyMaxHp;
            GameObject enemy = GameManager.Instance.poolManager_Enemy.Get(Random.Range(0 , iMobKind));
            if (enemy != null) enemy.transform.position = EnemySpawnPoint[Random.Range(1, EnemySpawnPoint.Length)].position;
        }
    }
    public GameObject BossSpawn(int bossNumber)
    {
        GameObject boss = Instantiate(BossPrefabs[bossNumber], BossSpawnPoint);
        boss.transform.parent = null;
        return boss;
    }
}
