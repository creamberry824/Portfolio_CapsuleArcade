using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy_Health : MonoBehaviour
{
    Enemy enemy; //Enemy지정
    public Slider slider_Hp; //실린더 가져오기

    private void Start()
    {
        enemy = GetComponent<Enemy>();   
    }

    public void Damage(float fDamage) //데이미지
    {
        slider_Hp.value -= fDamage; //실린더 값을 데미지만큼 뒤로 이동
        if(!enemy.isDeath) //만약 연결된 Enemy가 죽었다면
        {
            enemy.StartCoroutine(enemy.Damage());
            if (slider_Hp.value <= 0) enemy.StartCoroutine(enemy.Death());
        }
    }
    public void Nuckback(float fPower) //넉백
    {
        if (enemy)
            enemy.StartCoroutine(enemy.Nuckback(fPower));
    }
}
