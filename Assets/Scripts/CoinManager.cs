using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    public delegate void CoinCaptureHandler();
    public static event CoinCaptureHandler OnCoinCaptured;
    // Start is called before the first frame update
    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            FindObjectOfType<AudioManager>().PlaySound("CoinCapture");
            Destroy(gameObject);
            OnCoinCaptured?.Invoke();
        }
    }
}
