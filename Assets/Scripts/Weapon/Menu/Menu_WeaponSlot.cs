using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Menu_WeaponSlot : MonoBehaviour, IPointerDownHandler
{
    public Menu_Weapon weaponselectMenu;
    public Transform SelectMenu;
    public Menu_WeaponSlotUI weaponInterface;
    WeaponUnit WeaponUnit;

    Menu_Scroll scrollScript;

    bool isButtonDown = false;
    float fButtonDownTime = 0f;

    GraphicRaycaster rayCaster;
    PointerEventData pointerEventData;
    EventSystem eventSystem;
    GameObject hitObject;

    public int iSlotIndex = -1;
    int iHoldSlotIndx = -1;
    DataManager dataManager;

    private void Start()
    {
        dataManager = GameManager.Instance.dataManager;
        weaponselectMenu = FindObjectOfType<Menu_Weapon>();
        scrollScript = FindObjectOfType<Menu_Scroll>();
        eventSystem = FindObjectOfType<EventSystem>();
        SelectMenu = GameObject.Find("Canvas_Menu_Select").transform;
        rayCaster = GameObject.Find("Canvas_Menu_Main").GetComponent<GraphicRaycaster>();
    }
    public void SelectSlot()
    {
        GameManager_Sound.instance.ButtonClick();

        if (weaponselectMenu && transform.childCount != 0)
        {
            weaponselectMenu.SelectWeaponView.transform.localRotation = Quaternion.identity;
            weaponInterface = gameObject.transform.GetChild(1).GetComponent<Menu_WeaponSlotUI>();
            WeaponUnit = gameObject.transform.GetChild(0).GetComponent<WeaponUnit>();
            if (weaponInterface)
                weaponselectMenu.Select(this, WeaponUnit, weaponInterface);
            else weaponselectMenu.Select(null, null, null);

        }
        else weaponselectMenu.Select(null, null, null);
    }



    private void Update()
    {
        if (isButtonDown)
            fButtonDownTime += Time.deltaTime;

        if (fButtonDownTime > 0.5f && weaponInterface != null && weaponInterface.isSelect) //�������� �ð��� Ư���ð� �̻��̶�� ���� Ȧ��
        {
            if (weaponInterface.gameObject.activeSelf)
                weaponselectMenu.DeletIconState(true, false);

            weaponInterface.gameObject.SetActive(false);
            SelectMenu.GetChild(0).gameObject.SetActive(true);
            SelectMenu.GetChild(0).GetChild(0).transform.position = Input.mousePosition;

            scrollScript.canScroll = false;
            TouchRay();
        }

        if (Input.GetKeyUp(KeyCode.Mouse0)) //��ġ�� ���� Ȧ�� ���
        {
            SelectMenu.GetChild(0).gameObject.SetActive(false);
            if (weaponInterface)
                weaponInterface.gameObject.SetActive(true);
            scrollScript.canScroll = true;
            fButtonDownTime = 0;
            isButtonDown = false;
        }
    }
    public void OnPointerDown(PointerEventData eventData) => isButtonDown = true; //Ȧ�� Ÿ�� ����

    void TouchRay() //�巡�� �Ǵ� ��ġ
    {
        pointerEventData = new PointerEventData(eventSystem);
        pointerEventData.position = Input.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();

        rayCaster.Raycast(pointerEventData, results);//�����ɽ�Ʈ ����


        if (results.Count > 0)
        {
            hitObject = results[0].gameObject;
            if (hitObject.CompareTag("Delet") && dataManager) //����Ʈ �������� ���� �ִٸ�
            {
                Hold_SlotIndx(dataManager);
                weaponselectMenu.DeletIconState(true, true);
                if (!dataManager.data.WeaponDatas[iHoldSlotIndx].isTake) //���� Ȯ��â ���
                    if (Input.GetKeyUp(KeyCode.Mouse0))
                    {
                        weaponselectMenu.Delet_Menu.SetActive(true);
                        weaponselectMenu.DeletButton.onClick.RemoveAllListeners();
                        weaponselectMenu.DeletButton.onClick.AddListener(DeletWeapon);
                        weaponselectMenu.DeletIconState(false, false);
                    }
            }
            else weaponselectMenu.DeletIconState(true, false);

            if (hitObject.CompareTag("Slot") && dataManager) //���� �������� ���� �ִٸ�
            {
                Hold_SlotIndx(dataManager);
                if (!dataManager.data.WeaponDatas[iHoldSlotIndx].isTake)
                    if (Input.GetKeyUp(KeyCode.Mouse0))
                        if (hitObject.GetComponent<Menu_WeaponSlot>().transform.childCount == 0) //���Կ� ������ ���ٸ� �̵�
                        {
                            dataManager.data.WeaponDatas[iHoldSlotIndx].Weapon_SlotNumber = hitObject.GetComponent<Menu_WeaponSlot>().iSlotIndex;
                            dataManager.SaveGameData();

                            hitObject.GetComponent<Menu_WeaponSlot>().weaponInterface = weaponInterface;
                            hitObject.GetComponent<Menu_WeaponSlot>().weaponInterface.gameObject.SetActive(false);
                            weaponselectMenu.Taking_SlotNumber = hitObject.GetComponent<Menu_WeaponSlot>().iSlotIndex;
                            weaponInterface = null;

                            transform.GetChild(0).parent = hitObject.transform;
                            transform.GetChild(0).parent = hitObject.transform;
                            hitObject.transform.GetChild(0).localPosition = new Vector3(0, 0, 0);
                            hitObject.transform.GetChild(1).localPosition = new Vector3(0, 0, 0);
                            hitObject.transform.GetChild(1).gameObject.SetActive(true);
                        }
            }
            if (Input.GetKeyUp(KeyCode.Mouse0)) weaponselectMenu.DeletIconState(false, false); //����Ʈ �������� ��Ȱ��
        }
        else if (Input.GetKeyUp(KeyCode.Mouse0)) weaponselectMenu.DeletIconState(false, false);

        void Hold_SlotIndx(DataManager _dataManager) //���� �ε��� Ȯ��
        {
            if (_dataManager.data.WeaponDatas.Count > 0)
                for (int i = 0; i < _dataManager.data.WeaponDatas.Count; i++)
                    if (_dataManager.data.WeaponDatas[i].Weapon_SlotNumber == iSlotIndex)
                    {
                        iHoldSlotIndx = i;
                        break;
                    }
        }
    }
    public void DeletWeapon() //���� ����
    {
        weaponselectMenu.Select(null, null, null);
        Destroy(transform.GetChild(0).gameObject);
        Destroy(transform.GetChild(1).gameObject);
        dataManager.data.WeaponDatas.RemoveAt(iHoldSlotIndx);
        dataManager.SaveGameData();
        weaponselectMenu.Delet_Menu.SetActive(false);
    }
}