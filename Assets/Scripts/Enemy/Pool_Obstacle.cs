using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool_Obstacle : MonoBehaviour
{
    public GameObject obstacle; //투사체
    List<GameObject> obstacles =  new List<GameObject>(); //투세체 풀

    float fObstacleSpawnTimer = 0;
    float fStopSpawnTimer = 0; //투사체 생성 주기
    float fTurnSpeed = 180; //투사체 스피드
    [HideInInspector] public float fStopSpawnTime = 5; //투사체 생성 시간 길이
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
        if (fStopSpawnTimer > fStopSpawnTime) //투사체가 생성되는 시간 길이
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
            if (fObstacleSpawnTimer > 0.1f) //투사체 생성 주기
            {
                StaticMethodScript.CreatPoolObject(obstacles, obstacle, transform).transform.localScale = new Vector3(2, 2, 2);
                fObstacleSpawnTimer = 0;
            }
        }
        else
            fObstacleSpawnTimer = 0;
    }
}
