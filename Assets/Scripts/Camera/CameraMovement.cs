using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    GameObject Player;
    Camera camera;
    Vector3 CameraPosition;
    Vector3 CameraArmPosition;
    float fCameraArmPosY = 15f;
    float fCameraArmPosZ = -15f;



    void Start()
    {
        Player = GameManager.Instance.Player;
        camera = Camera.main;
        CameraPosition = camera.transform.localPosition;
    }

    //카메라 Arm을 플레이어에 위치
    private void LateUpdate()
    {
        CameraArmPosition.x = Player.transform.position.x;
        CameraArmPosition.y = fCameraArmPosY;
        CameraArmPosition.z = Player.transform.position.z + fCameraArmPosZ;
        transform.position = CameraArmPosition;
    }

    //카메라 흔들기
    public IEnumerator Shake(float fTime, float magnitude) //시간, 힘
    {
        float timer = 0;
        while (timer <= fTime)//흔들리는 시간
        {
            camera.transform.localPosition = Random.insideUnitSphere * magnitude + CameraPosition;
            timer += Time.deltaTime;
            yield return null;
        }
        camera.transform.localPosition = CameraPosition; //원위치
    }
}
