using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Menu_Name : MonoBehaviour
{
    DataManager dataManager;
    Animator animator;
    public TMP_InputField nameInputField;

    void Start()
    {
        dataManager = GameManager.Instance.dataManager;
        animator = GetComponent<Animator>();

        if (dataManager) //�̸��� �ִ��� Ȯ��
            if (dataManager.data.strName != " ")
                gameObject.SetActive(false);
    }

    public void NameResolution(string _name) //�̸� ����
    {
        GameManager_Sound.instance.ButtonClick();
        if (dataManager)
        {
            dataManager.data.strName = nameInputField.text;
            dataManager.SaveGameData();
        }
        StartCoroutine(OffNameMenu());
    }

    IEnumerator OffNameMenu() //�̸��� ���ٸ� �˾�
    {
        if (dataManager) dataManager.SaveGameData();
        animator.SetBool("isOpen", false);
        yield return new WaitForSeconds(0.5f);
        animator.SetBool("isOpen", true);
        gameObject.SetActive(false);
    }
}
