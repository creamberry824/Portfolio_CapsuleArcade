using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIEffect_ObjectHandler : MonoBehaviour, IDragHandler
{
    float rotateSpeed = 10;

    public void OnDrag(PointerEventData eventData)
    {
        float x = eventData.delta.x * Time.deltaTime * rotateSpeed;
        transform.Rotate(0, -x, 0, Space.World);
    }
}
