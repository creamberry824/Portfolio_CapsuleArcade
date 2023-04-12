using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool_TargetDropMissile : MonoBehaviour
{
    public GameObject dropMissile; //��ӹ̻���
    List<GameObject> dropMissiles = new List<GameObject>(); //��ӹ̻��� Ǯ

    float fTimer = 0;
    float fTime = 0.3f;

    public void Drop() //������Ʈ ���� �Լ�
    {
        fTimer += Time.deltaTime;
        if (fTimer > fTime)
        {
            StaticMethodScript.CreatPoolObject(dropMissiles, dropMissile, transform).transform.parent = null;
            fTimer = 0;
        }
    }
}
