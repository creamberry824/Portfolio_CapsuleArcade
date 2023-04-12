using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserCannon : MonoBehaviour
{
    Player player;
    private void Start() //�÷��̾�ã��
    {
        player = GameManager.Instance.Player.GetComponent<Player>();
    }

    private void OnTriggerEnter(Collider other) //���˽� �÷��̾�� ������
    {
        if (other.gameObject.CompareTag("Player"))
            player.Damage(10000);
    }
}
