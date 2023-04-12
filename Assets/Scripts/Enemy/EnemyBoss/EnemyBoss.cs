using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoss : MonoBehaviour
{
    protected GameObject player;
    protected Rigidbody rigid;
    protected Collider collider;
    protected Animator animator;
    protected AudioSource audioSource;
    protected float fMoveSpeed = 3f;
    Vector3 vecPlayerLook;
    [HideInInspector] public bool isDeath = false;
    [HideInInspector] public bool isMove = true;

    [Header("Damage")]
    public AudioClip aclipDamage;
    public AudioClip aclipDie;
    public ParticleSystem particleDie;
    public ParticleSystem particHit;

    [Header("Hp")]
    public int iMaxHp = 30;

    [Header("Item")]
    public GameObject Coin;
    public static bool canDropCoin = true;

    #region Constant
    private void OnEnable()
    {
        ConnectComponent();
        StateInit();
    }
    private void FixedUpdate()
    {
        Movement();
        AttacPattern();
        AudioUpdate();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            player.GetComponent<Player>().Damage(10);

        if (collision.gameObject.CompareTag("Enemy"))
            rigid.velocity = Vector3.zero;
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
            rigid.velocity = Vector3.zero;
    }
    void AudioUpdate()
    {
        audioSource.volume = GameManager.Instance.dataManager.data.fSoundEffect;
    }
    #endregion
    #region Variable
    protected virtual void ConnectComponent()
    {
        if (!gameObject.GetComponent<Rigidbody>())
            gameObject.AddComponent<Rigidbody>();
        player = GameManager.Instance.Player;
        rigid = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        collider = GetComponent<Collider>();
        audioSource = GetComponent<AudioSource>();
    }
    protected virtual void StateInit() //initialization
    {
        isDeath = false;
        isMove = true;
        gameObject.layer = 3;
        collider.isTrigger = false;
        rigid.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
    }
    protected virtual void Movement()
    {
        if (isDeath || !isMove) return;
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, fMoveSpeed * Time.deltaTime);
        vecPlayerLook = player.transform.position - transform.position;
        vecPlayerLook.y = 0;
        rigid.rotation = Quaternion.Lerp(rigid.transform.rotation, Quaternion.LookRotation(vecPlayerLook), Time.deltaTime * 2);

    }
    protected virtual void AttacPattern()
    {
    }
    #endregion
    #region Damage
    public virtual void Damage()
    {
        audioSource.PlayOneShot(aclipDamage);
        particHit.Play();
    }
    public virtual IEnumerator DeathCoroutine()
    {
        audioSource.loop = false;
        audioSource.PlayOneShot(aclipDie);
        particleDie.Play();
        GameManager_Stage.iKillCount++;
        GameManager_Stage.iBossKillCount++;
        GameManager_Stage.iBossKillCountScore++;
        animator.Play("Death");
        if (rigid) rigid.AddForce(-transform.forward * 50);
        isDeath = true;
        StaticMethodScript.ChangeLayersRecursively(transform, "Outline");
        yield return new WaitForSeconds(0.5f);
        if (canDropCoin)
            Coin.SetActive(true);
        Coin.SetActive(false);
        Destroy(collider);
        Destroy(rigid);
        yield return new WaitForSeconds(2f);
        gameObject.SetActive(false);
    }
    #endregion
}
