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

    //ī�޶� Arm�� �÷��̾ ��ġ
    private void LateUpdate()
    {
        CameraArmPosition.x = Player.transform.position.x;
        CameraArmPosition.y = fCameraArmPosY;
        CameraArmPosition.z = Player.transform.position.z + fCameraArmPosZ;
        transform.position = CameraArmPosition;
    }

    //ī�޶� ����
    public IEnumerator Shake(float fTime, float magnitude) //�ð�, ��
    {
        float timer = 0;
        while (timer <= fTime)//��鸮�� �ð�
        {
            camera.transform.localPosition = Random.insideUnitSphere * magnitude + CameraPosition;
            timer += Time.deltaTime;
            yield return null;
        }
        camera.transform.localPosition = CameraPosition; //����ġ
    }
}
