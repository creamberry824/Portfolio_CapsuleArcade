using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_CloseAttack : MonoBehaviour
{
    public Enemy_Health enemy_health;
    public EnemyBoss_Health enemy_Boss_health;

    float fPower = 1;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            enemy_health = other.GetComponent<Enemy_Health>();
            if (enemy_health != null)
            {
                enemy_health.Damage(fPower);
                enemy_health.Nuckback(Random.Range(800, 1000));
            }

            enemy_Boss_health = other.GetComponent<EnemyBoss_Health>();
            if (enemy_Boss_health != null)
                enemy_Boss_health.Damage(fPower);
        }
    }

}
