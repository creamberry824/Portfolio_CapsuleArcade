using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool_Obstacle : MonoBehaviour
{
    public GameObject obstacle; //����ü
    List<GameObject> obstacles =  new List<GameObject>(); //����ü Ǯ

    float fObstacleSpawnTimer = 0;
    float fStopSpawnTimer = 0; //����ü ���� �ֱ�
    float fTurnSpeed = 180; //����ü ���ǵ�
    [HideInInspector] public float fStopSpawnTime = 5; //����ü ���� �ð� ����
    [HideInInspector] public bool isSpawn = true;

    private void OnEnable()
    {
        transform.parent = null;
    }
    private void Update()
    {
        transform.Rotate(new Vector3(0, fTurnSpeed * Time.deltaTime, 0));

        StopSpawner();
        ObstacleSpawner();
    }
    void StopSpawner()
    {
        if (isSpawn)
            fStopSpawnTimer += Time.deltaTime;
        if (fStopSpawnTimer > fStopSpawnTime) //����ü�� �����Ǵ� �ð� ����
        {
            isSpawn = false;
            fStopSpawnTimer = 0;
        }
    }
    void ObstacleSpawner()
    {
        if(isSpawn)
        {
            fObstacleSpawnTimer += Time.deltaTime;
            if (fObstacleSpawnTimer > 0.1f) //����ü ���� �ֱ�
            {
                StaticMethodScript.CreatPoolObject(obstacles, obstacle, transform).transform.localScale = new Vector3(2, 2, 2);
                fObstacleSpawnTimer = 0;
            }
        }
        else
            fObstacleSpawnTimer = 0;
    }
}
