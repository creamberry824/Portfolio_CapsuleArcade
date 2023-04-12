using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu_Pause : MonoBehaviour
{
    DataManager dataManager;
    public GameObject canvesOptionMenu;

    void Start()
    {
        dataManager = GameManager.Instance.dataManager;
    }
    public void PauseGame(GameObject PauseMenu)
    {
        GameManager_Sound.instance.ButtonClick();
        PauseMenu.SetActive(true);
        Time.timeScale = 0;
    }
    public void PlaybackeGame(GameObject PauseMenu)
    {
        GameManager_Sound.instance.ButtonClick();
        PauseMenu.SetActive(false);
        Time.timeScale = 1;
    }
    public void GotoMainMenu()
    {
        GameManager_Sound.instance.ButtonClick();
        if (dataManager) dataManager.SaveGameData();
        GameManager_Stage.iKillCount = 0;
        GameManager_Stage.iBossKillCountScore = 0;
        GameManager_Loading.Instance.LoadScene("MainMenu");
    }

    public void GotoOptionMenu()
    {
        GameManager_Sound.instance.ButtonClick();
        canvesOptionMenu.SetActive(true);
    }
}
