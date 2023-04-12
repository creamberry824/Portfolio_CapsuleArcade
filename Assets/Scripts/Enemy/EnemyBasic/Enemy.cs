using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //컴포넌트
    Rigidbody rigid;
    CapsuleCollider capsulecol;
    Animator animator;
    Enemy_Health health; //체력정보
    Vector3 vecPlayerLook; //플레이어 방향 저장
    float fMoveSpeed = 3f; //움직임 속도
    
    protected GameObject player; //플레이어
    protected AudioSource audioSource;
    protected bool isMove = true; //움직이는지 확인

    [Header("Damage")]
    public AudioClip aclipDamage;
    public ParticleSystem particDamage;
    public Renderer rendBody;
    public Renderer rendHead;
    [HideInInspector]public bool isDeath = false;

    [Header("Item")]
    public GameObject Coin;
    
    public static int iMaxHp = 2; //외부에서 체력 지정 가능
    public static bool canDropCoin = true;//코인을 드랍할지 여부

    #region Constant
    private void OnEnable()
    {
        ConnectComponent(); //컴포넌트 연결
        StateInit(); //기본값 초기화
    }
    private void FixedUpdate()
    {
        Movement(); //움직임
        AttacPattern(); //공격패턴
        AudioUpdate(); //오디오 볼륨 세이브데이터에서 가져오기
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player")) //플레이어와 충돌하면 데미지
            player.GetComponent<Player>().Damage(1);

        if (collision.gameObject.CompareTag("Enemy")) //Enemy와 충돌하면 이동을 멈추기
            rigid.velocity = Vector3.zero;
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy")) //Enemy와 충돌중이라면 이동을 멈추기
            rigid.velocity = Vector3.zero;
    }
    void AudioUpdate()
    {
        audioSource.volume = GameManager.Instance.dataManager.data.fSoundEffect; //세이브데이터에서 저장된 볼륨값 가져오기
    }
    void StateSlider() //화면에 체력바 출력
    {
        health.slider_Hp.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, -0.6f, 0));
    }
    #endregion
    #region Variable
    protected virtual void ConnectComponent() //컴포넌트 연결
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
    protected virtual void StateInit() //기본값 초기화
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
    protected virtual void Movement() //움직임
    {
        if (isDeath) return; //죽었다면 리턴
        if (isMove)
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, fMoveSpeed * Time.deltaTime);
        vecPlayerLook = player.transform.position - transform.position;
        rigid.rotation = Quaternion.LookRotation(new Vector3(vecPlayerLook.x, 0, vecPlayerLook.z));
    }
    protected virtual void AttacPattern() //공격패턴
    {
    }
    #endregion
    #region Damage
    public IEnumerator Damage() //데미지 코루틴
    {
        audioSource.PlayOneShot(aclipDamage); //소리 재생
        particDamage.Play(); //파티클 재생
        animator.SetTrigger("Damage"); //애니메이션 재생 
        rendBody.material.color = Color.red; //머테리얼 칼라 변경
        rendHead.material.color = Color.red; //머테리얼 칼라 변경
        yield return new WaitForSeconds(.2f); //0.2초 대기
        if (!isDeath)
        {
            rendBody.material.color = Color.white; //칼라 복구
            rendHead.material.color = Color.white;
        }
    }
    public IEnumerator Nuckback(float fPower) //뒤로 밀쳐지기
    {
        if (rigid) rigid.AddForce(-transform.forward * fPower); //리지드를 이용한 힘 작용
        capsulecol.isTrigger = true;
        yield return new WaitForSeconds(.2f); //0.2초 후 복구
        capsulecol.isTrigger = false;
        rigid.velocity = Vector3.zero;
    }
    public IEnumerator Death() //죽음
    {
        GameManager.Instance.poolManager_Enemy.EnemyCount--; //풀링에 Enemy밀집도 값 마이너스
        GameManager_Stage.iKillCount++; //플레이어 킬 카운트 증가
        animator.Play("Death"); //죽음 애니에이션 재생
        if (rigid) rigid.AddForce(-transform.forward * 50);
        isDeath = true;
        gameObject.layer = 1;
        capsulecol.enabled = false;
        capsulecol.isTrigger = true;
        rendBody.material.color = Color.gray; //머테리얼 칼라 변경
        rendHead.material.color = Color.gray;
        yield return new WaitForSeconds(0.5f);
        if (canDropCoin) //코인을 드랍할지 여부
            Coin.SetActive(true);
        Coin.SetActive(false); //코인 회수
        Destroy(rigid);
        yield return new WaitForSeconds(2f);
        gameObject.SetActive(false); //풀링을 위해 엑티브 꺼주기
    }
    #endregion
}