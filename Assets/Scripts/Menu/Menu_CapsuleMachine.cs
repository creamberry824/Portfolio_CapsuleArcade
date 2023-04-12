using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Menu_CapsuleMachine : MonoBehaviour
{
    Animator animator;
    [HideInInspector] public string strItemName;
    [HideInInspector] public GameObject Item;
    public static GameObject GetItem;

    [Header("Particle")]
    public ParticleSystem Spark;
    public ParticleSystem Open;

    [Header("UI")]
    public GameObject Canvas_Menu_CapsuleMachine;
    public TMP_Text textMessage;
    public Transform ItemPosition;
    public GameObject modelMachine;
    public GameObject buttonClose;
    public GameObject buttonSkip;
    public GameObject buttonOpen;
    public MeshRenderer rendererButton;
    float fRendButtonTimer = 0;
    float fRendButtonTime = 0.5f;
    Color32 TargetColor = new Color32(225, 225, 225, 225);

    [Header("Audio")]
    public AudioClip aclipLever;
    public AudioClip aclipRoll;
    public AudioClip aclipCharging;
    public AudioClip aclipOpen;
    AudioSource audioSource;

    private void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = GameManager.Instance.dataManager.data.fSoundButton;
    }

    private void Update()
    {
        //RendererColorChange
        rendererButton.material.color = Color.Lerp(rendererButton.material.color, TargetColor, 0.1f);
        fRendButtonTimer += Time.deltaTime;
        if (fRendButtonTimer > fRendButtonTime)
        {
            TargetColor = TargetColor == Color.white ? Color.red : Color.white;
            fRendButtonTimer = 0;
        }
    }
    #region Button
    public void StartButton()
    {
        Canvas_Menu_CapsuleMachine.SetActive(true);
        animator.Play("Start");
        Spark.Play();
    }
    public void OpenCapsule()
    {
        GameManager_Sound.instance.ButtonClick();
        buttonOpen.SetActive(false);
        animator.Play("Open");
    }
    public void Skip()
    {
        modelMachine.SetActive(false);
        OpenCapsule();
    }
    public void CloseCapsuleMenu()
    {
        GameManager_Sound.instance.ButtonClick();
        buttonClose.SetActive(false);
        buttonSkip.SetActive(true);
        Destroy(Item);
        textMessage.gameObject.SetActive(false);

        Canvas_Menu_CapsuleMachine.SetActive(false);
        modelMachine.SetActive(true);
    }
    #endregion
    #region AnimationClip
    public void Clip_CapsuleOpen()
    {
        Open.Play();
        textMessage.text = "'" + GetItem.GetComponent<WeaponUnit>().name + "'" + " È¹µæ!";
        buttonClose.SetActive(true);
        buttonSkip.SetActive(false);
        Item = Instantiate(GetItem.GetComponent<WeaponUnit>().WeaponData, ItemPosition);
        textMessage.gameObject.SetActive(true);
    }
    public void Clip_OpenButtonVisibel() => buttonOpen.SetActive(true);
    public void Clip_MachineHide() => modelMachine.SetActive(false);
    public void ClipAudio_Lever() => audioSource.PlayOneShot(aclipLever);
    public void ClipAudio_Roll() => audioSource.PlayOneShot(aclipRoll);
    public void ClipAudio_Charging() => audioSource.PlayOneShot(aclipCharging);
    public void ClipAudio_Open() => audioSource.PlayOneShot(aclipOpen);
    #endregion

}
