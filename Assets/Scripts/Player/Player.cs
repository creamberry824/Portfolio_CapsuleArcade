using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{

    Rigidbody rigid;
    Animator animator;
    Player_Joystick joystickMovement;
    DataManager dataManager;
    CapsuleCollider playerCollider;

    [Header("Sound")]
    AudioSource audioSource;
    public AudioClip aclipDamage;
    public AudioClip aclipSwing;
    public GameObject GetCoinSound;
    List<GameObject> GetCoinSounds = new List<GameObject>();

    [Header("Slider, State")]
    public Slider slider_Hp, slider_Mp;
    int iMaxHp = 10;
    float fMoveSpeed = 5f;
    [HideInInspector] public bool isDeath = false;

    float time_cooltime = 0.4f;
    float time_current = 0;

    [Header("CloseAttackRange")]
    public BoxCollider CloseAttackRange;

    //Target
    LayerMask layer = 3; //Enemy 레이어
    Collider[] col;
    GameObject target;
    float fBasicsRadius = 3.5f;
    float fRadius = 3.5f;

    [Header("Weapon")]
    [HideInInspector] public Weapon weapon;
    public WeaponUnit WeaponUnit;
    public Transform Weapon_Pos;
    bool isWeapon = false;

    bool isAttack = false;

    [Header("UI")]
    CameraMovement cameraMovement;
    public GameObject Canvas_Player;

    [Header("TargetArrow")]
    public Transform AssistanceObject;
    public Transform TargetDirection;
    public Transform TargetArrow;

    bool isInvincibility = false;
    float fInvincibilityTimer = 0;

    float fGetCoinTime = 0.1f;
    float fGetCoinTimer = 0;
    [HideInInspector] public bool isGetCoinReady = true;
    AudioSource PlayingGetCoin;


    void Start()
    {
        dataManager = GameManager.Instance.dataManager;
        rigid = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        joystickMovement = FindObjectOfType<Player_Joystick>();
        cameraMovement = FindObjectOfType<CameraMovement>();
        playerCollider = GetComponent<CapsuleCollider>();
        audioSource = GetComponent<AudioSource>();
        if (SceneManager.GetActiveScene().name == "MaineMenu")
            Canvas_Player.SetActive(false);

        InvokeRepeating("FindCloseEnemy", 0, 0.2f);
        StatSliderReset();
        WeaponDataLoed(dataManager);
        playerCollider.enabled = true;
    }
    private void OnEnable() => WeaponDataLoed(dataManager);
    private void FixedUpdate()
    {
        if (slider_Hp.value <= 0)
            return;
        AttackCoolTime();
        StatSliderUpdate();
        Movement(joystickMovement);
        WeaponDirectionTarget();
        Invincibility();
        GetCoinTimer();

        audioSource.volume = GameManager.Instance.dataManager.data.fSoundEffect;
    }
    #region StatSlider
    void StatSliderReset() //체력과 쿨타임 연결 및 초기화
    {
        if (slider_Hp)
        {
            slider_Hp.maxValue = iMaxHp;
            slider_Hp.value = slider_Hp.maxValue;
        }
        if (slider_Mp) slider_Mp.maxValue = time_cooltime;
    }
    void StatSliderUpdate() //현재 쿨타임 업데이트
    {
        if (slider_Mp) slider_Mp.value = slider_Mp.maxValue - time_current;
    }
    #endregion
    void Movement(Player_Joystick joystickMovement)
    {
        if (joystickMovement) //조이스틱에 맞게 이동
        {
            rigid.velocity = new Vector3(joystickMovement.vecJoy.x * fMoveSpeed, rigid.velocity.y, joystickMovement.vecJoy.y * fMoveSpeed);
            if (rigid.velocity.x + rigid.velocity.z != 0)
                animator.SetBool("isMove", true);
            else
                animator.SetBool("isMove", false);

            if (!isAttack && animator.GetBool("isMove")) //공격중이 아닐떄 조이스틱에 맞게 회전
                rigid.rotation = Quaternion.Lerp(rigid.rotation, Quaternion.LookRotation(new Vector3(joystickMovement.vecJoyRot.x, 0, joystickMovement.vecJoyRot.y)), Time.deltaTime * 10);

            animator.SetBool("isWeapon", isWeapon);
        }
    }
    void FindCloseEnemy() //가까운 적 찾기
    {
        col = Physics.OverlapSphere(transform.position, fRadius, layer); //레이더 생성 
        GameObject short_enemy = null;
        if(col.Length > 0)
        {
            float short_distance = Mathf.Infinity; //Enemy 거리 확인
            foreach (Collider s_col in col)
            {
                float playertoenemy = Vector3.SqrMagnitude(transform.position - s_col.transform.position); //두 점간의 거리를 루트
                if(short_distance > playertoenemy) //측정중인 적의 거리가 적다면  타겟으로 설정
                {
                    short_distance = playertoenemy;
                    short_enemy = s_col.gameObject;
                }
            }
        }
        target = short_enemy; // 타겟 교체
    }
    private void AttackCoolTime()
    {
        if (time_current > 0)
            time_current -= Time.deltaTime;
        else if (time_current <= 0 && target)
        {
            time_current = slider_Mp.maxValue;

            if (!isWeapon) StartCoroutine(CloseAttack());
            else StartCoroutine(WeaponAttack());
        }
    }



    #region CloseAttack
    IEnumerator CloseAttack()
    {
        audioSource.PlayOneShot(aclipSwing);
        isAttack = true;
        Vector3 vecTarget = target.transform.position - transform.position;
        //타겟 방향으로 회전
        rigid.rotation = Quaternion.Lerp(rigid.rotation, Quaternion.LookRotation(new Vector3(vecTarget.x, 0, vecTarget.z)), Time.deltaTime * 50);
        if (!animator.GetBool("isMove"))
            joystickMovement.vecJoyRot = new Vector2(vecTarget.x, vecTarget.z);
        animator.SetTrigger("ATTACK");
        if (animator.GetInteger("iAttackCombo") == 1)
            animator.SetInteger("iAttackCombo", 2);
        else animator.SetInteger("iAttackCombo", 1);
        yield return new WaitForSeconds(0.18f);
        CloseAttackRange.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.05f);
        isAttack = false;
        CloseAttackRange.gameObject.SetActive(false);
    }
    #endregion
    #region Weapon
    void WeaponDirectionTarget() //무기를 보유중이라면
    {
        if (target)
        {
            animator.SetBool("isWeaponShot", true);
            Vector3 targetDirection = target.transform.position - transform.localPosition; //타겟과 플레이어 방향
            Quaternion targetQuaternion = Quaternion.LookRotation(targetDirection.normalized, transform.up.normalized); //타겟방향
            Quaternion forwardQuaterion = Quaternion.LookRotation(transform.forward.normalized, transform.up.normalized); //정면
            TargetDirection.eulerAngles = targetQuaternion.eulerAngles - forwardQuaterion.eulerAngles; //정면에서 타겟방향으로 바라본 방향
            Vector3 resultDirection = AssistanceObject.position - transform.localPosition; //정면에서 타겟방향을 바라본 방향과 플레이어의 방향
            animator.SetFloat("X", Mathf.Lerp(animator.GetFloat("X"), resultDirection.normalized.x, 0.5f));
            animator.SetFloat("Z", Mathf.Lerp(animator.GetFloat("Z"), resultDirection.normalized.z, 0.5f));

            TargetArrow.gameObject.SetActive(true);
            TargetArrow.localRotation = Quaternion.LookRotation(resultDirection);
            TargetArrow.localRotation = new Quaternion(0, TargetArrow.localRotation.y, 0, TargetArrow.localRotation.w);
        }
        else
        {
            animator.SetBool("isWeaponShot", false);
            TargetArrow.gameObject.SetActive(false);
        }
    }
    public void WeaponDataLoed(DataManager _dataManager) //착용중인 무기 데이터 가져오기
    {
        if (Weapon_Pos.childCount != 0)
            Destroy(Weapon_Pos.GetChild(0).gameObject);
        if (_dataManager)
        {
            if (_dataManager.data.HaveWeapon.WeaponData_Code >= 0)
            {
                WeaponUnit.WeaponData = WeaponUnit.WeaponDatas[_dataManager.data.HaveWeapon.WeaponData_Code];
                WeaponUnit.level = _dataManager.data.HaveWeapon.iLevel;
                WeaponUnit.Weapon_SlotNumber = _dataManager.data.HaveWeapon.Weapon_SlotNumber;
                weapon = WeaponUnit.WeaponData.GetComponent<Weapon>();
                WeaponUnit.name = weapon.WeaponData.name;
                WeaponUnit.iPower = weapon.WeaponData.iPower;
                WeaponUnit.fRange = weapon.WeaponData.fRange;
                WeaponUnit.iKnockBack = weapon.WeaponData.iKnockBack;
                WeaponUnit.fCooltime = weapon.WeaponData.fCooltime;

                isWeapon = true;
                slider_Mp.maxValue = WeaponUnit.fCooltime;
                fRadius = WeaponUnit.fRange;

                WeaponCreat(WeaponUnit, Weapon_Pos);
            }
            else
            {
                isWeapon = false;
                slider_Mp.maxValue = time_cooltime;
                fRadius = fBasicsRadius;
            }
            animator.SetBool("isWeapon", isWeapon);
        }
    }

    void WeaponCreat(WeaponUnit _weaponUnit, Transform _cratePos)
    {
        if (_cratePos.childCount != 0)
            Destroy(_cratePos.GetChild(0).gameObject);

        _weaponUnit.WeaponData.layer = 6;
        weapon = Instantiate(_weaponUnit.WeaponData, _cratePos).GetComponent<Weapon>();
    }
     
    IEnumerator WeaponAttack()
    {
        if (!animator.GetBool("isMove"))
        {
            animator.Play("Weapon_Shoot");
            isAttack = true;
            Vector3 vecTarget = target.transform.position - transform.position;
            rigid.rotation = Quaternion.Lerp(rigid.rotation, Quaternion.LookRotation(new Vector3(vecTarget.x, 0, vecTarget.z)), Time.deltaTime * 50);
        }
        yield return new WaitForSeconds(0.06f);
        if (target) weapon.Shot(target.transform);
        isAttack = false;
    }
    #endregion

    #region GetCoin
    public void GetCoin(int iCoinPrice) //코인 획득
    {
        PlayingGetCoin = StaticMethodScript.CreatPoolObject(GetCoinSounds, GetCoinSound, transform).GetComponent<AudioSource>();
        PlayingGetCoin.volume = GameManager.Instance.dataManager.data.fSoundEffect;

        PlayingGetCoin.Play();
        dataManager.data.iCoin += iCoinPrice;
        fGetCoinTimer = 0;
    }
    void GetCoinTimer() //코인 획득 딜레이 시간
    {
        fGetCoinTimer += Time.deltaTime;
        isGetCoinReady = fGetCoinTimer > fGetCoinTime ? true : false;
    }
    #endregion
    #region Damage
    public void Damage(float fDamage) //데미지
    {
        isInvincibility = true;
        slider_Hp.value -= fDamage;
        if (slider_Hp.value <= 0)
        {
            if (rigid)
                StartCoroutine(Death());
            return;
        }
        audioSource.PlayOneShot(aclipDamage);
        cameraMovement.StartCoroutine(cameraMovement.Shake(.5f, .5f)); //카메라 흔들림
    }
    void Invincibility() //무적
    {
        if(isInvincibility)
        {
            playerCollider.enabled = false;
            fInvincibilityTimer += Time.deltaTime;

            for (int i = 0; i < 20; i++)
                if ((i / 10f) < fInvincibilityTimer && fInvincibilityTimer < ((i + 0.5f) / 10))
                    StaticMethodScript.RendererHidn(transform, i % 2 == 0 ? false : true);

            if (fInvincibilityTimer > 1f)
            {
                playerCollider.enabled = true;
                isInvincibility = false;
                fInvincibilityTimer = 0;
                StaticMethodScript.RendererHidn(transform, false);
            }
        }
    }
    IEnumerator Death()
    {
        audioSource.PlayOneShot(aclipDamage);
        Time.timeScale = 0.2f;
        Destroy(rigid);
        TargetArrow.gameObject.SetActive(false);
        Canvas_Player.gameObject.SetActive(false);
        animator.SetBool("isMove", false);
        animator.Play("Death");
        yield return new WaitForSeconds(0.5f);
        Time.timeScale = 1f;
        yield return new WaitForSeconds(4f);
        isDeath = true;
        Time.timeScale = 0;
    }
    #endregion
}