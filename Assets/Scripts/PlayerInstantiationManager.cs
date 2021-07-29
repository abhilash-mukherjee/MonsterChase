using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInstantiationManager : MonoBehaviour
{
    public void OnLevelAnimationPlayed()
    {
        GameManager.Instance.levelIndexAnimationPlayed = true;
        gameObject.SetActive(false);
    }
}
