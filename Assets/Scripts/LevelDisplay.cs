using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



public class LevelDisplay : MonoBehaviour
{
    [SerializeField]
    private Text levelText;
    void OnEnable()
    {
        
        levelText.text = GameManager.Instance.levelIndex.ToString();
        
    }

   
}
