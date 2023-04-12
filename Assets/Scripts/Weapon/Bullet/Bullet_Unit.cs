using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class Bullet_Unit : MonoBehaviour
{
    //State
    float fSpeed;
    bool isGuidedBullet = false;
    bool isPenetrat = false;
    float fDuration;
    float fDurationTimer;
    bool isExplode = false;
    [HideInInspector] public float iPower;
    [HideInInspector] public int iKnockBack;

    Rigidbody rigid;
    Player player;
    Enemy_Health enemy_health;
    EnemyBoss_Health enemy_boss_health;
    Collider collider;
    Renderer renderer;

    [Header("Sound")]
    [HideInInspector] public AudioSource audioSource;
    public AudioClip aclipBurst;
    [Header("Options")]
    public GameObject BurstObject;
    public ParticleSystem particleTrail;
    public ParticleSystem particlExplode;

    [HideInInspector] public Transform Bullet_Pos;
    [HideInInspector] public Transform Target = null;

    private void OnEnable()
    {
        rigid = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
        renderer = GetComponent<Renderer>();
        audioSource = GetComponent<AudioSource>();

        player = GameManager.Instance.Player.GetComponent<Player>();
        iPower = player.weapon.WeaponData.iPower;
        fSpeed = player.weapon.WeaponData.fSpeed;
        fDuration = player.weapon.WeaponData.fDuration;
        iKnockBack = player.weapon.WeaponData.iKnockBack * 1000;
        isGuidedBullet = player.weapon.WeaponData.isGuidedBullet;
        isPenetrat = player.weapon.WeaponData.isPenetrat;

        if (collider) collider.enabled = true;
        if (renderer) renderer.enabled = true;
        if (particleTrail) particleTrail.Play();

        audioSource.volume = GameManager.Instance.dataManager.data.fSoundEffect;
        transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z);
        transform.localPosition = new Vector3(0, 0, 0);
        fDurationTimer = 0;
        isExplode = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
            if (BurstObject) //폭발 탄환이 있다면 
            {
                BurstObject.SetActive(true);
                StartCoroutine(ResetBullet());
            }
            else
            {
                enemy_health = other.GetComponent<Enemy_Health>();
                if (enemy_health != null)
                {
                    enemy_health.Damage(iPower); 
                    enemy_health.Nuckback(Random.Range(iKnockBack - 200, iKnockBack));
                }
                enemy_boss_health = other.GetComponent<EnemyBoss_Health>();
                if (enemy_boss_health != null)
                    enemy_boss_health.Damage(iPower);
                if (!isPenetrat) StartCoroutine(ResetBullet()); //관통탄환이라면 제거되 지 않음
            }
    }
    void Update()
    {
        Move();
        fDurationTimer += Time.deltaTime;
        if (fDurationTimer > fDuration)
            if (!isExplode)
                StartCoroutine(ResetBullet());

        if (isGuidedBullet)
        {
            GuideMove();
            if(Target == null)
            {
                if (!isExplode)
                    StartCoroutine(ResetBullet());
            }else if (Vector3.Distance(transform.position, Target.position) < 2f)
            {
                if (!isExplode)
                    StartCoroutine(ResetBullet());
            }
        }
    }
    void Move()
    {
        rigid.velocity = transform.forward * fSpeed;
        transform.position = new Vector3(transform.position.x, 1, transform.position.z);
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
    }
    void GuideMove() //유도탄이라면 플레이어에게 이동
    {
        if (Target)
        {
            Vector3 vecTarget = Target.transform.position - transform.position;
            rigid.rotation = Quaternion.Lerp(rigid.rotation, Quaternion.LookRotation(new Vector3(vecTarget.x, 0, vecTarget.z)), Time.deltaTime * 50);
        }
    }
    IEnumerator ResetBullet()
    {
        isExplode = true;
        if (BurstObject) //폭발탄환이 있다면
            BurstObject.SetActive(true);
        Target = null;
        fSpeed = 0;
        if (collider) collider.enabled = false;
        if (renderer) renderer.enabled = false;
        if (particleTrail) particleTrail.Stop();
        if(particlExplode) particlExplode.Play();
        if (BurstObject) yield return new WaitForSeconds(3f); //폭발을 기다리는 시간
        transform.parent = Bullet_Pos;
        gameObject.SetActive(false);
    }
}
