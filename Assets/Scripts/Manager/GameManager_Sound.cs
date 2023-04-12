using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager_Sound : MonoBehaviour
{
    [Header("Music")]
    public AudioSource audioSoundMusic;
    public AudioClip Music_MainMenu;
    public AudioClip Music_InGame;
    public AudioClip Music_InChallenge;

    [Header("Button")]
    public AudioSource audioSoundButton;
    public AudioSource audioSoundButtonDrag;
    public AudioClip aclipButtonSelect;
    public AudioClip aclipButtonPurcha;
    public AudioClip aclipButtonWeapon;


    #region Singletone
    public static GameManager_Sound instance;
    void Singletone()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }
    #endregion
    private void Awake()
    {
        Singletone();

        if (SceneManager.GetActiveScene().name == "MainMenu")
            instance.ChangeMusic(Music_MainMenu);
        else if (SceneManager.GetActiveScene().name == "InGame")
            instance.ChangeMusic(Music_InGame);
        else if (SceneManager.GetActiveScene().name == "InChallenge")
            instance.ChangeMusic(Music_InChallenge);
    }
    void Start()
    {
        DataManager dataManager = GameManager.Instance.dataManager; //데이터를 불러옴
        if (dataManager)
        {
            audioSoundMusic.volume = dataManager.data.fSoundMusic; //볼륨 저장값 업데이트
            audioSoundButton.volume = dataManager.data.fSoundButton;
            audioSoundButtonDrag.volume = dataManager.data.fSoundButton;
        }
    }
    void ChangeMusic(AudioClip aclipMusic) //음악 변경
    {
        audioSoundMusic.clip = aclipMusic;
        audioSoundMusic.Play();
    }
    public void ButtonClick() => audioSoundButton.PlayOneShot(aclipButtonSelect);
    public void ButtonPurcha() => audioSoundButton.PlayOneShot(aclipButtonPurcha);
    public void ButtonWeapon() => audioSoundButton.PlayOneShot(aclipButtonWeapon);
    public void ButtonDrag() => audioSoundButtonDrag.Play();
}
