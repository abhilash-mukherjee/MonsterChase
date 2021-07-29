using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public void PlayGame()
    {
        GameManager.Instance.levelIndex = 1;
        SceneManager.LoadScene("Level");
        string clickedObjStr = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name;
        int clickedObj = int.Parse(clickedObjStr);
        GameManager.Instance.CharIndex = clickedObj;
        Debug.Log($"Clicked object is Player: {clickedObj})");
    }
}
