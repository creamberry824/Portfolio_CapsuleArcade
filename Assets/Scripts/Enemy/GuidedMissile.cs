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
    public GameObject Model; //�̻��� ��
    public float fSpeed; //�̻��� ���ǵ�

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
        if (other.gameObject.CompareTag("Player")) //�÷��̾�� �����Ѵٸ�
        {
            other.GetComponent<Player>().Damage(1);
            StartCoroutine(Explode());
        }
    }
    void Movement() //�̻��� �̵�
    {
        rigid.velocity = transform.forward * fSpeed;
        TargetRotate = Target.position - transform.position;
        TargetRotate.y = 0;
        if (transform.position.y > 1)
            rigid.rotation = Quaternion.Lerp(rigid.transform.rotation, Quaternion.Euler(90, 0, 0), Time.deltaTime * 10); //���߿� �ִٸ� �Ʒ��� �̵�
        else //���� �ִٸ� �÷��̾�� �̵�
        {
            rigid.constraints = RigidbodyConstraints.FreezePositionY;
            rigid.rotation = Quaternion.Lerp(rigid.transform.rotation, Quaternion.LookRotation(TargetRotate), Time.deltaTime * 15); //�ڿ������� ȸ��
        }
    }

    void LifeTim() //�����ֱ�
    {
        fLifeTimer += Time.deltaTime;
        if (fLifeTimer > fLifeTime)
        {
            Explode();
            fLifeTimer = 0;
        }
    }


    IEnumerator Explode() //����
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
