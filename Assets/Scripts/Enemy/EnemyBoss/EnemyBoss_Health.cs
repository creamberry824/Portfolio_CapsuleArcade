using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBoss_Health : MonoBehaviour
{
    EnemyBoss enemyBoss;
    public Slider slider_Hp;

    private void OnEnable()
    {
        enemyBoss = GetComponent<EnemyBoss>();
        if (slider_Hp)
        {
            slider_Hp.maxValue = enemyBoss.iMaxHp;
            slider_Hp.value = slider_Hp.maxValue;
        }
    }
    private void Update()
    {
        if (slider_Hp.value <= 0)
            if (enemyBoss.isDeath == false)
            {
                enemyBoss.StartCoroutine(enemyBoss.DeathCoroutine());
                enemyBoss.isDeath = true;
            }
    }
    public void Damage(float fDamage)
    {
        slider_Hp.value -= fDamage;
        if (enemyBoss && !enemyBoss.isDeath)
            enemyBoss.Damage();
    }

}
