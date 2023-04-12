using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserCannon : MonoBehaviour
{
    Player player;
    private void Start() //플레이어찾기
    {
        player = GameManager.Instance.Player.GetComponent<Player>();
    }

    private void OnTriggerEnter(Collider other) //접촉시 플레이어에게 데미지
    {
        if (other.gameObject.CompareTag("Player"))
            player.Damage(10000);
    }
}
