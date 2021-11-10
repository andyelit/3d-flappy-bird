using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;

    public static UIManager GetInstance()
    {
        return instance;
    }

    private void Awake()
    {
        instance = this;
    }

    [SerializeField] public Text scoreText;
    [SerializeField] public Text highScoreText;
    [SerializeField] public Button retryButton;
    [SerializeField] public GameObject scoreScreenObject;
    [SerializeField] public GameObject startScreenObject;
    [SerializeField] public GameObject gameOverScreenObject;


    public void Init()
    {
        highScoreText.text = "Highscore: " + PlayerPrefs.GetInt("highscore");

        Bird.GetInstance().OnDied += Bird_OnDied;
        Bird.GetInstance().OnStartedPlaying += Bird_OnStartedPlaying;

        startScreenObject.SetActive(true);

        retryButton.onClick.AddListener(RetryLevel);
    }

    private void RetryLevel()
    {
        SceneManager.LoadScene("Game");
    }

    private void Update()
    {
        scoreText.text = "Score: " + Level.GetInstance().GetPipesPassedCount();
    }

    private void Bird_OnStartedPlaying(object sender, EventArgs e)
    {
        startScreenObject.SetActive(false);
        scoreScreenObject.SetActive(true);
        gameOverScreenObject.SetActive(false);
    }

    private void Bird_OnDied(object sender, EventArgs e)
    {
        startScreenObject.SetActive(false);
        gameOverScreenObject.SetActive(true);

        SetNewHighscore(Level.GetInstance().GetPipesPassedCount());
    }

    public void SetNewHighscore(int score)
    {
        int currentHighscore = PlayerPrefs.GetInt("highscore");

        if (score > currentHighscore)
        {
            // New Highscore
            PlayerPrefs.SetInt("highscore", score);

            PlayerPrefs.Save();
        }
    }
}
