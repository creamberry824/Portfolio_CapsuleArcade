using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoss_Robot : EnemyBoss
{
    float fByTurnTimer = 0;
    float fByTurnTime =1;
    int iByTurnCount = 0;

    [Header("LaserCannon")]
    public GameObject LaserR;
    public GameObject LaserL;

    Pool_TargetDropMissile poolManager_TargetDropMissile;
    Pool_DropMissile poolManager_DropMissile;
    bool isDropMissile = false;
    bool isTargetDropMissile = false;

    [Header("GuidedMissile")]
    public GameObject GuidedMissile;
    public Transform GuidedMissilePosition;
    List<GameObject> GuidedMissiles = new List<GameObject>();

    [Header("particle")]
    public ParticleSystem particleBomb;

    [Header("Audio")]
    public AudioClip aclipLaserCannonReady;
    public AudioClip aclipLaserCannonLoop;
    public AudioClip aclipShotGuidedMissile;

    protected override void ConnectComponent()
    {
        base.ConnectComponent();
        poolManager_TargetDropMissile = FindObjectOfType<Pool_TargetDropMissile>();
        poolManager_DropMissile = FindObjectOfType<Pool_DropMissile>();
    }
    protected override void AttacPattern()
    {
        base.AttacPattern();
        if (isMove) 
            fByTurnTimer += Time.deltaTime;
        else if (rigid)
            rigid.velocity = Vector3.zero;

        if (!isDeath)
            ByTurnSkill();

        if (animator.GetBool("isLaserCannon"))
            if (rigid)
                rigid.rotation = rigid.rotation * Quaternion.Euler(0, 50 * Time.deltaTime, 0);

        if (isDropMissile) poolManager_DropMissile.Drop();
        if (isTargetDropMissile) poolManager_TargetDropMissile.Drop();

    }
    void ByTurnSkill()
    {
        if (fByTurnTimer > fByTurnTime)
        {
            fByTurnTimer = 0;
            switch (iByTurnCount)
            {
                case 0:
                    StartCoroutine(CreateGuidedMissile());
                    break;
                case 1:
                    StartCoroutine(LaserCannon());
                    break;
                case 2:
                    StartCoroutine(DropMissile());
                    break;
            }
            iByTurnCount = iByTurnCount >= 2 ? 0 : ++iByTurnCount;
        }
    }
    #region LaserCannon
    IEnumerator LaserCannon()
    {
        audioSource.clip = aclipLaserCannonReady;
        audioSource.Play();

        isMove = false;
        animator.SetBool("isLaserCannon", true);
        yield return new WaitForSeconds(1.2f);
        audioSource.clip = aclipLaserCannonLoop;
        audioSource.loop = true;
        audioSource.Play();
        LaserR.SetActive(true);
        LaserL.SetActive(true);
        yield return new WaitForSeconds(10f);
        audioSource.loop = false;
        audioSource.Pause();
        isMove = true;
        animator.SetBool("isLaserCannon", false);
        LaserR.SetActive(false);
        LaserL.SetActive(false);
    }
    #endregion
    #region DropMissile
    IEnumerator DropMissile()
    {
        isMove = false;
        animator.SetBool("isDropMissile", true);
        isTargetDropMissile = true;
        yield return new WaitForSeconds(2f);
        isDropMissile = true;
        yield return new WaitForSeconds(3f);
        isMove = true;
        animator.SetBool("isDropMissile", false);
        isTargetDropMissile = false;
        isDropMissile = false;
    }
    #endregion
    #region GuidedMissile
    IEnumerator CreateGuidedMissile()
    {
        isMove = false;
        animator.SetBool("isBombCreate", true);
        yield return new WaitForSeconds(5f);
        isMove = true;
        animator.SetBool("isBombCreate", false);
    }

    public void ClipCreateGuidedMissile()
    {
        audioSource.PlayOneShot(aclipShotGuidedMissile);
        particleBomb.Play();
        GameObject missile = StaticMethodScript.CreatPoolObject(GuidedMissiles, GuidedMissile, GuidedMissilePosition);
        missile.transform.parent = null;
    }
    #endregion
}