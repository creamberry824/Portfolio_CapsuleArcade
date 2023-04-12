using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool_TargetDropMissile : MonoBehaviour
{
    public GameObject dropMissile; //드롭미사일
    List<GameObject> dropMissiles = new List<GameObject>(); //드롭미사일 풀

    float fTimer = 0;
    float fTime = 0.3f;

    public void Drop() //업데이트 전용 함수
    {
        fTimer += Time.deltaTime;
        if (fTimer > fTime)
        {
            StaticMethodScript.CreatPoolObject(dropMissiles, dropMissile, transform).transform.parent = null;
            fTimer = 0;
        }
    }
}
