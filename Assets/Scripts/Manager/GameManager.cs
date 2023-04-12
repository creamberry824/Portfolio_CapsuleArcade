using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Player")]
    public GameObject Player_Prefab; //�÷��̾� ������Ʈ

    [HideInInspector] public Transform PlayerSpawnPoint; //�÷��̾ ������ ��ġ
    [HideInInspector] public GameObject Player; //�ٸ� ������Ʈ�� ������ �÷��̾�
    [HideInInspector] public Pool_Enemy poolManager_Enemy; 
    [HideInInspector] public Pool_DropMissile poolManager_DropMissile;
    [HideInInspector] public GameManager_Stage stageManager;
    [HideInInspector] public DataManager dataManager; //�̱������� �����Ͽ����� ���� ������ ���� ���Ӹ޴������� ����
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
        Application.targetFrameRate = 300;// ������ ����

        DataManager.Instance.LoadGameData();//�������� ����
        dataManager = DataManager.Instance;
    }

    void ConnectManager()
    {
        poolManager_Enemy = FindObjectOfType<Pool_Enemy>();
        poolManager_DropMissile = FindObjectOfType<Pool_DropMissile>();
        stageManager = FindObjectOfType<GameManager_Stage>();
    }

    public void SpawnPlayer() //�÷��̾� ����
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
