using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdsReset : MonoBehaviour
{
    AdsInitializer adsInitializer;
    RewardedAdsButton rewarded;
    void Start()
    {
        adsInitializer = GetComponent<AdsInitializer>();
        rewarded = GetComponent<RewardedAdsButton>();
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.S)) adsInitializer.OnInitializationComplete();

    }
}
