using System;
using UnityEngine;

[Serializable]
public class WeaponData
{
    public int Weapon_SlotNumber = -1;
    public string name = "0";
    public int iLevel;
    public int iPower;
    public int iKnockBack;
    public float fCooltime;
    public float fRange;
    public float fSpeed;
    public bool isPenetrat = false;
    public bool isGuidedBullet = false;
    public float fDuration;

    public int WeaponData_Code = -1;
    public bool isTake = false;
}
