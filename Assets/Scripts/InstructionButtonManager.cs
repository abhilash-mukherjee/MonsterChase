using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InstructionButtonManager : MonoBehaviour
{
    private bool isActive = false;
    [SerializeField]
    private GameObject instructionPanel;
    [SerializeField]
    private GameObject buttonSelected;
    [SerializeField]
    private GameObject buttonNotSelected;
    [SerializeField]
    Color activeColor;
    [SerializeField]
    Color inActiveColor;
   public void SetInstructionPanelActiveInActive()
    {
        if(isActive == false)
        {
            isActive = true;
            instructionPanel.SetActive(true);
            buttonNotSelected.SetActive(false);
            buttonSelected.SetActive(true);
        }
        else
        {
            isActive = false;
            instructionPanel.SetActive(false);
            buttonNotSelected.SetActive(true);
            buttonSelected.SetActive(false);

        }
    }
}
