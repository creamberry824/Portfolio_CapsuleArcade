using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_UFO : Enemy
{
    float fRange = 20;
    public GameObject Obstacle;
    GameObject ObstacleStay;


    protected override void Movement() //UFO움직임
    {
        base.Movement();
        if (Vector3.Distance(player.transform.position, transform.position) < fRange) //플레이어가 사거리 안에 있다면 움직임 멈춤
            isMove = false;
        else isMove = true;

        if (isDeath) //죽는다면 스케일이 작아지는 연출
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, 5f * Time.deltaTime);
    }
    protected override void AttacPattern() //공격패턴
    {
        base.AttacPattern();
        ObstacleShot(!isMove && !isDeath);
    }
    #region Obstacle
    void ObstacleShot(bool isSpawn) //투사체 발사
    {
        if (isSpawn)
            if (ObstacleStay == null) //투사체가 없다면 생성
                GetObstacleSingle();
            else if (!ObstacleStay.activeSelf) //있다면 재사용
            {
                ObstacleStay.SetActive(true);
                ObstacleStay.transform.parent = null;
            }
    }
    void GetObstacleSingle()
    {
        ObstacleStay = Instantiate(Obstacle, transform);
        ObstacleStay.transform.parent = null;
    }
    #endregion
}
