using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu_Home : MonoBehaviour
{
    [Header("Canves")]
    public GameObject canvesMainMenu;
    public GameObject canvesMainMenuOut;
    public GameObject canvesOptionMenu;
    public GameObject canvesStageMenu;
    public GameObject canvesChallengeMenu;

    [Header ("ChangeMode")]
    public MeshRenderer BackGround;
    public Image ChangeButtonIcon;
    public Image ChangeButtonImgae;
    public Image[] TapImage = new Image[2];
    public Image TapIcon;


    [Header ("ChangeModeStage")]
    public GameObject StageButton;
    public Material matStage;
    public Sprite StageImgae;
    public Sprite StageIcon;
    public Sprite TapStageIcon;
    public Sprite tapStageImgae;

    [Header("ChangeModeChallenge")]
    public GameObject ChallengeStageButton;
    public Material matChallenge;
    public Sprite ChallengeImgae;
    public Sprite ChallengeIcon;
    public Sprite TapChallengeIcon;
    public Sprite tapChallengeImgae;

    public void GotoStageMenu()
    {
        GameManager_Sound.instance.ButtonClick();
        canvesMainMenu.SetActive(false);
        canvesMainMenuOut.SetActive(false);

        if (StageButton.activeSelf)
            canvesStageMenu.SetActive(true);
        else
            canvesChallengeMenu.SetActive(true);
    }

    public void GotoOptionMenu()
    {
        GameManager_Sound.instance.ButtonClick();
        canvesOptionMenu.SetActive(true);
    }

    public void ChangeStage()
    {
        GameManager_Sound.instance.ButtonClick();

        StageButton.SetActive( StageButton.activeSelf ? false : true);
        ChallengeStageButton.SetActive(ChallengeStageButton.activeSelf ? false : true);

        BackGround.material = StageButton.activeSelf ? matStage : matChallenge;
        ChangeButtonImgae.sprite = StageButton.activeSelf ? ChallengeImgae : StageImgae;
        ChangeButtonIcon.sprite = StageButton.activeSelf ? ChallengeIcon : StageIcon;


        TapIcon.sprite = StageButton.activeSelf ? TapStageIcon : TapChallengeIcon;
        if (StageButton.activeSelf)
            for (int i = 0; i < TapImage.Length; i++)
                TapImage[i].sprite = tapStageImgae;
        else
            for (int i = 0; i < TapImage.Length; i++)
                TapImage[i].sprite = tapChallengeImgae;
    }
}
