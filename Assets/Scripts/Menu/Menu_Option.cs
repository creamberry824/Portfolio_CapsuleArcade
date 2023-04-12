using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menu_Option : MonoBehaviour
{
    DataManager dataManager;
    Animator animator;

    [Header("OptionStateBar")]
    public Scrollbar SoundMusicBar;
    public Scrollbar SoundEffectBar;
    public Scrollbar SoundButtonBar;

    [Header("ResetMenu")]
    public GameObject ResetDataCheckPanel;

    private void Start()
    {
        dataManager = FindObjectOfType<DataManager>();
        animator = GetComponent<Animator>();
        StartBarRest();
    }
    private void Update() //실시간으로 사운드 조절 및 볼륨 저장
    {
        if(dataManager)
        {
            dataManager.data.fSoundMusic = SoundMusicBar.value;
            dataManager.data.fSoundEffect = SoundEffectBar.value;
            dataManager.data.fSoundButton = SoundButtonBar.value;

            GameManager_Sound.instance.audioSoundMusic.volume = SoundMusicBar.value;
            GameManager_Sound.instance.audioSoundButton.volume = SoundButtonBar.value;
            GameManager_Sound.instance.audioSoundButtonDrag.volume = SoundButtonBar.value;
        }
    }
    #region Button
    public void OptionReset()
    {
        GameManager_Sound.instance.ButtonClick();
        if(dataManager)
        {
            SoundMusicBar.value = 0.5f;
            SoundEffectBar.value = 0.5f;
            SoundButtonBar.value = 0.5f;
            dataManager.SaveGameData();
        }
    }
    public void GotoMainMenu()
    {
        GameManager_Sound.instance.ButtonClick();
        if (SceneManager.GetActiveScene().name == "MainMenu")
            StartCoroutine(OffOptionMenu());
        else
        {
            if (dataManager) dataManager.SaveGameData();
            gameObject.SetActive(false);
        }
    }
    public void ResetDataPanelOn() => ResetDataCheckPanel.SetActive(true);
    public void ResetDataPanelOff() => ResetDataCheckPanel.SetActive(false);
    public void ResetData()
    {
        if (dataManager)
        {
            dataManager.data.strName = " ";
            dataManager.data.iSlot = 6;
            dataManager.data.iCoin = 0;
            dataManager.data.iStageLevel = 1;
            dataManager.data.iPassTicket = 3;
            dataManager.data.WeaponDatas.Clear();
            dataManager.data.HaveWeapon = null;

            dataManager.data.iChallengeScore.Clear();

            dataManager.data.fSoundMusic = 0.5f;
            dataManager.data.fSoundEffect = 0.5f;

            dataManager.SaveGameData();
            SceneManager.LoadScene(0);
        }
    }
    #endregion
    void StartBarRest() //볼륨 핸들 리셋
    {
        if (dataManager)
        {
            SoundMusicBar.value = dataManager.data.fSoundMusic;
            SoundEffectBar.value = dataManager.data.fSoundEffect;
            SoundButtonBar.value = dataManager.data.fSoundButton;
        }
    }
    IEnumerator OffOptionMenu()
    {
        if (dataManager) dataManager.SaveGameData();
        animator.SetBool("isOpen", false);
        yield return new WaitForSeconds(0.5f);
        animator.SetBool("isOpen", true);
        gameObject.SetActive(false);
    }
}
