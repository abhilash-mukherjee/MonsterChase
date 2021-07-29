using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonManager : MonoBehaviour
{
    private bool isPlaying = true;
    [SerializeField]
    private Animator anim;
    private void Awake()
    {
        anim.SetBool("Button_Play", true);
    }
    private void Update()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            if (EventSystem.current.IsPointerOverGameObject() == true)
            {
                if (EventSystem.current.currentSelectedGameObject.name == "PlayPauseButton")
                {
                    if (isPlaying == true)
                    {
                        anim.SetBool("Button_Play", false);
                        PauseGame();
                        isPlaying = false;
                    }
                    else
                    {
                        anim.SetBool("Button_Play", true);
                        PlayGame();
                        isPlaying = true;                       
                    }
                }

            }
        }
        
    }

    private void PauseGame()
    {
        Time.timeScale = 0;
        AudioListener.pause = true;
    }

    private void PlayGame()
    {
        Time.timeScale = 1;
        AudioListener.pause = false;
    }
}
