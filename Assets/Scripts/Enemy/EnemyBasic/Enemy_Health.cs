using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy_Health : MonoBehaviour
{
    Enemy enemy; //Enemy����
    public Slider slider_Hp; //�Ǹ��� ��������

    private void Start()
    {
        enemy = GetComponent<Enemy>();   
    }

    public void Damage(float fDamage) //���̹���
    {
        slider_Hp.value -= fDamage; //�Ǹ��� ���� ��������ŭ �ڷ� �̵�
        if(!enemy.isDeath) //���� ����� Enemy�� �׾��ٸ�
        {
            enemy.StartCoroutine(enemy.Damage());
            if (slider_Hp.value <= 0) enemy.StartCoroutine(enemy.Death());
        }
    }
    public void Nuckback(float fPower) //�˹�
    {
        if (enemy)
            enemy.StartCoroutine(enemy.Nuckback(fPower));
    }
}
