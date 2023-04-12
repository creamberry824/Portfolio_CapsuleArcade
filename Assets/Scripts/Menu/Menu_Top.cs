using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Menu_Top : MonoBehaviour
{
    public TextMeshProUGUI Coin; //보유코인 출력
    public TextMeshProUGUI Name; //이름 출력

    DataManager dataManager;

    void Start()
    {
        dataManager = GameManager.Instance.dataManager;
        if (dataManager && Name) Name.text = dataManager.data.strName;
    }
    void Update()
    {
        if (dataManager)
        {
            if (Name) Name.text = dataManager.data.strName;
            if (dataManager.data.iCoin <= 0)
                Coin.text = "0";
            else
                Coin.text = GetThousandCommaText(dataManager.data.iCoin);
        }
    }

    string GetThousandCommaText(int data) => string.Format("{0:#,###}", data);
}
