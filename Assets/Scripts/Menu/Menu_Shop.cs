using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Menu_Shop : MonoBehaviour
{
    DataManager dataManager;
    Menu_Weapon weaponSelectMenu;
    public TextMeshProUGUI Text_ShopMessage;
    public TextMeshProUGUI Text_WeaponPrice;
    public TextMeshProUGUI Text_SlotPrice;
    public TextMeshProUGUI Text_PassPrice;

    int iWeaponPrice = 9999;
    int iSlotPrice = 9500;
    int iPassPrice = 1000;

    float fShopMessageTimerMax = 2.0f;
    float fShopMessageTimer;

    public GameObject CapsuleMachine;

    public GameObject PassPanel;

    private void Start()
    {
        dataManager = GameManager.Instance.dataManager;
        weaponSelectMenu = FindObjectOfType<Menu_Weapon>();

        Text_WeaponPrice.text = GetThousandCommaText(iWeaponPrice);
        Text_SlotPrice.text = GetThousandCommaText(iSlotPrice);
        Text_PassPrice.text = GetThousandCommaText(iPassPrice);

        fShopMessageTimer = fShopMessageTimerMax;
    }
    private void Update()
    {
        if (fShopMessageTimer < 0)
            Text_ShopMessage.gameObject.SetActive(false);
        else
            fShopMessageTimer -= Time.deltaTime;
    }

    string GetThousandCommaText(int data) => string.Format("{0:#,###}", data);


    public void ConfirmationPanel(GameObject confirmationpanel)
    {
        GameManager_Sound.instance.ButtonClick();
        confirmationpanel.SetActive(true);
    }
    public void ConfirmationPanelExit(GameObject confirmationpanel)
    {
        GameManager_Sound.instance.ButtonClick();
        Text_ShopMessage.gameObject.SetActive(false);
        confirmationpanel.SetActive(false);
    }
    public void WeaponPurcha(GameObject confirmationpanel)
    {
        int iWeaponDatasCount = 1;
        if (dataManager.data.WeaponDatas != null)
            iWeaponDatasCount = dataManager.data.WeaponDatas.Count;


        if (dataManager)
        {
            dataManager.SaveGameData();
            if (dataManager.data.iCoin < iWeaponPrice)
                ShopMessage("포인트가 부족합니다!!", Color.red);
            else if(dataManager.data.iSlot <= iWeaponDatasCount)
                ShopMessage("슬롯이 부족합니다!!", Color.red);
            else
            {
                GameManager_Sound.instance.ButtonPurcha();
                CapsuleMachine.GetComponent<Menu_CapsuleMachine>().StartButton();
                ShopMessage("무기를 구매하였습니다!", Color.yellow);
                Debug.Log(dataManager.data.WeaponDatas);
                dataManager.data.iCoin -= iWeaponPrice;

                //웨폰 데이터에 슬롯 위치 데이터가 없다면
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
                        else
                            iWeaponIndex = i + 1;
                    }
                else
                    iWeaponIndex = 0;

                for (int i = 0; i < 18; i++)
                {
                    if (weaponSelectMenu.SlotTransform[i].childCount == 0) //슬롯에 웨폰이 없다면
                    {
                        iSlotNumber = weaponSelectMenu.SlotTransform[i].GetComponent<Menu_WeaponSlot>().iSlotIndex; //슬롯 넘버 가져오기
                        Debug.Log(weaponSelectMenu.SlotTransform[i].GetComponent<Menu_WeaponSlot>().iSlotIndex);
                        break;
                    }
                }
                switch (Random.Range(0, 6)) //랜덤으로 무기 획득
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
                confirmationpanel.SetActive(false);
                dataManager.SaveGameData();
            }
        }
        GameManager_Sound.instance.ButtonClick();
    }
    public void SlotPurcha(GameObject confirmationpanel)
    {
        if (dataManager)
        {
            if (dataManager.data.iCoin < iSlotPrice)
                ShopMessage("포인트가 부족합니다!!", Color.red);
            else
            {
                if (dataManager.data.iSlot <= 17)
                {
                    GameManager_Sound.instance.ButtonPurcha();
                    ShopMessage("슬롯을 구매하였습니다!", Color.yellow);
                    dataManager.data.iCoin -= iSlotPrice;

                    dataManager.data.iSlot += 1;
                    weaponSelectMenu.ActiveSlot(dataManager);

                    confirmationpanel.SetActive(false);
                    dataManager.SaveGameData();
                }
                else
                    ShopMessage("슬롯을 더 이상 구매할 수 없습니다!", Color.red);
            }
        }
        GameManager_Sound.instance.ButtonClick();
    }
    public void PassPurcha(GameObject confirmationpanel)
    {
        if (dataManager)
        {
            if (dataManager.data.iCoin < iPassPrice)
                ShopMessage("포인트가 부족합니다!!", Color.red);
            else
            {
                GameManager_Sound.instance.ButtonPurcha();
                ShopMessage("패스권을 구매하였습니다!", Color.yellow);
                dataManager.data.iCoin -= iPassPrice;

                dataManager.data.iPassTicket += 1;

                confirmationpanel.SetActive(false);
                dataManager.SaveGameData();
            }
        }
        GameManager_Sound.instance.ButtonClick();



        GameManager_Sound.instance.ButtonClick();
        Debug.Log("PassPurcha");
        confirmationpanel.SetActive(false);
    }
    public void PassRewarded(GameObject confirmationpanel)
    {
        if (dataManager)
        {
            GameManager_Sound.instance.ButtonClick();
            ShopMessage("패스권을 획득하였습니다!", Color.blue);
            dataManager.data.iPassTicket += 1;
            confirmationpanel.SetActive(false);
            dataManager.SaveGameData();
        }
    }

    void ShopMessage(string _message , Color _color)
    {
        fShopMessageTimer = fShopMessageTimerMax;
        Text_ShopMessage.text = _message;
        Text_ShopMessage.color = _color;
        Text_ShopMessage.gameObject.SetActive(true);
    }
}
