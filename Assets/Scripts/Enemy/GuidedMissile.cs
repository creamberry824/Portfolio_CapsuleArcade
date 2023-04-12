using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GuidedMissile : MonoBehaviour
{
    Rigidbody rigid;
    Transform Target;
    Collider collider;
    Transform parent;
    Vector3 TargetRotate;
    float fLifeTimer = 0;
    float fLifeTime = 5;

    [Header("Option")]
    public GameObject Model; //미사일 모델
    public float fSpeed; //미사일 스피드

    [Header("Particle")]
    public ParticleSystem particleExplode;
    public ParticleSystem particleTrail;

    private void OnEnable()
    {
        rigid = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
        Target = GameManager.Instance.Player.transform;
        parent = transform.parent;
        rigid.constraints = RigidbodyConstraints.None;
        transform.localPosition = Vector3.zero;
        fSpeed = 2.5f;
        fLifeTimer = 0;
        Model.SetActive(true);
        collider.enabled = true;
        particleTrail.Play();
    }

    void Update()
    {
        LifeTim();
        Movement();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) //플레이어에게 접촉한다면
        {
            other.GetComponent<Player>().Damage(1);
            StartCoroutine(Explode());
        }
    }
    void Movement() //미사일 이동
    {
        rigid.velocity = transform.forward * fSpeed;
        TargetRotate = Target.position - transform.position;
        TargetRotate.y = 0;
        if (transform.position.y > 1)
            rigid.rotation = Quaternion.Lerp(rigid.transform.rotation, Quaternion.Euler(90, 0, 0), Time.deltaTime * 10); //공중에 있다면 아래로 이동
        else //지상에 있다면 플레이어에게 이동
        {
            rigid.constraints = RigidbodyConstraints.FreezePositionY;
            rigid.rotation = Quaternion.Lerp(rigid.transform.rotation, Quaternion.LookRotation(TargetRotate), Time.deltaTime * 15); //자연스러운 회전
        }
    }

    void LifeTim() //생명주기
    {
        fLifeTimer += Time.deltaTime;
        if (fLifeTimer > fLifeTime)
        {
            Explode();
            fLifeTimer = 0;
        }
    }


    IEnumerator Explode() //폭발
    {
        fSpeed = 0;
        rigid.velocity = Vector3.zero;
        collider.enabled = false;
        Model.SetActive(false);
        particleTrail.Stop();
        particleExplode.Play();
        yield return new WaitForSeconds(3f);
        transform.parent = parent;
        gameObject.SetActive(false);
    }
}
