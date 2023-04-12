using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager_Menu : MonoBehaviour
{

    //게임 테스트를 위한 메뉴

    DataManager dataManager;
    Menu_Weapon weaponSelectMenu;
    private void Start()
    {
        dataManager = GameManager.Instance.dataManager;
        weaponSelectMenu = FindObjectOfType<Menu_Weapon>();
    }
    
    public void GetMoney()
    {
        if (dataManager)
        {
            dataManager.data.iCoin = 999999;
            dataManager.data.iStageLevel = 30;
            dataManager.SaveGameData();
            SceneManager.LoadScene(1);
            for (int i = 0; i < 6; i++)
                GetWeapon(i);
        }
    }

    private void GetWeapon(int iWeaponNuumber)
    {
        int iWeaponDatasCount = 1;
        if (dataManager.data.WeaponDatas != null)
            iWeaponDatasCount = dataManager.data.WeaponDatas.Count;
        if (dataManager.data.iSlot <= iWeaponDatasCount)
            return;

        int iWeaponIndex = 0;
        int iSlotNumber = 0;
        if (dataManager.data.WeaponDatas.Count > 0)
            for (int i = 0; i < dataManager.data.WeaponDatas.Count; i++)
            {
                if (i != dataManager.data.WeaponDatas[i].Weapon_SlotNumber)
                {
                    iWeaponIndex = i;
                    break;
                }
                else iWeaponIndex = i + 1;
            }
        else iWeaponIndex = 0;

        for (int i = 0; i < 18; i++)
        {
            if (weaponSelectMenu.SlotTransform[i].childCount == 0)
            {
                iSlotNumber = weaponSelectMenu.SlotTransform[i].GetComponent<Menu_WeaponSlot>().iSlotIndex;
                break;
            }
        }
        switch (iWeaponNuumber)
        {
            case 0:
                dataManager.data.WeaponDatas.Insert(iWeaponIndex, new WeaponData_Pistol());
                dataManager.data.WeaponDatas[iWeaponIndex].Weapon_SlotNumber = iSlotNumber;
                weaponSelectMenu.BuyWeapon(iWeaponIndex);
                break;
            case 1:
                dataManager.data.WeaponDatas.Insert(iWeaponIndex, new WeaponData_BoomShot());
                dataManager.data.WeaponDatas[iWeaponIndex].Weapon_SlotNumber = iSlotNumber;
                weaponSelectMenu.BuyWeapon(iWeaponIndex);
                break;
            case 2:
                dataManager.data.WeaponDatas.Insert(iWeaponIndex, new WeaponData_RocketLauncher());
                dataManager.data.WeaponDatas[iWeaponIndex].Weapon_SlotNumber = iSlotNumber;
                weaponSelectMenu.BuyWeapon(iWeaponIndex);
                break;
            case 3:
                dataManager.data.WeaponDatas.Insert(iWeaponIndex, new WeaponData_Bomber());
                dataManager.data.WeaponDatas[iWeaponIndex].Weapon_SlotNumber = iSlotNumber;
                weaponSelectMenu.BuyWeapon(iWeaponIndex);
                break;
            case 4:
                dataManager.data.WeaponDatas.Insert(iWeaponIndex, new WeaponData_Gatling());
                dataManager.data.WeaponDatas[iWeaponIndex].Weapon_SlotNumber = iSlotNumber;
                weaponSelectMenu.BuyWeapon(iWeaponIndex);
                break;
            case 5:
                dataManager.data.WeaponDatas.Insert(iWeaponIndex, new WeaponData_Lazer());
                dataManager.data.WeaponDatas[iWeaponIndex].Weapon_SlotNumber = iSlotNumber;
                weaponSelectMenu.BuyWeapon(iWeaponIndex);
                break;
        }
        dataManager.SaveGameData();
    }

    public void ResetSave()
    {
        if (dataManager)
        {
            dataManager.data.strName = " ";
            dataManager.data.iSlot = 6;
            dataManager.data.iCoin = 0;
            dataManager.data.iStageLevel = 1;
            dataManager.data.iPassTicket = 3;
            dataManager.data.WeaponDatas.Clear();
            dataManager.data.HaveWeapon = null;

            dataManager.data.iChallengeScore.Clear();

            dataManager.data.fSoundMusic = 0.5f;
            dataManager.data.fSoundEffect = 0.5f;

            dataManager.SaveGameData();
            Time.timeScale = 1;
            SceneManager.LoadScene(0);
        }
    }
}
