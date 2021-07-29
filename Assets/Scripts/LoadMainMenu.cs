using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadMainMenu : MonoBehaviour
{
    public void OnMainMenuLoaded()
    {
       gameObject.SetActive(false);
    }
}
