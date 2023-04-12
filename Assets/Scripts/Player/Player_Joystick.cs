using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player_Joystick : MonoBehaviour
{
    public GameObject objHandle;
    public GameObject objJoystick;
    public Vector2 vecJoy;
    public Vector2 vecJoyRot;
    float stickRadius;

    Vector2 vecStickFirstPosition;
    Vector2 vecJoyStickFirstPosition;

    void Start()
    {
        stickRadius = objJoystick.gameObject.GetComponent<RectTransform>().sizeDelta.y / 8;
        vecJoyStickFirstPosition = objJoystick.transform.position;
    }
    public void PointDown()
    {
        objJoystick.transform.position = Input.mousePosition;
        objHandle.transform.position = Input.mousePosition;
        vecStickFirstPosition = Input.mousePosition;
    }
    public void Drag(BaseEventData baseEventData)
    {
        PointerEventData pointerEventData = baseEventData as PointerEventData;
        Vector2 DragPosition = pointerEventData.position;
        vecJoy = (DragPosition - vecStickFirstPosition).normalized;
        vecJoyRot = (DragPosition - vecStickFirstPosition).normalized;

        float stickDistance = Vector2.Distance(DragPosition, vecStickFirstPosition);

        if(stickDistance < stickRadius)
            objHandle.transform.position = vecStickFirstPosition + vecJoy.normalized * stickDistance;
        else
            objHandle.transform.position = vecStickFirstPosition + vecJoy.normalized * stickRadius;
    }
    public void Drop()
    {
        vecJoy = Vector2.zero;
        objJoystick.transform.position = vecJoyStickFirstPosition;
        objHandle.transform.localPosition = new Vector3(0, 0, 0);
    }
}
