using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CoinSpawnManager : MonoBehaviour
{
    public delegate void LevelHandler();
    public static event LevelHandler OnLevelFinished;
    [SerializeField]
    private GameObject coinPrefab;

    private int coinsRemaining;

   // private readonly List<GameObject> coinList;

    [SerializeField]
    private float minX, maxX, minY, maxY;
    private float xPosition, yPosition;
    private TMPro.TextMeshProUGUI coinsRemainingText;

    // Start is called before the first frame update
    private void OnEnable()
    {
        CoinManager.OnCoinCaptured += ReduceCoinsRemainingOnCoinCapture;
    }

    private void OnDisable()
    {
        CoinManager.OnCoinCaptured -= ReduceCoinsRemainingOnCoinCapture;
    }
    void Start()
    {
        coinsRemaining = GameManager.Instance.MaxCoinsForLevel;
        coinsRemainingText = GameObject.Find("Canvas").transform.Find("CoinsRemaining").
            transform.GetComponent<TMPro.TextMeshProUGUI>();
        coinsRemainingText.text = coinsRemaining.ToString();
        for(int i=0; i < GameManager.Instance.MaxCoinsForLevel; i++)
        {
            xPosition = Random.Range(minX, maxX);
            yPosition = Random.Range(minY, maxY);
            Instantiate(coinPrefab, new Vector3(xPosition,yPosition,0f), Quaternion.identity);
            //coinList.Add(coin);
        }
    }

    private void ReduceCoinsRemainingOnCoinCapture()
    {
        coinsRemaining -= 1;
        coinsRemainingText.text = coinsRemaining.ToString();
        if (coinsRemaining == 0)
            OnLevelFinished?.Invoke();
    }

}
