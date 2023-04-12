using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager_Stage : MonoBehaviour
{
    [Header("UI")]
    public GameObject StageTitle;
    public GameObject BossTitle;
    public GameObject WinTitle;
    public GameObject GameOverMenu;
    public TextMeshProUGUI StageTextMesh;
    public TextMeshProUGUI KillCount;
      
    Player player;
    Pool_Enemy poolManager_Enemy;
    Enemy_Spawner spawner;
    DataManager dataManager;
    Camera mainCamera;
    Pool_DropMissile poolManager_DropMissile;


    [HideInInspector] public GameObject Boss = null;
    public static int iStageLevel = 1; //스테이지 레벨
    public static int iKillCount = 0; //제거한 Enemy 숫자
    public static int iBossKillCount = 0; //Boss제거 횟수
    bool isBoss = false; //보스 존제 여부
    bool isStageLive = false; //스테이지 진행 상황
    int iNeedKillCount = 100; //목표 킬 카운트
    int iBossNumber = -1; //생성될 보스 인덱스
    int iEnemyCoin = 5; //코인 가격
    int iEnemyHpMax = 1; //Enemy지정 Hp
    int iEnemyCountMax = 300; //Enemuy 밀집도
    int iEnemyKind = 1; //Enemy 종류


    [Header("Challger")]
    float fTimer_Sec = 0;
    float timer;
    int iTimer_Min = 0;
    int iTimerScore = 0;
    public static int iBossKillCountScore = 0;

    [Header("ChallgerUI")]
    public TextMeshProUGUI ScreenTimer;
    public TextMeshProUGUI BossKillCount;
    public GameObject objScreenTimer;
    public GameObject objKillCount;
    public GameObject objBossKillCount;

    private void Start()
    {
        iKillCount = 0;
        dataManager = GameManager.Instance.dataManager;

        player = GameManager.Instance.Player.GetComponent<Player>();
        poolManager_Enemy = GameManager.Instance.poolManager_Enemy;
        poolManager_DropMissile = GameManager.Instance.poolManager_DropMissile;
        spawner = FindAnyObjectByType<Enemy_Spawner>();
        mainCamera = Camera.main;
        StartCoroutine(StageTitlePlay(iStageLevel));



        if (SceneManager.GetActiveScene().name == "InGame")
            StageStart(iStageLevel);
        else ChallengeStart(iStageLevel);
    }

    void Update()
    {
        RunTimePlayerCheck(player);
        KillCountCheck();
        if (isStageLive)
            if (SceneManager.GetActiveScene().name == "InGame")
                StageLive();
            else ChallengeLive(iStageLevel);

            if (Boss != null)
            if (!Boss.activeSelf && RunTimePlayerCheck(player))
                if (isStageLive)
                    StartCoroutine(GameWinPlay());
    }

    string Timer()
    {
        fTimer_Sec += Time.deltaTime;
        iTimerScore = (int)fTimer_Sec;
        if (((int)fTimer_Sec > 59))
        {
            fTimer_Sec = 0;
            iTimer_Min++;
        }
        return string.Format("{0:D2}:{1:D2}", iTimer_Min , (int)fTimer_Sec);
    }

    #region Stage
    void StageStart(int iStageLevel)
    {
        Enemy.canDropCoin = true;
        EnemyBoss.canDropCoin = true;
        mainCamera.backgroundColor = new Color32(255, 255, 255, 255); //흰
        iEnemyHpMax = iStageLevel;
        if (1 <= iStageLevel && iStageLevel <= 10)
        {
            mainCamera.backgroundColor = new Color32(255, 165, 0, 255); //주황
            if (iStageLevel == 10)
                iBossNumber = 0;
        }
        else if (11 <= iStageLevel && iStageLevel <= 20)
        {
            mainCamera.backgroundColor = new Color32(110, 0, 190, 255); //보라
            iEnemyKind = 2;
            if (iStageLevel == 20)
                iBossNumber = 1;
        }
        else if (21 <= iStageLevel && iStageLevel <= 30)
        {
            mainCamera.backgroundColor = new Color32(100, 100, 100, 255); //회색
            iEnemyKind = 2;
            if (iStageLevel == 30)
                iBossNumber = 2;
        }
    }
    void StageLive()
    {
        #region UI확인
        if (StageTitle.activeSelf) return;
        #endregion
        EnemySpawn(!isBoss, iEnemyCountMax, iEnemyHpMax, iEnemyCoin, iEnemyKind);
        if (iKillCount >= iNeedKillCount)
            if (!isBoss)
            {
                EnemyClear();
                StartCoroutine(BossSpawn(iBossNumber));
                isBoss = true;
            }
    }
    #endregion
    #region Challenge
    void ChallengeStart(int iChallengeNumber)
    {
        Enemy.canDropCoin = false;
        EnemyBoss.canDropCoin = false;
        mainCamera.backgroundColor = new Color32(255, 255, 255, 255); //흰

        if (iChallengeNumber == 1)
        {
            mainCamera.backgroundColor = new Color32(110, 0, 190, 255); //보라
            objKillCount.SetActive(true);
            iNeedKillCount = 100;
        }
        if (iChallengeNumber == 2)
        {
            mainCamera.backgroundColor = new Color32(125, 0, 0, 255); //빨강
            objScreenTimer.SetActive(true);

        }
        if (iChallengeNumber == 3)
        {
            mainCamera.backgroundColor = new Color32(110, 0, 190, 255); //보라
            objBossKillCount.SetActive(true);
            iNeedKillCount = 1;
            iBossKillCount = 1;
        }
    }
    void ChallengeLive(int iChallengeNumber)
    {
        if(iChallengeNumber == 1)
        {
            if (iKillCount > iNeedKillCount)
            {
                iEnemyHpMax *= 2;
                iNeedKillCount += 100;
            }
            EnemySpawn(!isBoss, iEnemyCountMax, iEnemyHpMax, iEnemyCoin, iEnemyKind);
        }
        else if(iChallengeNumber == 2)
        {
            if (isStageLive)
            {
                if (ScreenTimer) ScreenTimer.text = Timer();
                poolManager_DropMissile.Drop();
            }
        }
        else if (iChallengeNumber == 3)
        {
            if (iBossKillCount >= iNeedKillCount)
            {
                Debug.Log(iBossKillCountScore);
                StartCoroutine(BossSpawn(iBossKillCountScore % 3));
                iBossKillCount = 0;
            }
        }
    }
    #endregion

    bool RunTimePlayerCheck(Player _player)
    {
        if (_player.isDeath && isStageLive)
        {
            GameOverPlay();
            return false;
        }
        else
            return true;
    }

    void KillCountCheck()
    {
        KillCount.text = iKillCount.ToString();
        if (BossKillCount)
            BossKillCount.text = iBossKillCountScore.ToString();
    }

    IEnumerator StageTitlePlay(int _stageLevel)
    {
        if(StageTitle != null) StageTitle.SetActive(true);

        if (SceneManager.GetActiveScene().name == "InGame")
            StageTextMesh.text = "스테이지" + _stageLevel.ToString();

        if (SceneManager.GetActiveScene().name == "InChallenge")
        {
            if (_stageLevel == 1)
                StageTextMesh.text ="무한의 적";
            if (_stageLevel == 2)
                StageTextMesh.text = "미사일 피하기";
            if (_stageLevel == 3)
                StageTextMesh.text = "보스만 사냥";

        }

        yield return new WaitForSeconds(3f);
        if (StageTitle != null) StageTitle.SetActive(false);
        isStageLive = true;
    }
    void GameOverPlay()
    {
        isStageLive = false;
        if (dataManager) dataManager.SaveGameData();
        GameOverMenu.SetActive(true);
        Debug.Log("게임오버");

        if (SceneManager.GetActiveScene().name == "InChallenge")
        {
            for (int i = 0; i < 5; i++)
                if (dataManager.data.iChallengeScore.Count < 5)
                    dataManager.data.iChallengeScore.Add(0);

            if (iStageLevel == 1)
            {
                if (dataManager.data.iChallengeScore[1] < iKillCount)
                    dataManager.data.iChallengeScore[1] = iKillCount;
                iKillCount = 0;
            }
            if (iStageLevel == 2)
            {
                if (dataManager.data.iChallengeScore[2] < iTimerScore)
                    dataManager.data.iChallengeScore[2] = iTimerScore;
            }
            if (iStageLevel == 3)
            {
                if (dataManager.data.iChallengeScore[3] < iBossKillCountScore)
                    dataManager.data.iChallengeScore[3] = iBossKillCountScore;
                iBossKillCountScore = 0;
            }

            dataManager.SaveGameData();
        }
    }
    IEnumerator GameWinPlay()
    {
        if (WinTitle != null)  WinTitle.SetActive(true);
        yield return new WaitForSeconds(3f);
        Menu_Fade.SetFade(false);
        yield return new WaitForSeconds(1f);
        if (iStageLevel < 30)
            iStageLevel++;
        if (dataManager != null)
        {
            if(dataManager.data.iStageLevel < iStageLevel)
            dataManager.data.iStageLevel = iStageLevel;
            dataManager.SaveGameData();
        }

        SceneManager.LoadScene("InGame");
    }

    public void EnemyClear()
    {
        for (int i = 0; i < poolManager_Enemy.transform.childCount; i++)
        {
            if (poolManager_Enemy.transform.GetChild(i).gameObject.activeSelf)
                poolManager_Enemy.transform.GetChild(i).GetComponent<Enemy_Health>().Damage(1000);
        }
    }
    void EnemySpawn(bool isSpawn, int iEnemyCountMax, int iEnemyMaxHp, int iCoinPrice , int iEnemyKind)
    {
        Coin.iCoinPrice = iCoinPrice;
        if (isSpawn)
        {
            timer += Time.deltaTime;
            if (timer > 0.2f)
            {
                if (spawner)
                    spawner.EnemySpawn(iEnemyCountMax, iEnemyMaxHp, iEnemyKind);
                timer = 0;
            }
        }
    }
    IEnumerator BossSpawn(int iBossNumber)
    {
        isBoss = true;
        if (iBossNumber < 0)
            StartCoroutine(GameWinPlay());
        else
        {
            if (BossTitle != null) BossTitle.SetActive(true);
            yield return new WaitForSeconds(2f);
            BossTitle.SetActive(false);

            if (spawner != null) Boss = spawner.BossSpawn(iBossNumber);
        }
    }
}
