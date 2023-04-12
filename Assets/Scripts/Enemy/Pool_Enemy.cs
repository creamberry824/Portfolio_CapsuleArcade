using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool_Enemy : MonoBehaviour
{
    public GameObject[] enemy; //Enemy���� 
    List<GameObject>[] enemys;
    [HideInInspector] public int EnemyCount = 0; //������ ����

    private void Awake()
    {
        enemys = new List<GameObject>[enemy.Length];

        for (int index = 0; index < enemys.Length; index++)
            enemys[index] = new List<GameObject>(); //�ʱ�ȭ
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
        EnemyCount++; //������ ����
        return select;
    }
}
