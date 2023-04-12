using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIEffect_Color : MonoBehaviour
{
    Image imageSprit;
    TextMeshProUGUI text;
    Color TargetColor = Color.white;
    float fTimer = 0;
    float fTime = 0.4f;
    float fSpeed = 0.1f; 


    void Start()
    {
        imageSprit = GetComponent<Image>();
        text = GetComponent<TextMeshProUGUI>();
    }
    void Update()
    {
        if(imageSprit) imageSprit.color = Color.Lerp(imageSprit.color, TargetColor, fSpeed);
        if(text) text.color = Color.Lerp(text.color, TargetColor, fSpeed);

        fTimer += Time.deltaTime;
        if (fTimer > fTime)
        {
            TargetColor = TargetColor == Color.white ? new Color32(225, 225, 225, 0) : Color.white;
            fTimer = 0;
        }
    }
}
