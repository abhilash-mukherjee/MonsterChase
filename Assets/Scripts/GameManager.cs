using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject[] playerChoice;
    
    private int _charIndex;
    
    private static List<string> levelList = new List<string>();
    [SerializeField]
    private Animator gameOverAnimator;
    [SerializeField]
    private Animator levelAnimator;
    private bool startAlphaLerp = false;
    [HideInInspector]
    public int levelIndex = 0;
    [SerializeField]
    private float gameOverAnimationTime = 2f;
    [HideInInspector]
    public bool levelIndexAnimationPlayed = false;
    public int CharIndex
    {
        get { return _charIndex; }
        set { _charIndex = value; }
    }

    private int _maxHealthForLevel;
    public int MaxHealthForLevel
    {
        get { return _maxHealthForLevel; }
        set { _maxHealthForLevel = value; }
    }

    private int _maxBulletCountForLevel;
    public int MaxBulletCountForLevel
    {
        get { return _maxBulletCountForLevel; }
        set { _maxBulletCountForLevel = value; }
    }
    
    private float _maxMonsterSpeedForLevel;
    public float MaxMonsterSpeedForLevel
    {
        get { return _maxMonsterSpeedForLevel; }
        set { _maxMonsterSpeedForLevel = value; }
    }

    private float _minMonsterSpeedForLevel;
    public float MinMonsterSpeedForLevel
    {
        get { return _minMonsterSpeedForLevel; }
        set { _minMonsterSpeedForLevel = value; }
    }

    private float _maxTimeGapBetweenTwoMonsterSpawnsForLevel;
    public float MaxTimeGapBetweenTwoMonsterSpawnsForLevel
    {
        get { return _maxTimeGapBetweenTwoMonsterSpawnsForLevel; }
        set { _maxTimeGapBetweenTwoMonsterSpawnsForLevel = value; }
    }

    private float _minTimeGapBetweenTwoMonsterSpawnsForLevel;
    public float MinTimeGapBetweenTwoMonsterSpawnsForLevel
    {
        get { return _minTimeGapBetweenTwoMonsterSpawnsForLevel; }
        set { _minTimeGapBetweenTwoMonsterSpawnsForLevel = value; }
    }

    private int _maxCoinsForLevel;
    

    public int MaxCoinsForLevel
    {
        get { return _maxCoinsForLevel; }
        set { _maxCoinsForLevel = value; }
    }

    private void Awake()
    {
        for(int i = 0; i<10; i++)
        {
            levelList.Add($"Level {i + 1}");
        }
        if (Instance == null)
        {
            Instance = this; 
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
        
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
        Player.OnPlayerDestroyed += LoadGameOverOnPlayerDestroyed;
        CoinSpawnManager.OnLevelFinished += LoadNewLevelOnLevelFinished;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
        Player.OnPlayerDestroyed -= LoadGameOverOnPlayerDestroyed;
        CoinSpawnManager.OnLevelFinished -= LoadNewLevelOnLevelFinished;
    }




    private void LerpAlpha()
    {
        //you made this function for lerping the alpha of a canvas, and it worked properly.
        //However, later you decided not to use it as the canvas group's alpha was decreasing
        //but the alpha of it's child UI object's children were not decreasing
        if (startAlphaLerp == true)
        {
            float canvasAlpha = GameObject.FindGameObjectWithTag("GameCanvas").GetComponent<CanvasGroup>().alpha;
            if (canvasAlpha > 0)
            {
                GameObject.FindGameObjectWithTag("GameCanvas").GetComponent<CanvasGroup>().alpha
               = Mathf.Lerp(canvasAlpha, 0f, 0.5f );
            }
            else
            {
                startAlphaLerp = false;
            }

        }
    }
    


    public void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        if(scene.name == "Level")
        {
            if(levelIndex == 1)
            {
                _maxBulletCountForLevel = 10;
                _maxHealthForLevel = 3;
                _minMonsterSpeedForLevel = 5f;
                _maxMonsterSpeedForLevel = 10f;
                _minTimeGapBetweenTwoMonsterSpawnsForLevel = 2f;
                _maxTimeGapBetweenTwoMonsterSpawnsForLevel = 12f;
                _maxCoinsForLevel = 10;
            }

            else
            {
                _maxBulletCountForLevel += (levelIndex % 2 == 0 ? 0 : 1);
                _maxHealthForLevel += (levelIndex % 3 == 0 ? 0:1);
                _minMonsterSpeedForLevel += 0.5f;
                _maxMonsterSpeedForLevel += 0.5f;
                _minTimeGapBetweenTwoMonsterSpawnsForLevel = 2f;
                _maxTimeGapBetweenTwoMonsterSpawnsForLevel -= (_maxTimeGapBetweenTwoMonsterSpawnsForLevel> 3f ? 2f : 0f);
                _maxCoinsForLevel += 5;
            }
            StartCoroutine(DisplayLevelIndex());
            
            Debug.Log("GamePlay has been loaded and variables initialized");

        }
        
       
    }

    private IEnumerator DisplayLevelIndex()
    {
        transform.GetChild(1).gameObject.SetActive(true);
        levelAnimator.Play("LevelAppear");
        yield return new WaitUntil(() => levelIndexAnimationPlayed == true);
        levelIndexAnimationPlayed = false;
        Instantiate(playerChoice[CharIndex]);
    }

    private void LoadGameOverOnPlayerDestroyed()
    {
        
        GameObject.FindGameObjectWithTag("GameCanvas").SetActive(false);
        transform.GetChild(0).gameObject.SetActive(true);
        gameOverAnimator.SetTrigger("LoadGameOver");
    }
    public void GoToMainMenu()
    {
        gameOverAnimator.SetTrigger("LoadMainMenu");
        levelIndex = 0;
        StartCoroutine(LoadMeinMenuAfterPause(gameOverAnimationTime));
    }
   IEnumerator LoadMeinMenuAfterPause(float time)
    {
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene("MainMenu");
    }
    private void LoadNewLevelOnLevelFinished()
    {
        Debug.Log("New Level Will be loaded");
        string currentScene = SceneManager.GetActiveScene().name;
        if(currentScene.Equals("MainMenu"))
        {
            levelIndex = 1;
            SceneManager.LoadScene("Level");
            
        }
        else
        {
            AudioManager.Instance.PlaySound("LevelFinished");
            levelIndex += 1;
            SceneManager.LoadScene("Level");
        }
        
    }
    public void PauseGame()
    {
        Time.timeScale = 0;
        Debug.Log("Game Paused");
    }
    public void ResumeGame()
    {
        Time.timeScale = 1;
        Debug.Log("Game Resumed");
    }

}
