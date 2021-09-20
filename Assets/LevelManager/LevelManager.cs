using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;
using System;

public class LevelManager : MonoBehaviour
{
    #region Singleton
    private static LevelManager _Instance=null;
    public static LevelManager Instance
    {
        get
        {
            return _Instance;
        }
    }
    private void Awake()
    {
        if (_Instance == null)
        {
            _Instance = this;
        }
    }
    #endregion

    [SerializeField]
    private GameObject[] levels;
    [SerializeField]
    private Button replay, nextLevel;
    [SerializeField]
    private Text level;

    InputManager inputManager;
    GameObject currentLevel,finishedLevel;

    public delegate void onLevelFailed();
    public delegate void onLevelCompleted();

    public onLevelFailed LevelFailed;
    public onLevelCompleted LevelCompleted;

    private bool levelFail = false, levelWin = false;
    private int currentLevelIndex = 0;

    void Start()
    {
        LevelFailed += LevelFailedEventFunc;
        LevelCompleted += LevelCompletedEventFunc;

        inputManager = InputManager.Instance;
        currentLevelIndex = PlayerPrefs.GetInt("LastLevel");
        currentLevel= Instantiate(levels[currentLevelIndex], Vector3.zero,Quaternion.identity);
        level.text = "LEVEL " + (currentLevelIndex + 1);

    }
    void Update()
    {
        if (levelFail)
        {
            PlayAgainButtonVisibility();
        }
        if (levelWin)
        {
            NextLevelButtonVisibility();
        }
    }
    void LevelCompletedEventFunc()
    {
        levelWin = true;
    }
    void LevelFailedEventFunc()
    {
        levelFail = true;
    }
    #region Level Controller

    public void NextLevel() // Called in Editor.
    {
        levelWin = false;
        nextLevel.GetComponent<CanvasGroup>().alpha = 0;
        nextLevel.interactable = false;
        currentLevelIndex++;
        
        if (currentLevelIndex > levels.Length - 1)
        {
            currentLevelIndex = 0;
        }
        LevelCreate();
        
    }

    public void Replay() // Called in Editor.
    {
        levelFail = false;
        replay.GetComponent<CanvasGroup>().alpha = 0;
        replay.interactable = false;
        
        LevelCreate();
    }

    void LevelCreate()
    {

        PlayerPrefs.SetInt("LastLevel", currentLevelIndex);
        finishedLevel = currentLevel;

        Destroy(finishedLevel);
        currentLevel = Instantiate(levels[currentLevelIndex], Vector3.zero, Quaternion.identity);

        level.text = "LEVEL " + (currentLevelIndex + 1);
    }
    #endregion

    #region Button Visibility Controls

    void PlayAgainButtonVisibility()
    {
        replay.GetComponent<CanvasGroup>().alpha += Time.deltaTime;

        if (replay.GetComponent<CanvasGroup>().alpha >= 0.5f)
        {
            replay.GetComponent<Button>().interactable = true;
        }
    }
   void NextLevelButtonVisibility()
    {
            nextLevel.GetComponent<CanvasGroup>().alpha += Time.deltaTime;

            if (nextLevel.GetComponent<CanvasGroup>().alpha >= 0.5f)
            {
                nextLevel.GetComponent<Button>().interactable = true;
            }
    }
    #endregion
}
