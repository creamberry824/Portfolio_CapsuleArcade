using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraResolution : MonoBehaviour
{
    //화면 비율 고정
    private void Awake()
    {
        Camera camera = GetComponent<Camera>(); //카메라 가져오기
        Rect rect = camera.rect; //렉트 가져오기
        float scaleheight = ((float)Screen.width / Screen.height) / ((float)9 / 16); //세로를 기준으로 비율 지정
        float scalewidth = 1f / scaleheight; //세로 비율에 맞게 가로 지정
        if(scaleheight < 1) //세로가 작다면 세로 길이 변경
        {
            rect.height = scaleheight;
            rect.y = (1f - scaleheight) / 2f; //조정
        }
        else //세로가 크다면 가로 길이 변경
        {
            rect.width = scalewidth;
            rect.x = (1f - scalewidth) / 2f; //조정
        }
        camera.rect = rect; //대입
    }
}
