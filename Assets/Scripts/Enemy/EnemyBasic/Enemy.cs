using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //������Ʈ
    Rigidbody rigid;
    CapsuleCollider capsulecol;
    Animator animator;
    Enemy_Health health; //ü������
    Vector3 vecPlayerLook; //�÷��̾� ���� ����
    float fMoveSpeed = 3f; //������ �ӵ�
    
    protected GameObject player; //�÷��̾�
    protected AudioSource audioSource;
    protected bool isMove = true; //�����̴��� Ȯ��

    [Header("Damage")]
    public AudioClip aclipDamage;
    public ParticleSystem particDamage;
    public Renderer rendBody;
    public Renderer rendHead;
    [HideInInspector]public bool isDeath = false;

    [Header("Item")]
    public GameObject Coin;
    
    public static int iMaxHp = 2; //�ܺο��� ü�� ���� ����
    public static bool canDropCoin = true;//������ ������� ����

    #region Constant
    private void OnEnable()
    {
        ConnectComponent(); //������Ʈ ����
        StateInit(); //�⺻�� �ʱ�ȭ
    }
    private void FixedUpdate()
    {
        Movement(); //������
        AttacPattern(); //��������
        AudioUpdate(); //����� ���� ���̺굥���Ϳ��� ��������
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player")) //�÷��̾�� �浹�ϸ� ������
            player.GetComponent<Player>().Damage(1);

        if (collision.gameObject.CompareTag("Enemy")) //Enemy�� �浹�ϸ� �̵��� ���߱�
            rigid.velocity = Vector3.zero;
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy")) //Enemy�� �浹���̶�� �̵��� ���߱�
            rigid.velocity = Vector3.zero;
    }
    void AudioUpdate()
    {
        audioSource.volume = GameManager.Instance.dataManager.data.fSoundEffect; //���̺굥���Ϳ��� ����� ������ ��������
    }
    void StateSlider() //ȭ�鿡 ü�¹� ���
    {
        health.slider_Hp.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, -0.6f, 0));
    }
    #endregion
    #region Variable
    protected virtual void ConnectComponent() //������Ʈ ����
    {
        if (!gameObject.GetComponent<Rigidbody>())
            gameObject.AddComponent<Rigidbody>();
        player = GameManager.Instance.Player;
        rigid = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        capsulecol = GetComponent<CapsuleCollider>();
        audioSource = GetComponent<AudioSource>();
        health = GetComponent<Enemy_Health>();
    }
    protected virtual void StateInit() //�⺻�� �ʱ�ȭ
    {
        isDeath = false;
        gameObject.layer = 3;
        capsulecol.enabled = true;
        capsulecol.isTrigger = false;
        rendBody.material.color = Color.white;
        rendHead.material.color = Color.white;
        rigid.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;

        health.slider_Hp.maxValue = iMaxHp;
        health.slider_Hp.value = health.slider_Hp.maxValue;

        transform.localScale = Vector3.one;
    }
    protected virtual void Movement() //������
    {
        if (isDeath) return; //�׾��ٸ� ����
        if (isMove)
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, fMoveSpeed * Time.deltaTime);
        vecPlayerLook = player.transform.position - transform.position;
        rigid.rotation = Quaternion.LookRotation(new Vector3(vecPlayerLook.x, 0, vecPlayerLook.z));
    }
    protected virtual void AttacPattern() //��������
    {
    }
    #endregion
    #region Damage
    public IEnumerator Damage() //������ �ڷ�ƾ
    {
        audioSource.PlayOneShot(aclipDamage); //�Ҹ� ���
        particDamage.Play(); //��ƼŬ ���
        animator.SetTrigger("Damage"); //�ִϸ��̼� ��� 
        rendBody.material.color = Color.red; //���׸��� Į�� ����
        rendHead.material.color = Color.red; //���׸��� Į�� ����
        yield return new WaitForSeconds(.2f); //0.2�� ���
        if (!isDeath)
        {
            rendBody.material.color = Color.white; //Į�� ����
            rendHead.material.color = Color.white;
        }
    }
    public IEnumerator Nuckback(float fPower) //�ڷ� ��������
    {
        if (rigid) rigid.AddForce(-transform.forward * fPower); //�����带 �̿��� �� �ۿ�
        capsulecol.isTrigger = true;
        yield return new WaitForSeconds(.2f); //0.2�� �� ����
        capsulecol.isTrigger = false;
        rigid.velocity = Vector3.zero;
    }
    public IEnumerator Death() //����
    {
        GameManager.Instance.poolManager_Enemy.EnemyCount--; //Ǯ���� Enemy������ �� ���̳ʽ�
        GameManager_Stage.iKillCount++; //�÷��̾� ų ī��Ʈ ����
        animator.Play("Death"); //���� �ִϿ��̼� ���
        if (rigid) rigid.AddForce(-transform.forward * 50);
        isDeath = true;
        gameObject.layer = 1;
        capsulecol.enabled = false;
        capsulecol.isTrigger = true;
        rendBody.material.color = Color.gray; //���׸��� Į�� ����
        rendHead.material.color = Color.gray;
        yield return new WaitForSeconds(0.5f);
        if (canDropCoin) //������ ������� ����
            Coin.SetActive(true);
        Coin.SetActive(false); //���� ȸ��
        Destroy(rigid);
        yield return new WaitForSeconds(2f);
        gameObject.SetActive(false); //Ǯ���� ���� ��Ƽ�� ���ֱ�
    }
    #endregion
}