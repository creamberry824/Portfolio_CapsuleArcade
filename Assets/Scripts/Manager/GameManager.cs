using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Player")]
    public GameObject Player_Prefab; //플레이어 오브젝트

    [HideInInspector] public Transform PlayerSpawnPoint; //플레이어를 생성할 위치
    [HideInInspector] public GameObject Player; //다른 컴포넌트와 공유할 플레이어
    [HideInInspector] public Pool_Enemy poolManager_Enemy; 
    [HideInInspector] public Pool_DropMissile poolManager_DropMissile;
    [HideInInspector] public GameManager_Stage stageManager;
    [HideInInspector] public DataManager dataManager; //싱글톤으로 구현하였지만 오류 방지를 위해 게임메니저에서 지정
    #region Singletone
    public static GameManager Instance;
    void Singletone()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }
    #endregion
    private void Awake()
    {
        Singletone();

        Instance.SpawnPlayer();
        Instance.ConnectManager();
        Application.targetFrameRate = 300;// 프레임 설정

        DataManager.Instance.LoadGameData();//저장파일 생성
        dataManager = DataManager.Instance;
    }

    void ConnectManager()
    {
        poolManager_Enemy = FindObjectOfType<Pool_Enemy>();
        poolManager_DropMissile = FindObjectOfType<Pool_DropMissile>();
        stageManager = FindObjectOfType<GameManager_Stage>();
    }

    public void SpawnPlayer() //플레이어 스폰
    {
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            PlayerSpawnPoint = GameObject.Find("PlayerPositionPoint").GetComponent<Transform>();
            if (PlayerSpawnPoint != null)
                Player = Instantiate(Player_Prefab, PlayerSpawnPoint.transform);
        }
        else
        {
            poolManager_Enemy = FindObjectOfType<Pool_Enemy>();
            PlayerSpawnPoint = GameObject.Find("PlayerPositionPoint").GetComponent<Transform>();
            if (PlayerSpawnPoint != null)
            {
                Player = Instantiate(Player_Prefab, PlayerSpawnPoint.transform);
                Player.transform.parent = null;
            }
        }
    }
}
