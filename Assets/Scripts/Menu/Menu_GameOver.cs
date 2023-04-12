using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu_GameOver : MonoBehaviour
{
    public void GotoMainMenu()
    {
        GameManager_Sound.instance.ButtonClick();
        GameManager_Loading.Instance.LoadScene("MainMenu");
    }
}
