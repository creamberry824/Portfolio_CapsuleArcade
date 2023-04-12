using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponUnit : MonoBehaviour
{
    public GameObject[] WeaponDatas;

    new public string name;
    public int level = 0;
    public int iPower = 0;
    public int iKnockBack = 0;
    public float fRange = 0;
    public float fCooltime = 0;
    public int Weapon_SlotNumber = 0;

    public GameObject WeaponData = null;
    public GameObject Weapon_SlotUI;
}
