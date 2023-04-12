using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Coin : MonoBehaviour
{
    [Header("Option")]
    public Transform Model; //코인 모델

    [HideInInspector] public GameObject parent; //코인을 드랍할 Enemy
    public static int iCoinPrice = 50; //가격
    float fGetPositionSpeed; //이동 속도
    bool isGet = false; //플레이어게 이동할지 여부
    Player player;

    private void OnEnable()
    {
        player = GameManager.Instance.Player.GetComponent<Player>();
        transform.localPosition = Vector3.zero;
        fGetPositionSpeed = 40.0f;
        isGet = false;
        Vector3 dumi = transform.position;
        dumi.y = 1;
        transform.position = dumi;
        transform.rotation = Quaternion.Euler(0, Random.Range(1, 37) * 10, 0);
        Model.transform.rotation = Quaternion.Euler(0, Random.Range(1, 37) * 10, 0);
        parent = transform.parent.gameObject;
        transform.localScale = Vector3.one;
        transform.parent = null;
    }
    private void Update()
    {
        if (isGet)
            MoveToPlayer();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
            isGet = true;
    }
    void MoveToPlayer() //플레이어에게 이동
    {
        transform.rotation = Quaternion.LookRotation(player.transform.position - transform.position);
        transform.Translate(Vector3.forward * fGetPositionSpeed * Time.deltaTime);
        if (Vector3.Distance(transform.position, player.transform.position) < 10)
            fGetPositionSpeed += 2.0f;
        if (Vector3.Distance(transform.position, player.transform.position) <= 1f)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, 10f);
            if (player.isGetCoinReady)
                GetCoin();
        }
    }
    public void GetCoin() //플레이어에게 이동 후 보유 코인 증가 호출
    {
        isGet = false;
        player.GetCoin(Random.Range(1, iCoinPrice + 1));
        transform.SetParent(parent.transform);
    }
}
