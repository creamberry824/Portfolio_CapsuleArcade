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
                ShopMessage("����Ʈ�� �����մϴ�!!", Color.red);
            else if(dataManager.data.iSlot <= iWeaponDatasCount)
                ShopMessage("������ �����մϴ�!!", Color.red);
            else
            {
                GameManager_Sound.instance.ButtonPurcha();
                CapsuleMachine.GetComponent<Menu_CapsuleMachine>().StartButton();
                ShopMessage("���⸦ �����Ͽ����ϴ�!", Color.yellow);
                Debug.Log(dataManager.data.WeaponDatas);
                dataManager.data.iCoin -= iWeaponPrice;

                //���� �����Ϳ� ���� ��ġ �����Ͱ� ���ٸ�
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
                    if (weaponSelectMenu.SlotTransform[i].childCount == 0) //���Կ� ������ ���ٸ�
                    {
                        iSlotNumber = weaponSelectMenu.SlotTransform[i].GetComponent<Menu_WeaponSlot>().iSlotIndex; //���� �ѹ� ��������
                        Debug.Log(weaponSelectMenu.SlotTransform[i].GetComponent<Menu_WeaponSlot>().iSlotIndex);
                        break;
                    }
                }
                switch (Random.Range(0, 6)) //�������� ���� ȹ��
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
                ShopMessage("����Ʈ�� �����մϴ�!!", Color.red);
            else
            {
                if (dataManager.data.iSlot <= 17)
                {
                    GameManager_Sound.instance.ButtonPurcha();
                    ShopMessage("������ �����Ͽ����ϴ�!", Color.yellow);
                    dataManager.data.iCoin -= iSlotPrice;

                    dataManager.data.iSlot += 1;
                    weaponSelectMenu.ActiveSlot(dataManager);

                    confirmationpanel.SetActive(false);
                    dataManager.SaveGameData();
                }
                else
                    ShopMessage("������ �� �̻� ������ �� �����ϴ�!", Color.red);
            }
        }
        GameManager_Sound.instance.ButtonClick();
    }
    public void PassPurcha(GameObject confirmationpanel)
    {
        if (dataManager)
        {
            if (dataManager.data.iCoin < iPassPrice)
                ShopMessage("����Ʈ�� �����մϴ�!!", Color.red);
            else
            {
                GameManager_Sound.instance.ButtonPurcha();
                ShopMessage("�н����� �����Ͽ����ϴ�!", Color.yellow);
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
            ShopMessage("�н����� ȹ���Ͽ����ϴ�!", Color.blue);
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
