using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Wide_Unit : MonoBehaviour
{

    Collider collider;
    float fExplosionTime = 0.2f;
    float fExplosionSpeed = 10f;
    Bullet_Unit bullet;
    float iPower;
    int iKnockBack;
    private void OnEnable()
    {
        collider = GetComponent<Collider>();
        bullet = transform.parent.GetComponent<Bullet_Unit>();
        iPower = bullet.iPower;
        iKnockBack = bullet.iKnockBack;

        transform.localScale = Vector3.one;
        collider.enabled = true;
        StartCoroutine(Burst());
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            Enemy_Health enemy_health = other.GetComponent<Enemy_Health>();
            if (enemy_health != null)
            {
                enemy_health.Damage(iPower);
                enemy_health.Nuckback(Random.Range(iKnockBack - 200, iKnockBack));
            }
            EnemyBoss_Health enemy_boss_health = other.GetComponent<EnemyBoss_Health>();
            if (enemy_boss_health != null)
                enemy_boss_health.Damage(iPower);
        }
    }
    void Update()
    {
        transform.localScale = transform.localScale * (1f + Time.deltaTime * fExplosionSpeed); //활성화시 공격범위 스케일 증가
    }
    IEnumerator Burst()
    {
        if (bullet.aclipBurst) bullet.audioSource.PlayOneShot(bullet.aclipBurst);
        yield return new WaitForSeconds(fExplosionTime);
        collider.enabled = false;
        gameObject.SetActive(false);
    }
}
