using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class Menu_Stage : MonoBehaviour, IDragHandler, IEndDragHandler
{
    [Header("Canves")]
    public GameObject canvesMainMenu;
    public GameObject canvesMainMenuOut;
    public GameObject canvesStageMenu;

    [Header("Scrollbar")]
    public Scrollbar scrollbar;
    //public Slider tabSlider;
    public GameObject StageMenu;
    const int SIZE = 30; //const 변경 불가 함수(상수)
    float[] pos = new float[SIZE]; //각 panel 위치 기억
    public RectTransform[] BtnRect;
    float distance, targetPos;
    int targetIndex = 0;
    int ButtonIndex = 0;
    bool isDrag;

    [Header("SpriteSlelet")]
    public Sprite spriteSlelet;
    public Sprite spriteUnSlelet;

    DataManager dataManager;
    int iStageLevel = 1;
    int iPassTicket = 0;
    public TextMeshProUGUI PassTicket;

    [Header("StageMessage")]
    public TextMeshProUGUI Text_StageMessage;
    float fStageMessageTimer, fStageMessageTimerMax = 2.0f;



    private void Start()
    {
        dataManager = FindAnyObjectByType<DataManager>();
        LoadStageLevel(dataManager);
        ScrollbarReset();
        ButtonIneractable();
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
    void LoadStageLevel(DataManager _dataManager)
    {
        if (_dataManager)
            iStageLevel = _dataManager.data.iStageLevel;
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
        for (int i = 0; i < iStageLevel; i++)
            BtnRect[i].gameObject.GetComponent<Button>().interactable = true;
    }
    #region Scrollbar
    public void OnDrag(PointerEventData eventData) => isDrag = true;
    public void OnEndDrag(PointerEventData eventData) //스냅, 버튼 위치로자동 이동
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
            else if (scrollbar.value < pos[iStageLevel-1])
            {
                targetPos = pos[iStageLevel -1];
                targetIndex = iStageLevel -1;
            }
    }

    void ScrollbarReset() //버튼 위치 지정
    {
        BtnRect = new RectTransform[SIZE];
        for (int i = 0; i < SIZE; i++)//버튼 참조
            BtnRect[i] = StageMenu.transform.GetChild(i).GetComponent<RectTransform>();

        distance = 1f / (SIZE - 1);//총 panel개수 분에 1
        for (int i = 0; i < SIZE; i++)//위치 초기화
            pos[(SIZE-1) - i] = distance * i;

        for (int i = iStageLevel; i < SIZE; i++)
            pos[i] = distance * iStageLevel;

        targetPos = pos[0];
    }
    void ScrollbarMove()
    {
        //지정된 위치로 이동
        if(!isDrag) scrollbar.value = Mathf.Lerp(scrollbar.value, targetPos, 0.5f);

        for (int i = 0; i < SIZE; i++) //이동 위치가 중간일때
            if (scrollbar.value < pos[i] + distance * 0.5f && scrollbar.value > pos[i] - distance * 0.1f)
                targetIndex = i;

        for (int i = 0; i < SIZE; i++) //크기및 활성화 조절
        {
            BtnRect[i].gameObject.transform.GetChild(1).GetComponent<Image>().color = new Color32(50, 50, 50, 225);
            BtnRect[i].gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = new Color32(225,225,225,0);
            if (BtnRect[i].gameObject.GetComponent<Button>().interactable)
            {
                BtnRect[i].gameObject.transform.GetChild(1).GetComponent<Image>().color = new Color(0, 0, 0, 0);
                BtnRect[i].gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.white;
                BtnRect[i].sizeDelta = Vector2.Lerp(BtnRect[i].sizeDelta, new Vector2(BtnRect[i].sizeDelta.x, i == targetIndex ? 330 : 220), 0.3f);
                BtnRect[i].gameObject.GetComponent<Image>().sprite = (i == targetIndex ? spriteSlelet : spriteUnSlelet);
            }

            if (i == targetIndex)
                BtnRect[i].gameObject.transform.GetChild(2).gameObject.SetActive(true);
            else
                BtnRect[i].gameObject.transform.GetChild(2).gameObject.SetActive(false);
        }

        if(targetIndex != ButtonIndex)
        {
            GameManager_Sound.instance.ButtonDrag();
            ButtonIndex = targetIndex;
        }

    }

    #endregion
    #region Button
    public void PlayGame(int _stage) //버튼 위치가 현재 위치가 아니라면 이동, 맞다면 스테이지로 이동
    {
        GameManager_Sound.instance.ButtonDrag();

        targetPos = pos[_stage - 1];

        if (targetIndex == _stage - 1)
        {
            GameManager_Sound.instance.ButtonClick();
            if (dataManager)
            {
                if (dataManager.data.iPassTicket <= 0)
                    StageMessage("패스권이 부족합니다!", Color.red);
                else
                {
                    dataManager.data.iPassTicket -= 1;
                    GameManager_Stage.iStageLevel = _stage;
                    GameManager_Loading.Instance.LoadScene("InGame");
                }
                dataManager.SaveGameData();
            }
        }
    }
    public void MoveToLastStageButton()
    {
        targetPos = pos[iStageLevel - 1];
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
    void StageMessage(string _message, Color _color)
    {
        fStageMessageTimer = fStageMessageTimerMax;
        Text_StageMessage.text = _message;
        Text_StageMessage.color = _color;
        Text_StageMessage.gameObject.SetActive(true);
    }
}
