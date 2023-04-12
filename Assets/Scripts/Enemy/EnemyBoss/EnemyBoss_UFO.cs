using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class EnemyBoss_UFO : EnemyBoss
{

    public Transform Model;
    GameObject ObstacleStay;
    EnemyBoss_Health health;
    float fSpawnePatternTimer = 0;
    float fSpawnePatternTime = 5;
    float fTeleportTimer = 0;
    float fTeleportTime = 20;

    [Header("Obstacle")]
    public Pool_Obstacle poolManagerObstacle;
    public GameObject ObstacleSingle;

    [Header("Particle")]
    public ParticleSystem particleTeleport;
    [Header("Audio")]
    public AudioClip aclipTeleport;

    protected override void ConnectComponent()
    {
        base.ConnectComponent();
        health = GetComponent<EnemyBoss_Health>();
        poolManagerObstacle.fStopSpawnTime = 5;
    }
    protected override void AttacPattern()
    {
        base.AttacPattern();
        fSpawnePatternTimer += Time.deltaTime;
        fTeleportTimer += Time.deltaTime;
        poolManagerObstacle.transform.position = transform.position + new Vector3(0, 1, 0);
        if (isDeath)
        {
            poolManagerObstacle.isSpawn = false;
            poolManagerObstacle.transform.parent = transform;
            Model.localScale = Vector3.Lerp(Model.localScale, Vector3.zero, 5f * Time.deltaTime);
        }
        else
        {
            ObstacleSingleSpawn();
            ObstaclePattern();
            TeleportPattern();
        }

        if (health.slider_Hp.value < health.slider_Hp.maxValue / 2)
        {
            fTeleportTime = 3;
            fSpawnePatternTime = 2;
        }
    }
    #region Teleport
    void TeleportPattern()
    {
        if (fTeleportTimer > fTeleportTime - 1f)
        {
            if (Model.localScale == Vector3.one)
            {
                audioSource.clip = aclipTeleport;
                audioSource.Play();
            }
            particleTeleport.Play();
            Model.localScale = Vector3.Lerp(Model.localScale, Vector3.zero, 10f * Time.deltaTime);
        }
        else
            Model.localScale = Vector3.Lerp(Model.localScale, Vector3.one, 10f * Time.deltaTime);
        if (fTeleportTimer > fTeleportTime)
        {
            Teleport();
            fTeleportTimer = 0;
        }
    }
    int num = 0;
    void Teleport()
    {
        switch (num)
        {
            case 0:
                transform.position = player.transform.position + new Vector3(10, 0, 0);
                break;
            case 1:
                transform.position = player.transform.position + new Vector3(-10, 0, 0);
                break;
            case 2:
                transform.position = player.transform.position + new Vector3(0, 0, 10);
                break;
        }
        num = num >= 2 ? 0 : num + 1;
    }
    #endregion
    #region ObstacleAttack
    void ObstaclePattern()
    {
        if (!isDeath)
            if (fSpawnePatternTimer > fSpawnePatternTime + poolManagerObstacle.fStopSpawnTime)
            {
                poolManagerObstacle.isSpawn = true;
                fSpawnePatternTimer = 0;
            }
    }
    void ObstacleSingleSpawn()
    {
        if (ObstacleStay == null)
            GetObstacleSingle();
        else if (!ObstacleStay.activeSelf)
            ObstacleStay.SetActive(true);
    }
    void GetObstacleSingle()
    {
        ObstacleStay = Instantiate(ObstacleSingle, transform);
        ObstacleStay.transform.parent = poolManagerObstacle.transform.parent;
    }
    #endregion
}

