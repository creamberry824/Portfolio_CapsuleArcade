using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu_Fade : MonoBehaviour
{
    static Animator animator;
    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    public static void SetFade(bool isOpen)
    {
        animator.SetBool("isOpen" , isOpen);
    }
}
