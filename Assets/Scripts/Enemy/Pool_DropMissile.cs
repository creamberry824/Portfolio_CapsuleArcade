using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool_DropMissile : MonoBehaviour
{
    //드랍 미사일 풀링으로 관리

    public GameObject dropMissile;
    List<GameObject> dropMissiles = new List<GameObject>();

    float fDropTimer = 0;
    float fDropTime = 0.1f;
    Transform transMoveTarget;

    public void Drop()
    {
        fDropTimer += Time.deltaTime;
        if (fDropTimer > fDropTime)
        {
            Creat();
            fDropTimer = 0;
        }
        Move();
    }
    void Move()
    {
        transMoveTarget = GameManager.Instance.Player.transform; //드랍될 위치 지정
        transform.position = new Vector3(transMoveTarget.position.x, transMoveTarget.position.y + 55, transMoveTarget.position.z);
    }
    void Creat()
    {
        GameObject missile = StaticMethodScript.CreatPoolObject(dropMissiles, dropMissile, transform);
        missile.transform.position = transform.position;
        missile.transform.parent = null;
    }
}
