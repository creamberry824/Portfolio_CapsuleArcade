using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Menu_Challenge : MonoBehaviour, IDragHandler, IEndDragHandler
{
    [Header("Canves")]
    public GameObject canvesMainMenu;
    public GameObject canvesMainMenuOut;
    public GameObject canvesStageMenu;

    [Header("Scrollbar")]
    public Scrollbar scrollbar;
    public GameObject StageMenu;
    const int SIZE = 3;
    float[] pos = new float[SIZE];
    public RectTransform[] BtnRect;
    float distance, targetPos;
    int targetIndex = 0;
    int ButtonIndex = 0;
    bool isDrag;

    [Header("SpriteSlelet")]
    public Sprite spriteSlelet;
    public Sprite spriteUnSlelet;


    [Header("StageMessage")]
    public TextMeshProUGUI Text_StageMessage;
    float fStageMessageTimer = 2.0f;

    public List<TextMeshProUGUI> Score;
    DataManager dataManager;
    int iPassTicket = 0;
    public TextMeshProUGUI PassTicket;


    private void Start()
    {
        dataManager = GameManager.Instance.dataManager;

        ScrollbarReset();
        ButtonIneractable();

        ScoreUpdate(dataManager);
    }
    private void OnEnable()
    {
        fStageMessageTimer = 0;
    }
    private void Update()
    {
        ScrollbarMove();
        LoadPassTicket(dataManager);

        if (fStageMessageTimer < 0)
            Text_StageMessage.gameObject.SetActive(false);
        else
            fStageMessageTimer -= Time.deltaTime;
    }

    void LoadPassTicket(DataManager _dataManager)
    {
        if (_dataManager)
        {
            iPassTicket = _dataManager.data.iPassTicket;
            if (PassTicket)
                PassTicket.text = iPassTicket.ToSafeString();
        }
    }
    void ButtonIneractable()
    {
        for (int i = 0; i < SIZE; i++)//버튼 활성화
            BtnRect[i].gameObject.GetComponent<Button>().interactable = true;
    }
    #region Scrollbar
    public void OnDrag(PointerEventData eventData) => isDrag = true;
    public void OnEndDrag(PointerEventData eventData)
    {
        isDrag = false;
        for (int i = 0; i < SIZE; i++)
            if (scrollbar.value < pos[i] + distance * 0.5f && scrollbar.value > pos[i] - distance * 0.5f)
            {
                targetPos = pos[i];
                targetIndex = i;
            }
            else if (scrollbar.value > pos[0])
            {
                targetPos = pos[0];
                targetIndex = 0;
            }
            else if (scrollbar.value < pos[SIZE-1])
            {
                targetPos = pos[SIZE - 1];
                targetIndex = SIZE - 1;
            }
    }
    void ScrollbarReset()
    {
        BtnRect = new RectTransform[SIZE];
        for (int i = 0; i < SIZE; i++)//버튼 참조
            BtnRect[i] = StageMenu.transform.GetChild(i).GetComponent<RectTransform>();

        distance = 1f / (SIZE - 1);//총 panel개수 분에 1
        for (int i = 0; i < SIZE; i++)//위치 초기화
            pos[(SIZE - 1) - i] = distance * i;

        targetPos = pos[0];
    }
    void ScrollbarMove()
    {
        //슬라이드 이동
        if (!isDrag) scrollbar.value = Mathf.Lerp(scrollbar.value, targetPos, 0.5f);

        for (int i = 0; i < SIZE; i++)
            if (scrollbar.value < pos[i] + distance * 0.5f && scrollbar.value > pos[i] - distance * 0.1f)
                targetIndex = i;

        for (int i = 0; i < SIZE; i++)
        {
            if (i == targetIndex)
            {
                BtnRect[i].gameObject.transform.GetChild(1).gameObject.SetActive(true);
                BtnRect[i].gameObject.transform.GetChild(2).gameObject.SetActive(false);

            }
            else
            {
                BtnRect[i].gameObject.transform.GetChild(1).gameObject.SetActive(false);
                BtnRect[i].gameObject.transform.GetChild(2).gameObject.SetActive(true);

            }
        }

        if (targetIndex != ButtonIndex)
        {
            GameManager_Sound.instance.ButtonDrag();
            ButtonIndex = targetIndex;
        }

    }
    #endregion
    #region Button
    public void PlayGame(int _stage)
    {
        GameManager_Sound.instance.ButtonDrag();

        targetPos = pos[_stage - 1];

        if (targetIndex == _stage - 1)
        {
            GameManager_Sound.instance.ButtonClick();
            GameManager_Stage.iStageLevel = _stage;
            GameManager_Loading.Instance.LoadScene("InChallenge");
        }

    }
    public void GotoMainMenu()
    {
        scrollbar.value = 0;
        GameManager_Sound.instance.ButtonClick();
        canvesMainMenu.SetActive(true);
        canvesMainMenuOut.SetActive(true);
        canvesStageMenu.SetActive(false);
    }
    #endregion
    void ScoreUpdate(DataManager _dataManager)
    {
        for (int i = 0; i < 5; i++)
            if (dataManager.data.iChallengeScore.Count < 5)
                dataManager.data.iChallengeScore.Add(0);

        for (int i = 0; i < Score.Count; i++)
        {
            if (_dataManager.data.iChallengeScore.Count > i + 1)
                Score[i].text = _dataManager.data.iChallengeScore[i + 1].ToString();
        }
    }
}

