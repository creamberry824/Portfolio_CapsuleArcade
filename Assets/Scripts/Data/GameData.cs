using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public class Data
{
    public string strName = " "; //�̸�
    public int iCoin = 0; //����
    public int iSlot = 6; //����
    public int iStageLevel = 1; //�Ϸ��� ��������
    public int iPassTicket = 3; //���������� �÷����ϴµ� �ʿ��� Ƽ��

    public float fSoundMusic = 0.5f; //���� ����
    public float fSoundEffect = 0.5f; //ȿ���� ����
    public float fSoundButton = 0.5f; //UI����


    public List<WeaponData> WeaponDatas = new List<WeaponData>(); //�������ִ� ���� ������
    public WeaponData HaveWeapon = new WeaponData(); //������ ���� ������

    public List<int> iChallengeScore = new List<int>(); //ç������� ����

    public bool isTutorialClear = false; //Ʃ������ Ŭ���� ����
}