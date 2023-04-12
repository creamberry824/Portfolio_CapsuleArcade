using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Menu_WeaponSlotUI : MonoBehaviour
{
    public WeaponUnit weapon;

    public Sprite Sprite_Select;
    public Sprite Sprite_UnSelect;
    public Sprite Sprite_Take;
    public GameObject Take;
    public Image ButtonTake_Image;
    Image Button_Image;
    public TMP_Text TMP_level;
    public TMP_Text TMP_Name;
    public Transform UI_Mesh_Pos;

    public bool isSelect = false;

    void Start()
    {
        Button_Image = GetComponent<Image>();
        if (TMP_level) TMP_level.text = "Lv" + weapon.level.ToString();
        if (TMP_Name) TMP_Name.text = weapon.name;
    }

    public void SetSelectImage(bool _isSelect)
    {
        if (_isSelect)
        {
            isSelect = true;
            Button_Image.sprite = Sprite_Select;
            ButtonTake_Image.sprite = Sprite_Select;
        }
        else
        {
            isSelect = false;
            Button_Image.sprite = Sprite_UnSelect;
            ButtonTake_Image.sprite = Sprite_Take;
        }
    }
}
