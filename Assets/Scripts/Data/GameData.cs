using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public class Data
{
    public string strName = " "; //이름
    public int iCoin = 0; //코인
    public int iSlot = 6; //슬롯
    public int iStageLevel = 1; //완료한 스테이지
    public int iPassTicket = 3; //스테이지를 플레이하는데 필요한 티켓

    public float fSoundMusic = 0.5f; //음악 볼륨
    public float fSoundEffect = 0.5f; //효과음 볼륨
    public float fSoundButton = 0.5f; //UI볼륨


    public List<WeaponData> WeaponDatas = new List<WeaponData>(); //가지고있는 무기 데이터
    public WeaponData HaveWeapon = new WeaponData(); //장착된 무기 데이터

    public List<int> iChallengeScore = new List<int>(); //챌린지모드 점수

    public bool isTutorialClear = false; //튜툐리얼 클리어 여부
}