using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_UFO : Enemy
{
    float fRange = 20;
    public GameObject Obstacle;
    GameObject ObstacleStay;


    protected override void Movement() //UFO������
    {
        base.Movement();
        if (Vector3.Distance(player.transform.position, transform.position) < fRange) //�÷��̾ ��Ÿ� �ȿ� �ִٸ� ������ ����
            isMove = false;
        else isMove = true;

        if (isDeath) //�״´ٸ� �������� �۾����� ����
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, 5f * Time.deltaTime);
    }
    protected override void AttacPattern() //��������
    {
        base.AttacPattern();
        ObstacleShot(!isMove && !isDeath);
    }
    #region Obstacle
    void ObstacleShot(bool isSpawn) //����ü �߻�
    {
        if (isSpawn)
            if (ObstacleStay == null) //����ü�� ���ٸ� ����
                GetObstacleSingle();
            else if (!ObstacleStay.activeSelf) //�ִٸ� ����
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
