using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public WeaponData WeaponData; //���� ������ ��������
    public GameObject Bullet; //�Ѿ�

    public ParticleSystem particle;
    public AudioClip aclipShot;
    public GameObject Audio_Shot;
    public Transform Bullet_Pos;

    List<GameObject> Audio_Shots = new List<GameObject>();
    List<GameObject> Bullets= new List<GameObject>();

    void Start()
    {
        Audio_Shot.GetComponent<AudioSource>().clip = aclipShot;
    }
    public void Shot(Transform target)
    {
        StaticMethodScript.CreatPoolObject(Audio_Shots, Audio_Shot, transform); //���� Ǯ������ ����
        if (target)
        {
            GameObject ShotBullet  = StaticMethodScript.CreatPoolObject(Bullets, Bullet, Bullet_Pos);//Ǯ������ �Ѿ� ����
            ShotBullet.transform.parent = null;
            StaticMethodScript.RendererHidn(ShotBullet.transform, false);
            ShotBullet.GetComponent<Bullet_Unit>().Target = target;
            ShotBullet.GetComponent<Bullet_Unit>().Bullet_Pos = Bullet_Pos;
            Vector3 BulletDirection = target.transform.position - ShotBullet.transform.localPosition;
            ShotBullet.transform.localRotation = Quaternion.LookRotation(BulletDirection);
        }
        if (particle) particle.Play();
    }
}
