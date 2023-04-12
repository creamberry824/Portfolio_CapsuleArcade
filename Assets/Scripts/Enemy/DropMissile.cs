using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropMissile : MonoBehaviour
{

    Rigidbody rigid;
    Transform parent;
    Collider collider;
    AudioSource audioSource;
    Transform DroopMissileTarget;
    float fSpeed;
    bool isDrop;
    bool isExplode;
    Vector3 DropMissilePosition = new Vector3(0, 40, 0);

    [Header("Option")]
    public Renderer renderer;
    public GameObject TargetMark;
    public bool isTargetDrop = false;

    [Header("Particle")]
    public ParticleSystem particleExplode;
    public ParticleSystem particleTrail;


    private void OnEnable()
    {
        rigid = GetComponent<Rigidbody>();
        DroopMissileTarget = FindObjectOfType<Pool_DropMissile>().transform;
        collider = GetComponent<Collider>();
        audioSource = GetComponent<AudioSource>();

        audioSource.volume = GameManager.Instance.dataManager.data.fSoundEffect;

        renderer.enabled = true;
        collider.enabled = true;
        fSpeed = 40;
        isDrop = false;
        isExplode = false;

        parent = transform.parent;
        transform.localPosition = Vector3.zero;
        TargetMark.transform.parent = transform;
        particleTrail.Play();
    }

    void Update()
    {
        if (!isExplode)
            rigid.velocity = transform.forward * fSpeed;

        if (transform.position.y > 30)
            Drop();

        if (transform.position.y < 1 && !isExplode)
            StartCoroutine(Explode());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
            other.GetComponent<Player>().Damage(10);
    }
    void RandomPostion()
    {
        DropMissilePosition.x = GameManager.Instance.Player.transform.position.x + Random.Range(-6, 6);
        DropMissilePosition.z = GameManager.Instance.Player.transform.position.z + Random.Range(-20, 30);
        DroopMissileTarget.position = DropMissilePosition;
    }
    void TargetPostion()
    {
        DropMissilePosition.x = GameManager.Instance.Player.transform.position.x;
        DropMissilePosition.z = GameManager.Instance.Player.transform.position.z;
        DroopMissileTarget.position = DropMissilePosition;
    }

    void Drop()
    {
        if (!isDrop)
        {
            if (isTargetDrop)
                TargetPostion();
            else
                RandomPostion();
            CreateTargetMark();
            transform.rotation = Quaternion.Euler(90, 0, 0);
            transform.position = DroopMissileTarget.position;
            isDrop = true;
        }
    }

    void CreateTargetMark()
    {
        TargetMark.SetActive(true);
        TargetMark.transform.parent = null;
        TargetMark.transform.position = new Vector3(DroopMissileTarget.position.x, 0, DroopMissileTarget.position.z);
        TargetMark.transform.rotation = Quaternion.identity;
    }
    IEnumerator Explode()
    {
        isExplode = true;
        renderer.enabled = false;
        collider.enabled = false;
        fSpeed = 0;
        rigid.velocity = Vector3.zero;
        particleTrail.Stop();
        particleExplode.Play();
        audioSource.Play();
        TargetMark.transform.parent = transform;
        TargetMark.SetActive(false);
        yield return new WaitForSeconds(2f);
        transform.parent = parent;
        gameObject.SetActive(false);
    }
}
