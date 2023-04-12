using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class Menu_Weapon : MonoBehaviour
{
    public GameObject SelectWeaponView;
    float fRotSpeed = 20;

    public GameObject StatePanel;
    public TMP_Text Text_Name;
    public TMP_Text Text_Stat;
    public GameObject Button_Take;
    public Sprite Sprite_Take;
    public Sprite Sprite_UnTake;
    public TMP_Text Text_Take;
    public int Taking_SlotNumber = -1;

    public GameObject Weapon_Slot;
    public GameObject Panel_WeapnSlot;
    public const int SLOT_SIZE = 18;
    public Transform[] SlotTransform = new Transform[SLOT_SIZE];
  
    DataManager dataManager;
    public WeaponUnit WeaponUnit;

    [Header("Delet")]
    public GameObject Delet_Stay;
    public GameObject Delet_Icon;
    public GameObject Delet_Icon_open;
    public GameObject Delet_Menu;
    public Button DeletButton;

    [Header("AddSlot")]
    public GameObject buttonAddSlot;

    void Start()
    {
        dataManager = GameManager.Instance.dataManager;

        AllCreatSlot(Weapon_Slot);
        if (dataManager != null)
        {
            ActiveSlot(dataManager);
            LoadSaveData(dataManager);
            WeaponHaveCheck(dataManager);
            WeaponTakeCheck(dataManager);
        }
    }
    void AllCreatSlot(GameObject _weapon_Slot)
    {
        for (int i = 0; i < SLOT_SIZE; i++)
        {
            _weapon_Slot.GetComponent<Menu_WeaponSlot>().iSlotIndex = i;
            Instantiate(_weapon_Slot).transform.parent = Panel_WeapnSlot.transform;
            SlotTransform[i] = Panel_WeapnSlot.transform.GetChild(i);
            SlotTransform[i].localPosition = Vector3.zero;
            SlotTransform[i].localScale = Vector3.one;
        }
    }
    public void ActiveSlot(DataManager _dataManager)
    {
        for (int i = 0; i < _dataManager.data.iSlot; i++)
            SlotTransform[i].gameObject.SetActive(true);


        if (_dataManager.data.iSlot >= SLOT_SIZE)
            Destroy(buttonAddSlot);

        if(_dataManager.data.iSlot < SLOT_SIZE)
        {
            SlotTransform[_dataManager.data.iSlot].gameObject.SetActive(true);
            buttonAddSlot.transform.parent = SlotTransform[_dataManager.data.iSlot];
            buttonAddSlot.transform.localPosition = Vector3.zero;
        }
    }
    public void LoadSaveData(DataManager _dataManager)
    {
        if (_dataManager.data.WeaponDatas != null)
            for (int i = _dataManager.data.WeaponDatas.Count - 1; i >= 0; --i)
            {
                WeaponDataCopy(dataManager, i);
                CreateWeapon(WeaponUnit);
            }
    }

    void WeaponDataCopy(DataManager _dataManager, int _index)
    {
        WeaponUnit.WeaponData = WeaponUnit.WeaponDatas[_dataManager.data.WeaponDatas[_index].WeaponData_Code];
        WeaponUnit.level = _dataManager.data.WeaponDatas[_index].iLevel;
        WeaponUnit.Weapon_SlotNumber = _dataManager.data.WeaponDatas[_index].Weapon_SlotNumber;

        Weapon weapon = WeaponUnit.WeaponData.GetComponent<Weapon>();
        WeaponUnit.name = weapon.WeaponData.name;
        WeaponUnit.iPower = weapon.WeaponData.iPower;
        WeaponUnit.fRange = weapon.WeaponData.fRange;
        WeaponUnit.iKnockBack = weapon.WeaponData.iKnockBack;
        WeaponUnit.fCooltime = weapon.WeaponData.fCooltime;
    }
    public void BuyWeapon(int _weaponDataIndex)
    {
        WeaponDataCopy(dataManager, _weaponDataIndex);
        CreateWeapon(WeaponUnit);
        Menu_CapsuleMachine.GetItem = WeaponUnit.gameObject;
    }
    void CreateWeapon(WeaponUnit _weaponUnit)
    {
        Instantiate(_weaponUnit).transform.parent = SlotTransform[_weaponUnit.Weapon_SlotNumber];
        _weaponUnit = SlotTransform[_weaponUnit.Weapon_SlotNumber].GetChild(0).GetComponent<WeaponUnit>();
        Instantiate(_weaponUnit.Weapon_SlotUI).transform.parent = SlotTransform[_weaponUnit.Weapon_SlotNumber];
        Menu_WeaponSlotUI weaponInterface = SlotTransform[_weaponUnit.Weapon_SlotNumber].GetChild(1).GetComponent<Menu_WeaponSlotUI>();
        _weaponUnit.WeaponData.layer = 9;
        Instantiate(_weaponUnit.WeaponData, weaponInterface.UI_Mesh_Pos);
        Transform weapon_slotui_transform = SlotTransform[_weaponUnit.Weapon_SlotNumber].GetChild(1);
        weapon_slotui_transform.localPosition = Vector3.zero;
        weapon_slotui_transform.transform.localScale = Vector3.one;
        _weaponUnit.gameObject.SetActive(false);
    }
    public void Select(Menu_WeaponSlot weaponSlot ,WeaponUnit weaponUnit , Menu_WeaponSlotUI weaponInterface)
    {
        WeaponInterfaceReset(dataManager);
        Taking_SlotNumber = -1;
        if (!weaponUnit && !weaponInterface)
        {
            SelectReset(weaponInterface);
            Taking_SlotNumber = -1;
            StatePanel.SetActive(false);
        }
        else if (weaponUnit && weaponInterface)
        {
            StatePanel.SetActive(true);
            SelectReset(weaponInterface);
            if (weaponInterface) WeaponInterfaceSelect(weaponInterface, true);
            Instantiate(weaponUnit.WeaponData).transform.parent = SelectWeaponView.transform;
            GameObject weapon_mesh = SelectWeaponView.transform.GetChild(SelectWeaponView.transform.childCount-1).gameObject;
            weapon_mesh.layer = 9;
            weapon_mesh.transform.localPosition = new Vector3(0, 0, 0);
            weapon_mesh.transform.localRotation = new Quaternion(0, 0, 0, 0);
            weapon_mesh.transform.localScale = new Vector3(300, 300, 300);
            SelectWeaponView.transform.localRotation = Quaternion.Euler(0, 60, 0);
            Text_Name.text = weaponUnit.name.ToString();
            Text_Stat.text =
                "공격력 : " + weaponUnit.iPower +
                "\n쿨타임 : " + weaponUnit.fCooltime +
                "\n사거리 : " + weaponUnit.fRange +
                "\n넉백\t : " + weaponUnit.iKnockBack;
            Taking_SlotNumber = weaponSlot.iSlotIndex;
            ChangTakeButtonSprite(dataManager);
            Button_Take.SetActive(true);
        }
    }

    void SelectReset(Menu_WeaponSlotUI weaponInterface)
    {
        if (weaponInterface)
            WeaponInterfaceSelect(weaponInterface, false);
        if (SelectWeaponView.transform.childCount != 0)
            Destroy(SelectWeaponView.transform.GetChild(0).gameObject);
        Text_Name.text = "";
        Text_Stat.text = "";
        Button_Take.SetActive(false);
    }

    void ChangTakeButtonSprite(DataManager _dataManager)
    {
        int index = 0;
        if (_dataManager.data.WeaponDatas.Count > 0)
            for (int i = 0; i < _dataManager.data.WeaponDatas.Count; i++)
                if (_dataManager.data.WeaponDatas[i].Weapon_SlotNumber == Taking_SlotNumber)
                {
                    index = i;
                    break;
                }

        if (_dataManager.data.HaveWeapon.Weapon_SlotNumber == _dataManager.data.WeaponDatas[index].Weapon_SlotNumber)
        {
            Button_Take.GetComponent<Image>().sprite = Sprite_UnTake;
            Text_Take.text = "해제";
        }
        else
        {
            Button_Take.GetComponent<Image>().sprite = Sprite_Take;
            Text_Take.text = "장착";
        }
    }
    public void WeaponTakeButton()
    {
        GameManager_Sound.instance.ButtonClick();
        if (dataManager)
        {
            int index = 0;
            if (dataManager.data.WeaponDatas.Count > 0)
                for (int i = 0; i < dataManager.data.WeaponDatas.Count; i++)
                    if (dataManager.data.WeaponDatas[i].Weapon_SlotNumber == Taking_SlotNumber)
                    {
                        index = i;
                        break;
                    }

            if (dataManager.data.HaveWeapon.Weapon_SlotNumber == dataManager.data.WeaponDatas[index].Weapon_SlotNumber)
                dataManager.data.HaveWeapon = new WeaponData();//기존 항목 지우기
            else
            {
                GameManager_Sound.instance.ButtonWeapon();
                dataManager.data.HaveWeapon = dataManager.data.WeaponDatas[index];//장착 무기
            }

            ChangTakeButtonSprite(dataManager);

            GameManager.Instance.Player.GetComponent<Player>().WeaponDataLoed(dataManager);
            dataManager.SaveGameData();

            WeaponTakeCheck(dataManager);
        }
    }
    void WeaponHaveCheck(DataManager _dataManager)
    {
        if (_dataManager.data.HaveWeapon != null && _dataManager.data.HaveWeapon.WeaponData_Code >= 0)
            Taking_SlotNumber = _dataManager.data.HaveWeapon.Weapon_SlotNumber;
        else
            Taking_SlotNumber = -1;
    }

    void WeaponTakeCheck(DataManager _dataManager)
    {
        int index = 0;//위치를 바꾸면 전부 인덱스0을 가리킴
        if (_dataManager.data.WeaponDatas.Count > 0)
            for (int i = 0; i < _dataManager.data.WeaponDatas.Count; i++)
                if (_dataManager.data.WeaponDatas[i].Weapon_SlotNumber == Taking_SlotNumber)
                {
                    index = i;
                    break;
                }

        if (_dataManager.data.WeaponDatas.Count > 0)
        {
            for (int i = 0; i < _dataManager.data.WeaponDatas.Count; i++)
            {
                _dataManager.data.WeaponDatas[i].isTake = false;
                if (SlotTransform[_dataManager.data.WeaponDatas[i].Weapon_SlotNumber].childCount > 0)
                    SlotTransform[_dataManager.data.WeaponDatas[i].Weapon_SlotNumber].GetChild(1).GetComponent<Menu_WeaponSlotUI>().Take.SetActive(false);
            }

            if (_dataManager.data.HaveWeapon.WeaponData_Code >= 0)
            {
                _dataManager.data.WeaponDatas[index].isTake = true;
            }

            for (int i = 0; i < _dataManager.data.WeaponDatas.Count; i++)
                if (_dataManager.data.WeaponDatas[i].isTake)
                    if (SlotTransform[_dataManager.data.WeaponDatas[i].Weapon_SlotNumber].childCount > 0)
                        SlotTransform[_dataManager.data.WeaponDatas[i].Weapon_SlotNumber].GetChild(1).GetComponent<Menu_WeaponSlotUI>().Take.SetActive(true);
        }
    }

    public void WeaponInterfaceSelect(Menu_WeaponSlotUI weaponInterface, bool isSelect)
    {
        weaponInterface.SetSelectImage(isSelect);
    }
    void WeaponInterfaceReset(DataManager _dataManager)
    {
        for (int i = 0; i < _dataManager.data.iSlot; i++)
            if (SlotTransform[i].childCount != 0)
            {
                Menu_WeaponSlotUI weaponInterface = SlotTransform[i].GetComponent<Menu_WeaponSlot>().weaponInterface;
                if (weaponInterface)
                    WeaponInterfaceSelect(weaponInterface, false);
            }
    }
    void Update()
    {
        SelectWeaponView.transform.Rotate(new Vector3(0, -fRotSpeed * Time.deltaTime, 0));
    }


    public void DeletIconState(bool isActive, bool isOpen)
    {
        Delet_Stay.SetActive(isActive);
        Delet_Icon_open.SetActive(isOpen);
        Delet_Icon.SetActive(!isOpen);
    }
    public void DeletMenuSetActiveFales()
    {
        GameManager_Sound.instance.ButtonClick();
        Delet_Menu.SetActive(false);
    }
}
