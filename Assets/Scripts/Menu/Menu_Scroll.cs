using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class Menu_Scroll : ScrollRect
{
    //탭 이동시 스크롤 위치를 초기화 하기 위한 코드
    public bool canScroll = true;
    public override void OnBeginDrag(PointerEventData eventData)
    {
        if (canScroll)
            base.OnBeginDrag(eventData);
    }
    public override void OnDrag(PointerEventData eventData)
    {
        if (canScroll)
            base.OnDrag(eventData);
    }
    public override void OnEndDrag(PointerEventData eventData)
    {base.OnEndDrag(eventData);
    }
}
