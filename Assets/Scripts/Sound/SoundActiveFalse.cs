using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundActiveFalse : MonoBehaviour
{
    //풀링을 위한 오디오 오브젝트
    AudioSource audioSource;
    private void OnEnable()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = GameManager.Instance.dataManager.data.fSoundEffect;
        StartCoroutine(SetActivefalse());
        audioSource.Play();
    }
    IEnumerator SetActivefalse()
    {
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
    }
}
