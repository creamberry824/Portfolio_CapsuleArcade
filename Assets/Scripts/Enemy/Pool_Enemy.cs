using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool_Enemy : MonoBehaviour
{
    public GameObject[] enemy; //Enemy종류 
    List<GameObject>[] enemys;
    [HideInInspector] public int EnemyCount = 0; //밀집도 측정

    private void Awake()
    {
        enemys = new List<GameObject>[enemy.Length];

        for (int index = 0; index < enemys.Length; index++)
            enemys[index] = new List<GameObject>(); //초기화
    }

    public GameObject Get(int index)
    {
        GameObject select = null;
        foreach (GameObject item in enemys[index])
            if (!item.activeSelf)
            {
                select = item;
                select.SetActive(true);
                break;
            }

        if (select == null)
        {
            select = Instantiate(enemy[index], transform);
            enemys[index].Add(select);
        }
        EnemyCount++; //밀집도 증가
        return select;
    }
}
