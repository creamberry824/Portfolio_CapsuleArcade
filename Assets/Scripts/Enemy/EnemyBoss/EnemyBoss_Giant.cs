using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoss_Giant : EnemyBoss
{
    float fRushTimer = 0;
    float fRushTime = 3;
    bool isRush = true;
    bool isHowling = false;
    int HowlingTerm = 0;
    Enemy_Spawner spawner;

    [Header("Rush")]
    public GameObject RushCollider;
    public ParticleSystem particleRush;

    [Header("Audio")]
    public AudioClip aclipRushStart;
    public AudioClip aclipRush;
    public AudioClip aclipRushEnd;
    protected override void ConnectComponent()
    {
        base.ConnectComponent();
        spawner = FindAnyObjectByType<Enemy_Spawner>();
    }
    public override IEnumerator DeathCoroutine()
    {
        isHowling = false;
        isMove = false;
        isRush = false;
        GameManager.Instance.stageManager.EnemyClear();
        return base.DeathCoroutine();
    }
    protected override void AttacPattern()
    {
        base.AttacPattern();
        fRushTimer += Time.deltaTime;
        RushPattern(isRush);
        if (isHowling) 
            spawner.EnemySpawn(100, 1, 1);
    }
    #region Rush
    void RushPattern(bool _isRush)
    {
        if (_isRush)
        {
            if (fRushTimer > fRushTime)
            {
                StartCoroutine(Rush());
                fRushTimer = 0;
                HowlingTerm++;
                if(HowlingTerm > 5)
                    StartCoroutine(Howling());
            }
        }
        else
        {
            HowlingTerm = 0;
            fRushTimer = 0;
        }

    }
    IEnumerator Rush()
    {
        if (isDeath) yield return null;
        RushCollider.GetComponent<SphereCollider>().enabled = true;
        animator.SetBool("isRush" , true);
        isMove = false;
        audioSource.PlayOneShot(aclipRushStart);
        yield return new WaitForSeconds(0.5f);
        if (isDeath) yield return null;
        audioSource.clip = aclipRush;
        audioSource.Play();
        if (rigid) rigid.AddForce(transform.forward * 2000);
        animator.SetBool("isRush", false);
        yield return new WaitForSeconds(0.8f);
        if (isDeath) yield return null;
        particleRush.Play();
        if (rigid) rigid.velocity = Vector3.zero;
        if (isRush)
            isMove = true;
        audioSource.PlayOneShot(aclipRushEnd);
        RushCollider.GetComponent<SphereCollider>().enabled = false;

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
            if (other.GetComponent<Enemy_Health>())
                other.GetComponent<Enemy_Health>().Damage(1000);
        if (other.gameObject.tag == "Player")
            other.GetComponent<Player>().Damage(1000);
    }
    #endregion
    #region Howling
    IEnumerator Howling()
    {
        if (isDeath) yield return null;
        isHowling = true;
        isMove = false;
        isRush = false;
        animator.SetBool("isHowling", true);
        rigid.constraints = RigidbodyConstraints.FreezeAll | RigidbodyConstraints.FreezeAll;
        yield return new WaitForSeconds(15f);
        if (isDeath) yield return null;
        rigid.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
        animator.SetBool("isHowling", false);
        isHowling = false;
        isMove = true;
        isRush = true;
    }
    #endregion
}
