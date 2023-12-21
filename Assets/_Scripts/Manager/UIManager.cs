using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    private const string MAIN_MENU_SCENE = "MainMenu";

    [Header("References")]
    [SerializeField] private GameManager gameManager;

    [Header("Timer UI")]
    [SerializeField] private Image timerImage;

    [Header("Lives UI")]
    [SerializeField] private Image[] livesImage;

    [Header("Score UI")]
    [SerializeField] private TMP_Text scoreText;

    [Header("Game Over UI")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TMP_Text finalScoreText;
    [SerializeField] private TMP_Text hiScoreText;

    private void OnEnable()
    {
        gameManager.OnTimerUpdate += OnTimerUIUpdated;
        gameManager.OnLivesUpdate += (int _lives) =>
        {
            if (_lives >= 0)
                livesImage[_lives].gameObject.SetActive(false);
            else
            {
                _lives = 0;
                for (int i = 0; i < livesImage.Length; i++)
                {
                    livesImage[i].gameObject.SetActive(false);
                }
            }
        };
        gameManager.OnScoreUpdate += OnScoreUpdated;
        gameManager.OnGameOver += OnGameOver;
    }

    private void OnDisable()
    {
        gameManager.OnTimerUpdate -= OnTimerUIUpdated;
        gameManager.OnLivesUpdate -= (int _lives) =>
        {
            if (_lives > 0)
                livesImage[_lives].gameObject.SetActive(false);
            else
            {
                _lives = 0;
                for (int i = 0; i < livesImage.Length; i++)
                {
                    livesImage[i].gameObject.SetActive(false);
                }
            }
        };
        gameManager.OnScoreUpdate -= OnScoreUpdated;
        gameManager.OnGameOver -= OnGameOver;
    }

    //Updating Timer UI
    private void OnTimerUIUpdated(float _currentTime, float _initTime)
    {
        timerImage.fillAmount = _currentTime / _initTime;
    }

    //Updating Lives UI
    private void OnLivesUpdated(int _lives)
    {
        if(_lives >= 0)
            livesImage[_lives].gameObject.SetActive(false);
    }

    //Updating Score
    private void OnScoreUpdated(int _score)
    {
        scoreText.text = $"Score : {_score}";
    }

    //Game Over UI
    private void OnGameOver(int _score, int _hiScore)
    {
        gameOverPanel.SetActive(true);
        finalScoreText.text = $"Final Score : {_score}";
        hiScoreText.text = $"High Score : {_hiScore}";
    }

    //Move Scene
    public void MoveScene(bool _isRestarting)
    {
        if (_isRestarting)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        else
            SceneManager.LoadScene(MAIN_MENU_SCENE);
    }
}
