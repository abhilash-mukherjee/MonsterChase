using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DisplayBulletCount : MonoBehaviour
{
    private TMPro.TextMeshProUGUI bulletsRemainingText;
    // Start is called before the first frame update
    void Start()
    {
        bulletsRemainingText = GetComponent<TMPro.TextMeshProUGUI>();
        bulletsRemainingText.text =GameManager.Instance.MaxBulletCountForLevel.ToString();
    }
    private void OnEnable()
    {
        Player.OnBulletFired += ChangeBulletCountOnBulletFired;
    }

    
    private void OnDisable()
    {
        Player.OnBulletFired -= ChangeBulletCountOnBulletFired;
    }

    private void ChangeBulletCountOnBulletFired(int remainingBullets)
    {
        bulletsRemainingText.text = remainingBullets.ToString();
    }

}
