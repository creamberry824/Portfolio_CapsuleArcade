using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Menu_Intro : MonoBehaviour
{
    public TextMeshProUGUI textVersion;
    private void Start()
    {
        textVersion.text = "v" + Application.version; //���� ��������
    }
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Mouse0))
            GameManager_Loading.Instance.LoadScene("MainMenu");
    }
}
