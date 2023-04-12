using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraResolution : MonoBehaviour
{
    //ȭ�� ���� ����
    private void Awake()
    {
        Camera camera = GetComponent<Camera>(); //ī�޶� ��������
        Rect rect = camera.rect; //��Ʈ ��������
        float scaleheight = ((float)Screen.width / Screen.height) / ((float)9 / 16); //���θ� �������� ���� ����
        float scalewidth = 1f / scaleheight; //���� ������ �°� ���� ����
        if(scaleheight < 1) //���ΰ� �۴ٸ� ���� ���� ����
        {
            rect.height = scaleheight;
            rect.y = (1f - scaleheight) / 2f; //����
        }
        else //���ΰ� ũ�ٸ� ���� ���� ����
        {
            rect.width = scalewidth;
            rect.x = (1f - scalewidth) / 2f; //����
        }
        camera.rect = rect; //����
    }
}
