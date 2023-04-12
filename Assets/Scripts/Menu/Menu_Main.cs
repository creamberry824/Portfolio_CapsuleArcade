using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Menu_Main : MonoBehaviour
{
    public Scrollbar scrollbar;
    public Transform ContentScrollTr;

    public Slider tabSlider;
    public RectTransform[] BtnRect , BtnImageRect;

    const int SIZE = 3; //메인메뉴 패널 갯수
    float[] pos = new float[SIZE]; //패널 위치
    float distance , targetPos; 
    int targetIndex;
    int startIndex = 2;

    void Start()
    {
        distance = 1f / (SIZE - 1);

        for (int i = 0; i < SIZE; i++)  //탭 위치 초기화
            pos[i] = distance * i;

        targetPos = distance * (startIndex - 1);
        targetIndex = (startIndex - 1);
        scrollbar.value = targetPos;
    }

    void Update()
    {
        tabSlider.value = scrollbar.value;
        scrollbar.value = Mathf.Lerp(scrollbar.value, targetPos, 0.4f);

        for (int i = 0; i < SIZE; i++)
            BtnRect[i].sizeDelta = new Vector2(i == targetIndex ? 450 : 315, BtnRect[i].sizeDelta.y); //탭 크기 변경

        for (int i = 0; i < SIZE; i++) //탭 핸들 이동 변경
        {
            Vector3 BtnTargetPos = BtnRect[i].anchoredPosition3D;
            Vector3 BtnTargetScale = Vector3.one;

            if (i == targetIndex)
                BtnTargetScale = new Vector3(1.1f, 1.1f, 1f);

            BtnImageRect[i].anchoredPosition3D = Vector3.Lerp(BtnImageRect[i].anchoredPosition3D, BtnTargetPos, 0.25f);
            BtnImageRect[i].localScale = Vector3.Lerp(BtnImageRect[i].localScale, BtnTargetScale, 0.25f);
        }
    }
    public void TabClick(int n) //탭 버튼을 통한 이동
    {
        GameManager_Sound.instance.ButtonClick();
        targetIndex = n;
        targetPos = pos[n];

        for (int i = 0; i < SIZE; i++)
            if (ContentScrollTr.GetChild(i).GetComponent<Menu_Scroll>() && targetPos == pos[i])
                ContentScrollTr.GetChild(i).GetChild(1).GetComponent<Scrollbar>().value = 1;
    }

}
