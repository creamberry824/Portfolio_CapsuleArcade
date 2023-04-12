using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    Rigidbody rigid;
    AudioSource audioSource;
    GameObject parent;
    float fSpeed = 10;
    float timer;

    [Header("Option")]
    public int iPower = 2;

    private void OnEnable()
    {
        rigid = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = GameManager.Instance.dataManager.data.fSoundEffect;

        transform.localPosition = Vector3.zero;
        transform.localScale = Vector3.one;
        transform.eulerAngles = transform.parent.transform.eulerAngles;
        parent = transform.parent.gameObject;

        if (transform.parent.transform.parent)//수정하기
            transform.parent = transform.parent.transform.parent;
        else transform.parent = null;
        timer = 0;
    }
    void Update()
    {
        rigid.velocity = transform.forward * fSpeed;
        timer += Time.deltaTime;
        if (timer > 3)
        {
            transform.parent = parent.transform;
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
            other.GetComponent<Player>().Damage(iPower);
    }
}
